using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyTv.Controllers
{
    public class TvController : Controller
    {
        //
        // GET: /Tv/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Media(string url)
        {
            ViewBag.Source = "http://vip.1717yun.com/xlyy.php?url=" + url;
            return View();
        }
    }
}
