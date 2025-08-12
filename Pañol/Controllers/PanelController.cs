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

        public ActionResult VolverAlPanel()
        {
            var rol = Session["IdRol"] != null ? Convert.ToInt32(Session["IdRol"]) : 0;

            if (rol == 1) 
                return RedirectToAction("PanelAdmin");
            else if (rol > 1) 
                return RedirectToAction("PanelUser");
            else
                return RedirectToAction("Login", "Usuarios"); 
        }

    }
}