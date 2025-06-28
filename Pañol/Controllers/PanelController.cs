using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pañol.Controllers
{
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult PanelAdmin()
        {
            return View();
        }
        public ActionResult PanelUser()
        {
            return View();
        }

    }
}