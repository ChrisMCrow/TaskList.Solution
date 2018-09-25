using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Tests
{
    [TestClass]
    public class ItemTest : IDisposable
    {
        public void ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;userid=root;password=root;port=8889;database=todo_test;";
        }
        public void Dispose()
        {
            Item.DeleteAll();
            Category.DeleteAll();
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

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        {
            // Arrange, Act
            Item firstItem = new Item("Mow the lawn", "", 1);
            Item secondItem = new Item("Mow the lawn", "", 1);

            // Assert
            Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn", "", 1);

            //Act
            testItem.Save();
            List<Item> result = Item.GetAll();
            List<Item> testList = new List<Item>{testItem};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn", "", 1);

            //Act
            testItem.Save();
            Item savedItem = Item.GetAll()[0];

            int result = savedItem.GetId();
            int testId = testItem.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn", "2018-09-23", 1);
            testItem.Save();

            //Act
            Item foundItem = Item.Find(testItem.GetId());

            //Assert
            Assert.AreEqual(testItem, foundItem);
        }

        [TestMethod]
        public void Edit_UpdatesItemInDatabase_String()
        {
            //Arrange
            string firstDescription = "walk the dog";
            Item testItem = new Item (firstDescription);
            testItem.Save();
            string secondDescription = "Mow the lawn";

            //Act
            testItem.Edit(secondDescription);

            string result = Item.Find(testItem.GetId()).GetDescription();

            //Assert
            Assert.AreEqual(secondDescription, result);
        }

        [TestMethod]
        public void AddCategory_AddsCategoryToItem_CategoryList()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn");
            testItem.Save();

            Category testCategory = new Category("Home stuff");
            testCategory.Save();

            //Act
            testItem.AddCategory(testCategory);

            List<Category> result = testItem.GetCategories();
            List<Category> testList = new List<Category>{testCategory};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetCategories_ReturnsAllItemCategories_CategoryList()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn");
            testItem.Save();

            Category testCategory1 = new Category("Home stuff");
            testCategory1.Save();

            Category testCategory2 = new Category("Work stuff");
            testCategory2.Save();

            //Act
            testItem.AddCategory(testCategory1);
            List<Category> result = testItem.GetCategories();
            List<Category> testList = new List<Category> {testCategory1};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesItemAssociationsFromDatabase_ItemList()
        {
            //Arrange
            Category testCategory = new Category("Home stuff");
            testCategory.Save();

            string testDescription = "Mow the lawn";
            Item testItem = new Item(testDescription);
            testItem.Save();

            //Act
            testItem.AddCategory(testCategory);
            testItem.Delete();

            List<Item> resultCategoryItems = testCategory.GetItems();
            List<Item> testCategoryItems = new List<Item> {};

            //Assert
            CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
        }
    }
}
