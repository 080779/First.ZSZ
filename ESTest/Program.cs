using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticConnection client = new ElasticConnection("localhost", 9200);
            SearchCommand cmd = new SearchCommand("verycd", "items");
            var query = new QueryBuilder<VerycdItem>()
            .Query(b =>
             b.Bool(m =>
            //并且关系
            m.Should(t =>
             //分词的最小单位或关系查询
             t.QueryString(t1 => t1.DefaultField("content").Query("周星驰"))
             )
             .Should(t => t.Term(t1  => t1.Field("category2").Value("喜剧")))
             )
            ).Size(100)
            /*
            //分页
            .From(0)
            .Size(10)
            //排序
            // .Sort(c => c.Field("age", SortDirection.desc))
            //添加高亮
            .Highlight(h => h
            .PreTags("<b>")
            .PostTags("</b>")
            .Fields(
             f => f.FieldName("Name").Order(HighlightOrder.score)
             )
            )*/
            .Build();
            var result = client.Post(cmd, query);
            var serializer = new JsonNetSerializer();
            var searchResult = serializer.ToSearchResult<VerycdItem>(result);   //普通遍历获取 list.Documents 就可以了，如何获取“
            foreach(var doc in searchResult.Documents)
            {
                Console.WriteLine(doc.title+","+doc.category1+","+doc.category2);
            }
            Console.WriteLine(searchResult.hits.total);
            Console.ReadKey();
        }
        static void Main1(string[] args)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=d:/verycd.sqlite3.db"))                
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from verycd";
                    using (var reader = cmd.ExecuteReader())
                    {
                        ElasticConnection client = new ElasticConnection("localhost", 9200);
                        
                        var serializer = new JsonNetSerializer();                        
                        while(reader.Read())
                        {                            
                            long verycdid =reader.GetInt64(reader.GetOrdinal("verycdid"));
                            string title = reader.GetString(reader.GetOrdinal("title"));
                            string status = reader.GetString(reader.GetOrdinal("status"));
                            string brief = reader.GetString(reader.GetOrdinal("brief"));
                            string pubtime = reader.GetString(reader.GetOrdinal("pubtime"));
                            string updtime = reader.GetString(reader.GetOrdinal("updtime"));
                            string category1 = reader.GetString(reader.GetOrdinal("category1"));
                            string category2 = reader.GetString(reader.GetOrdinal("category2"));
                            string ed2k = reader.GetString(reader.GetOrdinal("ed2k"));
                            string content = reader.GetString(reader.GetOrdinal("content"));
                            string related = reader.GetString(reader.GetOrdinal("related"));

                            VerycdItem item = new VerycdItem();
                            item.verycdid = verycdid;
                            item.title = title;
                            item.status = status;
                            item.brief = brief;
                            item.pubtime = pubtime;
                            item.updtime = updtime;
                            item.category1 = category1;
                            item.category2 = category2;
                            item.ed2k = ed2k;
                            item.content = content;
                            item.related = related;
                            try
                            {
                                //第一个参数相当于“数据库”，第二个参数相当于“表”，第三个参数相当于“主键”
                                IndexCommand Indexcmd = new IndexCommand("verycd", "items", item.verycdid.ToString());
                                //Put()第二个参数是要插入的数据
                                OperationResult result = client.Put(Indexcmd, serializer.Serialize(item));
                                var indexResult = serializer.ToIndexResult(result.Result);
                                if (indexResult.created)
                                {
                                    Console.WriteLine(verycdid + "创建了");
                                }
                                else
                                {
                                    Console.WriteLine(verycdid + "没创建" + indexResult.error);
                                }
                            }
                            catch(Exception ex)
                            {
                                continue;
                            }
                        }
                        Console.ReadKey();                
                    }
                }
            }
        }
    }
}
