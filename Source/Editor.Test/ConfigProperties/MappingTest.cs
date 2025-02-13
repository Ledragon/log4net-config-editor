﻿// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Models;
using Editor.Windows;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class MappingTest
    {
        private XmlDocument mXmlDoc;
        private IMessageBoxService mMessageBoxService;
        private Mapping mSut;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml("<appender>\r\n" +
                            "<mapping>\r\n" +
                            " <level value=\"ERROR\" />\r\n" +
                            " <foreColor value=\"DarkRed\" />\r\n" +
                            " <backColor value=\"White\" />\r\n" +
                            "</mapping>\r\n" +
                            "<mapping>\r\n" +
                            " <level value=\"WARN\" />\r\n" +
                            " <foreColor value=\"Yellow\" />\r\n" +
                            "</mapping>\r\n" +
                            "<mapping>\r\n" +
                            " <level value=\"info\" />\r\n" +
                            " <backColor value=\"White\" />\r\n" +
                            "</mapping>\r\n" +
                            "<mapping>\r\n" +
                            " <level value=\"\" />\r\n" +
                            " <foreColor value=\"Blue\" />\r\n" +
                            "</mapping>\r\n" +
                            "</appender>");

            mMessageBoxService = Substitute.For<IMessageBoxService>();

            IConfiguration configuration = Substitute.For<IConfiguration>();
            configuration.ConfigXml.Returns(mXmlDoc);

            mSut = new Mapping(new ReadOnlyCollection<IProperty>(new List<IProperty>()), configuration, mMessageBoxService);
        }

        [Test]
        public void Add_ShouldShowElementWindow()
        {
            mSut.Add.Execute(null);

            mMessageBoxService.Received(1).ShowWindow(Arg.Any<ElementWindow>());
        }

        [Test]
        public void Remove_ShouldRemoveModel()
        {
            mSut.Load(mXmlDoc.FirstChild);

            //Test sanity check
            Assert.AreEqual(4, mSut.Mappings.Count);

            mSut.Mappings.First().Remove.Execute(null);

            Assert.AreEqual(3, mSut.Mappings.Count);
        }

        [Test]
        public void Load_ShouldLoadMappingCorrectly()
        {
            IEnumerable<MappingModel> expectedMappings = new[]
            {
                new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[0]),
                new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[1]),
                new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[2]),
                new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[3])
            };

            mSut.Load(mXmlDoc.FirstChild);

            CollectionAssert.AreEquivalent(expectedMappings, mSut.Mappings);
        }

        [Test]
        public void Save_ShouldSaveEachMappingCorrectly()
        {
            mSut.Mappings.Add(new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[0]));
            mSut.Mappings.Add(new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[1]));
            mSut.Mappings.Add(new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[2]));
            mSut.Mappings.Add(new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild.ChildNodes[3]));

            XmlElement appender = mXmlDoc.CreateElement("appender");

            mSut.Save(mXmlDoc, appender);

            XmlNodeList mappings = appender.SelectNodes("/mapping");

            CollectionAssert.AreEquivalent(mSut.Mappings.Select(m => m.Node), mappings);
        }

        private void Dummy(MappingModel mappingModel)
        {
        }
    }
}
