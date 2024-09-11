using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.StringTests
{
    [TestFixture]
    public class GetmTest
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
        public void GETM_Should_Return_Value()
        {
            var keyValues = new Dictionary<string, string>
            {
                { "mk1", "mv1"},
                { "mk2", "mv2"},
                { "mk3", "mv3"}

            };
            client.SetM(keyValues);

            var mv1 = client.Get("mk1");
            var mv2 = client.Get("mk2");
            var mv3 = client.Get("mk3");

            Assert.That(mv1, Is.EqualTo("mv1"));
            Assert.That(mv1, Is.EqualTo("mv1"));
            Assert.That(mv1, Is.EqualTo("mv1"));

            List<string> list = client.GetM(new string[] { "mk2", "mk1", "mk3" });
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0], Is.EqualTo("mv2"));
            Assert.That(list[1], Is.EqualTo("mv1"));
            Assert.That(list[2], Is.EqualTo("mv3"));
        }



        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
