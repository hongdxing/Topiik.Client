using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test
{
    [TestFixture]
    public class ListTest
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
    }
}
