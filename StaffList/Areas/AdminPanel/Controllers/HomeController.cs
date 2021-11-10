using StaffList.BussinessLayer.BussinessHelper;
using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Web.Helpers;
using System.Net.Http;

namespace StaffList.Areas.AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        // GET: AdminPanel/Home
        public ActionResult AdminUI()
        {
            using (var db = new StaffListEntities())
            {
                TempData["staffcount"] = db.Staff.Count().ToString();
                TempData["depcount"] = db.Department.Count().ToString();
                TempData["managercount"] = db.Staff.Where(ss => ss.ManagerId != null).Count().ToString();
            }

            return View();
        }

        public ActionResult Staff(int? Id)
        {
           // ViewBag.DepartmanList = UtilPanelManager.getDepartment();
           // ViewBag.ManagerList = UtilPanelManager.getManagerList();
            IEnumerable<Staff> managerList;
            IEnumerable<Department> departmanList;
    
            // web api department list get işlemi 

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:29265/api/");
                var responseTask = client.GetAsync("departman");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Department>>();
                    readTask.Wait();
                    departmanList = readTask.Result;

                }
                else
                {
                    departmanList = Enumerable.Empty<Department>();
                }
                ViewBag.DepartmanList = departmanList;
            }


            // web api department list get işlemi 




            // web api manager list get işlemi 


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:29265/api/");
                var responseTask = client.GetAsync("manager");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Staff>>();
                    readTask.Wait();
                  managerList = readTask.Result;
 
                }
                else
                {
                    managerList = Enumerable.Empty<Staff>();
                }
                ViewBag.managerList = managerList;
            }

            // web api manager list get işlemi 



            if (Id == null)
            {
                return View(new StaffList.BussinessLayer.ModelClasses.StaffViewModel { });
            }
            else
            {
                using (var db = new StaffListEntities())
                {
                    ViewBag.ImageList = UtilManager.getStaffAllImages(Convert.ToInt32(Id));
                    StaffList.BussinessLayer.ModelClasses.StaffViewModel staffModel = new BussinessLayer.ModelClasses.StaffViewModel();
                    Staff StaffDetail = db.Staff.FirstOrDefault(ss => ss.Id == Id);
                    int managerId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == Id).ManagerId);
                    int depId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == Id).DepartmentId);
                    if (depId != null)
                    {
                        staffModel.DepartmanName = db.Department.FirstOrDefault(ss => ss.Id == depId).Department1;
                    }
                    if (managerId != 0)
                    {
                        string managerName = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Name;
                        string managerSurname = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Surname;
                        staffModel.ManagerName = managerName + " " + managerSurname;
                    }
                    else
                    {
                        string managerName = "";
                        string managerSurname = "";
                    }
                    staffModel.DateOfBirth = StaffDetail.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    staffModel.ManagerId = StaffDetail.ManagerId;
                    staffModel.Name = StaffDetail.Name;
                    staffModel.Id = StaffDetail.Id;
                    staffModel.Surname = StaffDetail.Surname;
                    staffModel.Phone = StaffDetail.Phone;
                    staffModel.DepartmentId = StaffDetail.DepartmentId;
                    return View(staffModel);
                }
            }

         
        }

        public ActionResult StaffList()
        {
            var result = UtilManager.GetAllStaffList();
            return View(result);
        }

        public ActionResult StaffDelete(int Id)
        {
           
            try
            {
                using (var db = new StaffListEntities())
                {

                    List<Staff> list = db.Staff.Where(ss => ss.ManagerId == Id).ToList();
                    if (list.Count != 0)
                    {
                        TempData["Message"]= "Silmek istediğiniz kişi Yönetici statüsünde olduğu için silme işlemi gerçekleşmedi.";
                        return RedirectToAction("StaffList");
                    }
                    else
                    {


                        Staff deletedStaff = db.Staff.FirstOrDefault(ss => ss.Id == Id);
                        db.Staff.Remove(deletedStaff);
                        foreach (var item in db.ImageTbl.Where(ss=>ss.StaffId==Id).ToList()) 
                        {
                            db.ImageTbl.Remove(item);
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                        return RedirectToAction("StaffList");
                    }

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult StaffAdd(Staff personel, IEnumerable<HttpPostedFileBase> ImageList)
        
        {
            var db = new StaffListEntities();

            if (personel.Id > 0)
            {
                Staff updateStaff = db.Staff.FirstOrDefault(ss => ss.Id == personel.Id);
                updateStaff.DateOfBirth = personel.DateOfBirth;
                updateStaff.DepartmentId = personel.DepartmentId;
                updateStaff.ManagerId = personel.ManagerId;
                updateStaff.Name = personel.Name;
                updateStaff.Phone = personel.Phone;
                updateStaff.Surname = personel.Surname;
                db.SaveChanges();
                if (ImageList.FirstOrDefault() != null)
                {

                    foreach (var item in db.ImageTbl.Where(ss=>ss.StaffId==personel.Id).ToList()) 
                    {
                        db.ImageTbl.Remove(item);
                        db.SaveChanges();
                    }

                    DataEntitiess.ImageTbl img = new DataEntitiess.ImageTbl();
                    int staffId = personel.Id;
                    foreach (var item in ImageList)
                    {
                        System.Drawing.Image photo = System.Drawing.Image.FromStream(item.InputStream);
                        string photoName = Path.GetFileName(item.FileName);
                        string yol = "/Content/StaffImages/" + Guid.NewGuid() + Path.GetExtension(item.FileName);
                        WebImage webImage = new WebImage(item.InputStream);
                        webImage.Resize(250, 250, false, false);    // IMAGE RESİZE İŞLEMİ BURADA YAPILIYOR. WİDTH 250PX HEİGHT 250PX
                        webImage.Save(Server.MapPath(yol));
                        img.ImageUrl = yol;
                        img.StaffId = staffId;
                        db.ImageTbl.Add(img);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("StaffList");
            }
            else
            {
                //UtilPanelManager.StaffAdd(personel);  // WEB API POST İŞLEMİ İÇİN GEREKLİ KODLAR
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:29265/api/staff");
                    var postTask = client.PostAsJsonAsync<Staff>("staff", personel);
                    postTask.Wait();
                    var result = postTask.Result;
                }

                DataEntitiess.ImageTbl img = new DataEntitiess.ImageTbl();
                int staffId = personel.Id;
                foreach (var item in ImageList)
                {
                    System.Drawing.Image photo = System.Drawing.Image.FromStream(item.InputStream);
                    string photoName = Path.GetFileName(item.FileName);
                    string yol = "/Content/StaffImages/" + Guid.NewGuid() + Path.GetExtension(item.FileName);
                    WebImage webImage = new WebImage(item.InputStream);
                    webImage.Resize(250, 250, false, false);    // IMAGE RESİZE İŞLEMİ BURADA YAPILIYOR. WİDTH 250PX HEİGHT 250PX
                    webImage.Save(Server.MapPath(yol));
                    img.ImageUrl = yol;
                    img.StaffId = staffId;
                    db.ImageTbl.Add(img);
                    db.SaveChanges();
                }
                return RedirectToAction("StaffList"); 
            }
            
            
            
        }

       


    }
}