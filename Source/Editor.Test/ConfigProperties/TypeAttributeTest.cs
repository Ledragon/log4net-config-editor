﻿// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class TypeAttributeTest
    {
        private TypeAttribute mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()), AppenderDescriptor.Async);
        }

        [Test]
        public void Name_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()));

            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()));

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void IsReadOnly_ShouldBeFalse_RegularCtor()
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()));

            Assert.IsFalse(mSut.IsReadOnly);
        }

        [Test]
        public void Name_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [Test]
        public void IsReadOnly_ShouldBeTrue_AppenderDescriptorCtor()
        {
            Assert.IsTrue(mSut.IsReadOnly);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("type=\"\"")]
        public void Load_ShouldNotLoadType_RegularCtor(string xml)
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"ColoredConsoleAppender\" {xml}>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void Load_ShouldLoadCorrectType()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"ColoredConsoleAppender\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("log4net.Appender.ColoredConsoleAppender", mSut.Value);
        }

        [TestCase("type=\"\"")]
        [TestCase("")]
        public void Load_ShouldMaintainType_FromCtor(string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"ColoredConsoleAppender\" {type}>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [Test]
        public void Save_ShouldSaveValueToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            const string value = "type";
            mSut.Value = value;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(value, appender.Attributes["type"].Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveValueToAttribute_WhenValueIsNullOrEmpty(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = value;

            mSut.Save(xmlDoc, appender);

            Assert.IsNull(appender.Attributes["type"]);
        }
    }
}
