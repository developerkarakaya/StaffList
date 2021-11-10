using StaffList.BussinessLayer.BussinessHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult PublicUI()
        {
            var result = UtilManager.GetAllStaffList();
            return View(result);
        }

        [HttpPost]
        public ActionResult StaffDelete(int Id)
        {
            UtilManager.StaffDelete(Id);
            return RedirectToAction("PublicUI");
        }

        public ActionResult HtmlHelper()
        {
            return View();
        }

        public ActionResult StaffDetail(int Id)
        {
            var staffDetail = UtilManager.getStaffDetail(Id);
            ViewBag.ImageList = UtilManager.getStaffAllImages(Id);
            return View(staffDetail);
        }
    }
}