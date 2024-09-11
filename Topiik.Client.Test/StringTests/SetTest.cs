using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.StringTests
{
    [TestFixture]
    public class SetTest
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

        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
