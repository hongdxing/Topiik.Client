using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.StringTests
{
    [TestFixture]
    public class GetTest
    {
        private ITopiikClient client;
        private IConnectionFactory connectionFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            connectionFactory = new ConnectionFactory(TestConsts.SERVERS);
            client = new TopiikClient(connectionFactory);
        }


        [Test]
        public void GET_Should_Return_Value_If_String_Key_Exists()
        {
            client.Set("k1", "v1", new Client.Arg.StrSetArg());

            var value = client.Get("k1");

            Assert.That(value, Is.EqualTo("v1"));
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
