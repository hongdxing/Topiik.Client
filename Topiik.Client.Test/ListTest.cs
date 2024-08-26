using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Topiik.Client.Interface;

namespace Topiik.Client.Test
{
    [TestFixture]
    public class ListTest
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
        public void LPUSH_Existing_StringTypeKey_Should_Throw_Exception_DT_MISMATCH()
        {
            client.Set("k1", "v1");
            var value = client.Get("k1");
            Assert.That(value, Is.EqualTo("v1"));

            //Assert.That(client.LPush("k1", new List<string> { "v1", "v2" }), Throws.Exception);
            var msg = Assert.Throws<Exception>(() => client.LPush("k1", new List<string> { "v1", "v2" }))?.Message;

            Assert.That(msg, Is.EqualTo("DT_MISMATCH"));
        }

        [Test]
        public void LPOP_Should_Return_Value()
        {
            client.LPush("fruits", new List<string> { "Banana", "Apple", "BalckBerry" });

            var len = client.LLen("fruits");

            Assert.That(len, Is.EqualTo(3));
        }
    }
}
