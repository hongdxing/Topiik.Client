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

        [OneTimeSetUp]
        public void Setup()
        {
            serverAddr = "localhost:8301";
            connectionFactory = new ConnectionFactory(serverAddr);
            client = new TopiikClient(connectionFactory);
        }

        [Test]
        public void LPUSH_LPOP_Should_Return_Items_In_Reverse_Order()
        {
            client.Del(TestConsts.KEY_CITIES);
            client.LPush(TestConsts.KEY_CITIES, TestConsts.CITY_SHANGHAI, TestConsts.CITY_NEWYORK);
            List<string> cities = client.LPop(TestConsts.KEY_CITIES, 2);

            Assert.That(cities.Count, Is.EqualTo(2));
            Assert.That(cities[0], Is.EqualTo(TestConsts.CITY_NEWYORK));
            Assert.That(cities[1], Is.EqualTo(TestConsts.CITY_SHANGHAI));
        }

        [Test]
        public void LPUSHR_LPOPR_Should_Return_Items_In_Reverse_Order()
        {
            client.Del(TestConsts.KEY_CITIES);
            client.LPushR(TestConsts.KEY_CITIES, TestConsts.CITY_SHANGHAI, TestConsts.CITY_NEWYORK);
            List<string> cities = client.LPopR(TestConsts.KEY_CITIES, 2);

            Assert.That(cities.Count, Is.EqualTo(2));
            Assert.That(cities[0], Is.EqualTo(TestConsts.CITY_NEWYORK));
            Assert.That(cities[1], Is.EqualTo(TestConsts.CITY_SHANGHAI));
        }

        [Test]
        public void LPUSH_Existing_StringTypeKey_Should_Throw_Exception_TYPE_MISMATCH()
        {
            client.Set("k1", "v1");
            var value = client.Get("k1");
            Assert.That(value, Is.EqualTo("v1"));

            //Assert.That(client.LPush("k1", new List<string> { "v1", "v2" }), Throws.Exception);
            var msg = Assert.Throws<Exception>(() => client.LPush("k1", new string[] { "v1", "v2" }))?.Message;

            Assert.That(msg, Is.EqualTo("TYPE_MISMATCH"));
        }

        [Test]
        public void LPOP_Should_Return_Value()
        {
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var len = client.LLen("fruits");
            Assert.That(len, Is.EqualTo(3));

            var rslt = client.LPop("fruits");
            Assert.That(rslt, Is.Not.Null);
            Assert.That(rslt, Is.Not.Empty);
            Assert.That(rslt.Count, Is.EqualTo(1));
            Assert.That(rslt[0], Is.EqualTo("BlackBerry"));
        }

        #region LSET
        [Test]
        public void LSET_Should_Return_OK_If_Key_Exists_And_Index_IsValid()
        {
            client.Del(TestConsts.KEY_FRUITS);
            client.LPush(TestConsts.KEY_FRUITS, TestConsts.FRUIT_APPLE);
            client.LPush(TestConsts.KEY_FRUITS, TestConsts.FRUIT_BANANA);
            client.LPush(TestConsts.KEY_FRUITS, TestConsts.FRUIT_ORANGE);

            var len = client.LLen(TestConsts.KEY_FRUITS);
            Assert.That(len, Is.EqualTo(3));

            client.LSet(TestConsts.KEY_FRUITS, TestConsts.FRUIT_BLACK_BERRY, 1);

            len = client.LLen(TestConsts.KEY_FRUITS);
            Assert.That(len, Is.EqualTo(3));

            var fruits = client.LPop(TestConsts.KEY_FRUITS, 3);
            Assert.That(fruits.Count, Is.EqualTo(3));

            Assert.That(fruits[1], Is.EqualTo(TestConsts.FRUIT_BLACK_BERRY));
        }
        #endregion


        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
