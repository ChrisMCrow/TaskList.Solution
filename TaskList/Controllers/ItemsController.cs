using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class ItemsController : Controller
    {
        [HttpGet("/items")]
        public ActionResult Index()
        {
            List<Item> allItems = Item.GetAll();
            return View(allItems);
        }

        [HttpGet("/items/new")]
        public ActionResult CreateForm(int categoryId)
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpPost("/items")]
        public ActionResult Create(string description, string dueDate)
        {
          Item newItem = new Item(description, dueDate);
          newItem.Save();
          return RedirectToAction("Success", "Home");
        }

        [HttpGet("/items/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Item selectedItem = Item.Find(id);
            List<Category> itemCategories = selectedItem.GetCategories();
            List<Category> allCategories = Category.GetAll();
            model.Add("selectedItem", selectedItem);
            model.Add("itemCategories", itemCategories);
            model.Add("allCategories", allCategories);
            return View(model);
        }

        [HttpGet("/items/{id}/delete")]
        public ActionResult DeleteItem(int id)
        {
            Item newItem = Item.Find(id);
            newItem.Delete();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("/items/{itemId}/categories/new")]
        public ActionResult AddCategory(int itemId, int categoryId)
        {
            Item item = Item.Find(itemId);
            Category category = Category.Find(categoryId);
            item.AddCategory(category);
            return RedirectToAction("Details",  new { id = itemId });
        }
    }
}
