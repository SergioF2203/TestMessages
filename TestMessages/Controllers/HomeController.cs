using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestMessages.Models.Mess;
using TestMessages.Models.Actions;
using System.Configuration;

namespace TestMessages.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            if (HttpContext.Session["messageList"] == null)
                HttpContext.Session["messageList"] = new List<Message>();
            return View();

        }

        [HttpPost]
        public ActionResult CreateMessage(Message message)
        {
            if (HttpContext.Request.Cookies["id"] == null)
                HttpContext.Response.Cookies["id"].Value = Methods.NewId();

            Message tempMessage = new Message(
                HttpContext.Request.Cookies["id"].Value,
                message.MessageBody,
                Methods.UnixTimeStamp(HttpContext.Timestamp)
                );

            var messagesList = HttpContext.Session["messageList"] as List<Message>;

            Methods.ModifitedList(messagesList).Add(tempMessage);

            HttpContext.Session["messageList"] = messagesList;

            return RedirectToAction("Index");
        }


        public ActionResult ShowMessages(string sortOrder)
        {
            if (sortOrder == "TimeSort" || sortOrder == null)
                ViewBag.IdSortParam = "IDsort";
            else
                ViewBag.IdSortParam = "";

            ViewBag.TimeSortParam = sortOrder == "TimeSort" ? "" : "TimeSort";

            var listMessages = HttpContext.Session["messageList"] as List<Message>;

            switch (sortOrder)
            {

                case "IDsort":
                    listMessages.Sort();
                    break;
                case "TimeSort":
                    listMessages = Methods.SortDate(listMessages);
                    break;
                default:
                    break;
            }

            return View(listMessages);
        }
    }
}