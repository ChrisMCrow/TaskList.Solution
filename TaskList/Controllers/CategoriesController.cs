using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/categories")]
        public ActionResult Index()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/categories/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create(string categoryName)
        {
          Category newCategory = new Category(categoryName);
          newCategory.Save();
          return RedirectToAction("Success", "Home");
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems();
            List<Item> allItems = Item.GetAll();
            model.Add("selectedCategory", selectedCategory);
            model.Add("categoryItems", categoryItems);
            model.Add("allItems", allItems);
            return View(model);
        }

        [HttpGet("/categories/{categoryId}/delete")]
        public ActionResult DeleteItem(int categoryId)
        {
            Category newCategory = Category.Find(categoryId);
            newCategory.Delete();
            return RedirectToAction("Index");
        }

        [HttpPost("/categories/{categoryId}/items/new")]
        public ActionResult AddItem(int categoryId, int itemId)
        {
            Category category = Category.Find(categoryId);
            Item item = Item.Find(itemId);
            category.AddItem(item);
            return RedirectToAction("Details",  new { id = categoryId });
        }
    }
}
