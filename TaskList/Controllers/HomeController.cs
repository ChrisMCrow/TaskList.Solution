using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            List<Item> allItems = Item.GetAll();
            return View(allItems);
        }

        [HttpGet("/home/success")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
