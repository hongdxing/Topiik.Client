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
            client.Del(Consts.KEY_CITIES);
            client.LPush(Consts.KEY_CITIES, Consts.CITY_SHANGHAI, Consts.CITY_NEWYORK);
            List<string> cities = client.LPop(Consts.KEY_CITIES, 2);

            Assert.That(cities.Count, Is.EqualTo(2));
            Assert.That(cities[0], Is.EqualTo(Consts.CITY_NEWYORK));
            Assert.That(cities[1], Is.EqualTo(Consts.CITY_SHANGHAI));
        }

        [Test]
        public void LPUSHR_LPOPR_Should_Return_Items_In_Reverse_Order()
        {
            client.Del(Consts.KEY_CITIES);
            client.LPushR(Consts.KEY_CITIES, Consts.CITY_SHANGHAI, Consts.CITY_NEWYORK);
            List<string> cities = client.LPopR(Consts.KEY_CITIES, 2);

            Assert.That(cities.Count, Is.EqualTo(2));
            Assert.That(cities[0], Is.EqualTo(Consts.CITY_NEWYORK));
            Assert.That(cities[1], Is.EqualTo(Consts.CITY_SHANGHAI));
        }

        [Test]
        public void LPUSH_Existing_StringTypeKey_Should_Throw_Exception_DT_MISMATCH()
        {
            client.Set("k1", "v1");
            var value = client.Get("k1");
            Assert.That(value, Is.EqualTo("v1"));

            //Assert.That(client.LPush("k1", new List<string> { "v1", "v2" }), Throws.Exception);
            var msg = Assert.Throws<Exception>(() => client.LPush("k1", new string[] { "v1", "v2" }))?.Message;

            Assert.That(msg, Is.EqualTo("DT_MISMATCH"));
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

        [Test]
        public void LRange_Start0_EndN_Should_Return_All_Elements()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LRange("fruits", 0, 3);

            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N));
        }

        [Test]
        public void LRange_Start_On_Right_Of_End_Should_Return_Zero_Element()
        {
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LRange("fruits", 2, 1);

            Assert.That(fruits.Count, Is.EqualTo(0));

            fruits = client.LRange("fruits", -1, -2);

            Assert.That(fruits.Count, Is.EqualTo(0));
        }

        [Test]
        public void LRange_End_Minus_One_Equivalent_To_End_N()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LRange("fruits", 0, -1);
            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N));
        }

        [Test]
        public void LRange_End_Bigger_Than_N_Equivalent_To_End_N()
        {
            var N = 3;
            client.Del("fruits");
            client.LPush("fruits", new string[] { "Banana", "Apple", "BlackBerry" });

            var fruits = client.LRange("fruits", 0, N + new Random().Next());

            Assert.That(fruits, Is.Not.Null);
            Assert.That(fruits, Is.Not.Empty);
            Assert.That(fruits.Count, Is.EqualTo(N));
        }

        #region LSET
        [Test]
        public void LSET_Should_Return_OK_If_Key_Exists_And_Index_IsValid()
        {
            client.Del(Consts.KEY_FRUITS);
            client.LPush(Consts.KEY_FRUITS, Consts.FRUIT_APPLE);
            client.LPush(Consts.KEY_FRUITS, Consts.FRUIT_BANANA);
            client.LPush(Consts.KEY_FRUITS, Consts.FRUIT_ORANGE);

            var len = client.LLen(Consts.KEY_FRUITS);
            Assert.That(len, Is.EqualTo(3));

            client.LSet(Consts.KEY_FRUITS, Consts.FRUIT_BLACK_BERRY, 1);

            len = client.LLen(Consts.KEY_FRUITS);
            Assert.That(len, Is.EqualTo(3));

            var fruits = client.LPop(Consts.KEY_FRUITS, 3);
            Assert.That(fruits.Count, Is.EqualTo(3));

            Assert.That(fruits[1], Is.EqualTo(Consts.FRUIT_BLACK_BERRY));
        }
        #endregion


        [OneTimeTearDown]
        public void TearDown()
        {
            client.Close();
        }
    }
}
