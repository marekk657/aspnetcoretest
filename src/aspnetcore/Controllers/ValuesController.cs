using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace aspnetcore.Controllers
{
    public class TestData
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<TestData> Get()
        {
            var mongod = new MongoClient();
            var db = mongod.GetDatabase("TestDatas");
            var datas = db.GetCollection<TestData>("TestDataCollection");

            var filter = new BsonDocument();
            using (var cursor = datas.FindSync(filter))
            {
                while (cursor.MoveNext())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        yield return document;
                    }
                }
            }

        }

        [HttpGet]
        public void Get(ObjectId id, string name)
        {
            var mongod = new MongoClient();
            var db = mongod.GetDatabase("TestDatas");
            var datas = db.GetCollection<TestData>("TestDataCollection");

            var testData = new TestData { Id = id, Name = name };
            datas.InsertOne(testData);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
