﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.test
{
    public class GetControllerLeaderAddrTest
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
        public void GetControllerLeaderAddr_Should_Not_Empty()
        {
            /*
            var addr = topiikClient.GetControllerLeaderAddr(serverAddr);
            topiikClient.
            Console.WriteLine(addr);
            Assert.That(addr, Is.Not.Null);
            Assert.That(addr, Is.Not.Empty);
            */
        }
    }
}
