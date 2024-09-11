using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Topiik.Client.Test.ListTests
{
    [TestFixture]
    public class LsliceTest
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
        public void LSlice_Start0_EndN_Should_Return_All_Elements()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LSlice("fruits", 0, 3);

            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N));
        }

        [Test]
        public void LSlice_Start_On_Right_Of_End_Should_Return_Zero_Element()
        {
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LSlice("fruits", 2, 1);

            Assert.That(fruits.Count, Is.EqualTo(0));

            fruits = client.LSlice("fruits", -1, -2);

            Assert.That(fruits.Count, Is.EqualTo(0));
        }

        [Test]
        public void LSlice_End_Minus_One_Equivalent_To_End_N()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LSlice("fruits", 0, -1);
            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N-1));
        }

        [Test]
        public void LSlice_End_Bigger_Than_N_Equivalent_To_End_N()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LSlice("fruits", 0, N + new Random().Next());

            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N));
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
