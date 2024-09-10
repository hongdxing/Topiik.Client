using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.StringTest
{
    [TestFixture]
    public class GetTest
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
        public void Get_Should_Return_null_If_Key_Not_Exists()
        {
            var key = "Not-Exists";
            client.Del(key);
            var result = client.Get(key);

            Assert.That(result, Is.Null);
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
