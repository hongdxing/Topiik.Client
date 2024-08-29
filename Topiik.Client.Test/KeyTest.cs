using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test
{
    [TestFixture]
    public class KeyTest
    {
        private string serverAddr;
        private ITopiikClient client;
        private IConnectionFactory connectionFactory;

        [SetUp]
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
            Assert.That(isExist,Is.True);
        }

        [Test]
        public void Exists_Should_Return_N_If_N_Keys_Exists()
        {
            int N = 2;
            client.Del("existKey");
            client.Del("existList");
            client.Set("existKey", "Topiik");
            client.LPush("existList", new string[] { "Banana", "Apple", "Orange" });

            var count = client.Exists("existKey", "existList");
            Assert.That(count, Is.EqualTo(N));
        }

        [Test]
        public void Exists_Should_Return_Actual_N_If_Some_Keys_Not_Exists()
        {
            int N = 2;
            client.Del("existKey");
            client.Del("existList");
            client.Del("NonExistKey");
            client.Set("existKey", "Topiik");
            client.LPush("existList", new string[] { "Banana", "Apple", "Orange" });

            var count = client.Exists("existKey", "existList", "NonExistKey");
            Assert.That(count, Is.EqualTo(N));
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

    }
}
