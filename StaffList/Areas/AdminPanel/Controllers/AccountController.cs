using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaffList.DataEntitiess;
using System.Web.Mvc;
using System.Security.Cryptography;
using StaffList.BussinessLayer.BussinessHelper;

namespace StaffList.Areas.AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        // GET: AdminPanel/Account

        public ActionResult Login()
        {

            return View();
        }

        public ActionResult Password()
        {
            var model = UtilPanelManager.getAccount();
            model.Password = UtilPanelManager.MD5Olustur(model.Password);
            return View(model);
        }

        [HttpPost]

        public ActionResult PasswordUpdate(Account account)
        {
            using (var db = new StaffListEntities())
            {
                var updateAccount = db.Account.FirstOrDefault();
                updateAccount.UserName = account.UserName;
                updateAccount.Password = UtilPanelManager.MD5Olustur(account.Password);
                db.SaveChanges();
                TempData["Message"] = "Şifre Güncelleme İşlemi Başarılı !";
                return Redirect("/AdminPanel/Account/Password");
            }

        }


        [HttpPost]
        public ActionResult LoginAdmin(Account account)
        {
            try
            {
                using (var db = new StaffListEntities())
                {

                    var dbAccount = db.Account.FirstOrDefault();
                    if (dbAccount.UserName == account.UserName && dbAccount.Password == UtilPanelManager.MD5Olustur(account.Password)) // ŞİFREYİ MDF'Lİ BİÇİME GETİRDİM.
                    {
                        Session["UserName"] = account.UserName;
                        return Redirect("/AdminPanel/Home/AdminUI");
                    }
                    else
                    {
                        TempData["Message"] = "Girdiğiniz bilgiler hatalıdır. Lütfen tekrar deneyiniz.";
                        return Redirect("/AdminPanel/Account/Login");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
  
}

