using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pañol.Filtros
{
   
        public class Admin : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var rol = filterContext.HttpContext.Session["IdRol"];
                if (rol == null || rol.ToString() != "1")
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Usuarios", action = "Login" }
                        ));
                }
                base.OnActionExecuting(filterContext);
            }
        }
    
}