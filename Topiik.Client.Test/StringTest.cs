using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Topiik.Client.Interface;

namespace Topiik.Client.Test
{
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
            var value = ((IStringCommand)topiikClient).Get("k1");

            Assert.That(value, Is.EqualTo("v1"));
        }
    }
}
