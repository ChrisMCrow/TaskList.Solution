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
            List<Category> allCategories = Category.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems();
            model.Add("category", selectedCategory);
            model.Add("items", categoryItems);
            return View(model);
        }

        [HttpPost("/items")]
        public ActionResult CreateItem(string itemDescription, string dueDate, string category)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category foundCategory = Category.Find(categoryId);
            Item newItem = new Item(itemDescription, dueDate);
            newItem.Save();
            List<Item> categoryItems = foundCategory.GetItems();
            model.Add("items", categoryItems);
            model.Add("category", foundCategory);
            return View("Details", model);
        }

        [HttpGet("/categories/{categoryId}/update")]
        public ActionResult UpdateForm(int categoryId)
        {
            Category newCategory = Category.Find(categoryId);
            return View(newCategory);
        }

        [HttpPost("/categories/{categoryId}/update")]
        public ActionResult Update(int categoryId, string newName)
        {
            Category newCategory = Category.Find(categoryId);
            newCategory.Edit(newName);
            return RedirectToAction("Index");
        }

        [HttpGet("/categories/{categoryId}/delete")]
        public ActionResult DeleteItem(int categoryId)
        {
            Category newCategory = Category.Find(categoryId);
            newCategory.Delete();
            return RedirectToAction("Index");
        }
    }
}
