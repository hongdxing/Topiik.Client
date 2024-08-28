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
    public class StringTest
    {
        private string serverAddr;
        private ITopiikClient topiikClient;
        private IConnectionFactory connectionFactory;

        [SetUp]
        public void Setup()
        {
            serverAddr = "localhost:8301";
            connectionFactory = new ConnectionFactory(serverAddr);
            topiikClient = new TopiikClient(connectionFactory);
        }

        [Test]
        public void GET_Should_Return_Value()
        {
            var client = (IStringCommand)topiikClient;
            client.Set("k1", "v1", new Clien.Arg.StrSetArg());

            var value = client.Get("k1");

            Assert.That(value, Is.EqualTo("v1"));
        }

        [Test]
        public void GETM_Should_Return_Value()
        {
            var client = (IStringCommand)topiikClient;
            var keyValues = new Dictionary<string, string>
            {
                { "mk1", "mv1"},
                { "mk2", "mv2"},
                {"mk3","mv3"}

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

        /*
         * SET will overwrite any existing key even the existing key type is not String type
         */
        [Test]
        public void SET_Will_Overwrite_Existing_List_Key()
        {
            /* LPUSH fruits apple orange banana */

            /* Assert LPUSH succeed */

            /* SET fruits abc */

            /* GET fruits */

            /* Assert result ecquals to abc */
        }
    }
}
