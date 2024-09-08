using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Topiik.Client.Test
{
    [TestFixture]
    public class KeyTest
    {
        private string serverAddr;
        private ITopiikClient client;
        private IConnectionFactory connectionFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            serverAddr = "localhost:8301";
            connectionFactory = new ConnectionFactory(serverAddr);
            client = new TopiikClient(connectionFactory);
        }

        [Test]
        public void Exists_Should_Return_True_If_Key_Exists()
        {
            client.Del("existKey");
            client.Del("existList");
            client.Set("existKey", "Topiik");
            client.LPush("existList", new string[] { "Banana", "Apple", "Orange" });

            var isExist = client.Exists("existKey");
            Assert.That(isExist, Is.True);

            isExist = client.Exists("existList");
            Assert.That(isExist, Is.True);
        }

        [Test]
        public void Exists_Should_Return_All_True_If_All_Keys_Exists()
        {
            client.Del("existKey");
            client.Del("existList");
            client.Set("existKey", "Topiik");
            client.LPush("existList", new string[] { "Banana", "Apple", "Orange" });

            var result = client.Exists("existKey", "existList");
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ToArray()[0], Is.EqualTo(true));
            Assert.That(result.ToArray()[1], Is.EqualTo(true));
        }

        [Test]
        public void Exists_Should_Return_False_If_The_Key_Not_Exists()
        {
            client.Del("existKey");
            client.Del("existList");
            client.Del("NonExistKey");
            client.Set("existKey", "Topiik");
            client.LPush("existList", new string[] { "Banana", "Apple", "Orange" });

            var result = client.Exists("existKey", "existList", "NonExistKey");
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result.ToArray()[0], Is.EqualTo(true));
            Assert.That(result.ToArray()[1], Is.EqualTo(true));
            Assert.That(result.ToArray()[2], Is.EqualTo(false));
        }

        [Test]
        public void Ttl_Should_Return_Minus_Two_If_Key_Not_Exist()
        {
            client.Del("NonExistKey");

            var result = client.Ttl("NonExistKey");
            Assert.That(result, Is.EqualTo(-2));
        }

        [Test]
        public void Ttl_Should_Return_Minus_One_If_Ttl_Not_Set()
        {
            client.Set("user:00001", "Tom");
            client.LPush("cities", "Singapore", "Shanghai");

            var result = client.Ttl("user:00001");
            Assert.That(result, Is.EqualTo(-1));
            result = client.Ttl("cities");
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void Ttl_Should_Return_Minus_Two_If_Expired_And_Key_Should_Be_Deleted()
        {
            client.Set("user:00001", "Tom");
            client.Ttl("user:00001", 1);
            var value = client.Get("user:00001");
            Assert.That(value, Is.EqualTo("Tom"));
            //Thread.Sleep(1500);

            Assert.That(() => client.Ttl(Consts.KEY_USER_00001), Is.EqualTo(-2).After(1000));

            Assert.That(() => client.Get("user:00001"), Is.EqualTo(Consts.RES_NIL).After(1000));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
