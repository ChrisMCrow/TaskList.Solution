using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskList.Models;

namespace TaskList.Tests
{
    [TestClass]
    public class ItemTest : IDisposable
    {
        public void Dispose()
        {
            Item.DeleteAll();
        }
        public void ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;userid=root;password=root;port=8889;database=todo_test;";
        }
        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {
            //Arrange
            //Act
            int result = Item.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }
    }
}
