using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PZone.Xrm.Testing;


namespace PZone.Xrm.Testing_Tests
{
    [TestClass]
    public class EntityReferenceExtensionsTest
    {
        [TestMethod]
        public void ZeroIndent()
        {
            var entityRef = new EntityReference("new_entity", new Guid("00000000-0000-0000-0000-000000000001"));
            var actual = entityRef.ToPlainString();
            Assert.AreEqual("new_entity | 00000000-0000-0000-0000-000000000001 | ", actual);

            entityRef.Name = "Test";
            actual = entityRef.ToPlainString();
            Assert.AreEqual("new_entity | 00000000-0000-0000-0000-000000000001 | Test", actual);
        }


        [TestMethod]
        public void Indent()
        {
            var entityRef = new EntityReference("new_entity", new Guid("00000000-0000-0000-0000-000000000001"));
            var actual = entityRef.ToPlainText(4);
            Assert.AreEqual(@"    LogicalName = new_entity
    ID = 00000000-0000-0000-0000-000000000001
    Name =", actual);

            entityRef.Name = "Test";
            actual = entityRef.ToPlainText(4);
            Assert.AreEqual(@"    LogicalName = new_entity
    ID = 00000000-0000-0000-0000-000000000001
    Name = Test", actual);
        }
    }
}