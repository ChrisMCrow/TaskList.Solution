using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class ItemsController : Controller
    {
        [HttpGet("/categories/{categoryId}/items/new")]
        public ActionResult CreateForm(int categoryId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            return View(category);
        }

        [HttpGet("/categories/{categoryId}/items/{itemId}")]
        public ActionResult Details(int categoryId, int itemId)
        {
            Item item = Item.Find(itemId);
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category category = Category.Find(categoryId);
            model.Add("item", item);
            model.Add("category", category);
            return View(model);
        }

        [HttpGet("/allitems")]
        public ActionResult Index()
        {
            List<Item> allItems = Item.GetAll();
            return View(allItems);
        }

        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Item thisItem = Item.Find(id);
            return View(thisItem);
        }

        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id, string newDescription)
        {
            Item thisItem = Item.Find(id);
            thisItem.Edit(newDescription);
            return RedirectToAction("Index");
        }

        [HttpGet("/items/{id}/delete")]
        public ActionResult DeleteItem(int id)
        {
            Item newItem = Item.Find(id);
            newItem.Delete();
            return RedirectToAction("Index");
        }
    }
}
