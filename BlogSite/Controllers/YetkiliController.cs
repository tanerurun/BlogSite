using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSite.Controllers
{
    public class YetkiliController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {//yetkisiz eleman iceriye girmez
             if(Session["username"]==null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }
             
            base.OnActionExecuted(filterContext);
        }

        public ActionResult Hata(string yazilacak)
        {
            ViewBag.yaz = yazilacak;
            return View();
        }

    }
}