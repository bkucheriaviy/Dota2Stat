﻿using System;
using NUnit.Framework;
using Moq;

namespace D2S.Service.Tests
{
    [TestFixture]
    [Ignore("")]
    public class DataServiceTests
    {
        [Ignore("Not implemented yet")]
        [Test]
        [TestCase("11")]
        [TestCase("12")]
        public void Test_Test1_ThatTeststTest(string number)
        {
            //given
            //when 
            //then
            Assert.That(number, Is.EqualTo("12"));
        }
    }
}
