using StaffList.BussinessLayer.BussinessHelper;
using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using StaffList.WEBAPI.Models;

namespace StaffList.Areas.AdminPanel.Controllers
{
    public class DepartmentController : Controller
    {
     
        public DepartmentController()
        {
            string apiurl = ConfigurationSettings.AppSettings.Get("apiurl");
            Environment.SetEnvironmentVariable("DepartmanApiUrl", apiurl + "api/departman");
            Environment.SetEnvironmentVariable("DepartmanDeleteApiUrl", apiurl + "api/departman/Delete/");
        }
        //private readonly string DepartmanApiUrl { get { return apiurl + "api/departman/DepartmanAdd"; } }
        // GET: AdminPanel/Department
        public ActionResult Index()
        {
            var db = new StaffListEntities();
            if (Request.QueryString["Id"] != null)
            {
                int Id = Convert.ToInt32(Request.QueryString["Id"]);
                TempData["depName"] = db.Department.FirstOrDefault(ss => ss.Id == Id).Department1;
                TempData["depId"] = db.Department.FirstOrDefault(ss => ss.Id == Id).Id;
            }
            //var result = UtilPanelManager.getDepartment();

            List<Department> depList;
            HttpResponseMessage response = GlobalVeriables.WebApiClient.GetAsync("departman").Result;
            depList = response.Content.ReadAsAsync<List<Department>>().Result;
            
            return View(depList);
        }

        [HttpPost]
        public ActionResult DepartmentAdd(Department dep) {
          //  UtilPanelManager.DepartmentAdd(dep);
            // web api departman post işlemi 

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:29265/api/staff");
                var postTasks = client.PostAsJsonAsync<Department>("departman", dep);
                postTasks.Wait();
                var result = postTasks.Result;
            }
            // web api departman post işlemi 
            return RedirectToAction("Index");
        }


        public ActionResult DepartmentDelete(int Id)
        {
            try
            {

                using (var db = new StaffListEntities())
                {
                    List<Staff> depIn = db.Staff.Where(ss => ss.DepartmentId == Id).ToList();
                    if (depIn.Count != 0) {
                        TempData["Message"] = "Silmek istediğiniz departmanın altında tanımlı çalışan vardır. Silme işlemi gerçekleşemez.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // web api delete işlemi 

                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("DepartmanDeleteApiUrl"));
                            var responseTask = client.DeleteAsync(Id.ToString());
                            responseTask.Wait();
                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        
                        
                        // web api delete işlemi 
                       
                        return RedirectToAction("Index");
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