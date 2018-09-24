using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class ItemsController : Controller
    {
        [HttpGet("/items/new")]
        public ActionResult CreateForm(int categoryId)
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/items/{itemId}")]
        public ActionResult Details(int categoryId, int itemId)
        {
            Item newItem = Item.Find(itemId);
            List<Category> itemsCategories = newItem.GetCategories();
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("items", newItem);
            model.Add("categories", itemsCategories);
            return View(model);
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
