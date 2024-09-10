using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.StringTest
{
    [TestFixture]
    public class IncrTest
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
        public void INCR_Should_Able_To_Increase_Non_Exists_Key()
        {
            client.Del("NonExistsKey");

            var result = client.Incr("NonExistsKey");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void INCR_Should_Throw_TYPE_MISMATCH_If_Existing_Key_Value_Cannot_Convert_To_Integer()
        {
            var result = client.Set("user:00001", "Tom");
            Assert.That(result, Is.EqualTo("OK"));

            var message = Assert.Throws<Exception>(() => client.Incr("user:00001"), "")?.Message;
            Assert.That(message, Is.EqualTo(TestConsts.ERR_TYPE_MISMATCH));
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
