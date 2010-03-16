using System;
using NUnit.Framework;

namespace TW.Core.Test
{
    [TestFixture]
    public class FactoryTest
    {
        public interface ITestClass
        {
        }

        private class TestClass1 : ITestClass
        {
            public int Var1 { get; private set; }
            public int Var2 { get; private set; }
            public int Var3 { get; private set; }

            public TestClass1(int arg1, int arg2, int arg3)
            {
                Var1 = arg1;
                Var2 = arg2;
                Var3 = arg3;
            }
        }

        private class TestClass2 : ITestClass
        {
            public int Var1 { get; private set; }
            public int Var2 { get; private set; }

            public TestClass2(int arg1, int arg2)
            {
                Var1 = arg1;
                Var2 = arg2;
            }
        }

        private class TestClass3 : ITestClass
        {
            public string GetClassName()
            {
                return "TestClass3";
            }
        }

        private object onCreateTestClass1(Type type, object[] arguments)
        {
            return new TestClass1((int)arguments[0], (int)arguments[1], (int)arguments[2]);
        }

        private object onCreateTestClass2(Type type, object[] arguments)
        {
            return new TestClass2((int)arguments[0], (int)arguments[1]);
        }

        private object onCreateTestClass3(Type type, object[] arguments)
        {
            return new TestClass3();
        }


        private readonly FactoryMock instance = new FactoryMock();

        [SetUp]
        public void SetUp()
        {
            instance.Add(typeof(TestClass1), onCreateTestClass1);
            instance.Add(typeof(TestClass2), onCreateTestClass2);
            instance.Add(typeof(TestClass3), onCreateTestClass3);
        }

        [Test]
        public void ShouldAbleToInstantiateTestClass1()
        {
            var testClass1 = (TestClass1)instance.CreateObject(typeof(TestClass1), 1, 2, 3);
            Assert.AreEqual(1, testClass1.Var1);
            Assert.AreEqual(2, testClass1.Var2);
            Assert.AreEqual(3, testClass1.Var3);
        }

        [Test]
        public void ShouldAbleToInstantiateTestClass2()
        {
            var testClass2 = (TestClass2)instance.CreateObject(typeof(TestClass2), 1, 2);
            Assert.AreEqual(1, testClass2.Var1);
            Assert.AreEqual(2, testClass2.Var2);
        }

        [Test]
        public void ShouldAbleToInstantiateTestClass3()
        {
            var testClass3 = (TestClass3)instance.CreateObject(typeof(TestClass3));
            Assert.AreEqual("TestClass3", testClass3.GetClassName());
        }
    }
}
