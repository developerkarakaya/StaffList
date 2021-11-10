using StaffList.BussinessLayer.ModelClasses;
using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffList.BussinessLayer.BussinessHelper
{
    public static class UtilManager
    {
       public static string managerName, managerSurname;

        #region PERSONEL İŞLEMLERİ

       /// <summary>
       /// TÜM PERSONEL LİSTESİNİ VERİTABANINDAN ÇEKER
       /// </summary>
       /// <returns></returns>
       public static List<StaffViewModel> GetAllStaffList()
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   List<StaffViewModel> staffList = new List<StaffViewModel>();
                   var result = db.Staff.OrderByDescending(ss => ss.Id).ToList();
                   foreach (var item in result)
                   {
                       int managerId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == item.Id).ManagerId);
                       int depId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == item.Id).DepartmentId);

                       if (managerId != 0)
                       {
                           managerName = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Name;
                           managerSurname = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Surname;
                       }
                       else
                       {
                           managerName = "";
                           managerSurname = "";
                       }


                       staffList.Add(new StaffViewModel
                       {
                           DateOfBirth = item.DateOfBirth.ToString(),
                           DepartmentId = item.DepartmentId,
                           ManagerId = item.ManagerId,
                           Name = item.Name,
                           Phone = item.Phone,
                           Surname = item.Surname,
                           Id = item.Id,
                           DepartmanName = db.Department.FirstOrDefault(ss => ss.Id == depId).Department1,
                           ManagerName = managerName + " " + managerSurname
                       });
                   }
                   return staffList;
               }

           }
           catch (Exception e)
           {

               throw;
           }
       }
       /// <summary>
       /// VERİTABANINDAN PERSONEL SİLMEK İÇİN GEREKLİ METHOD.
       /// </summary>
       /// <param name="Id"></param>
       public static void StaffDelete(int Id)
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   Staff deletedStaff = db.Staff.FirstOrDefault(ss => ss.Id == Id);
                   db.Staff.Remove(deletedStaff);
                   db.SaveChanges();
               }
           }
           catch (Exception e)
           {

               throw;
           }


       }

       /// <summary>
       /// PERSONEL DETAYLARINI ALAN METHOD.
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
       public static StaffViewModel getStaffDetail(int Id)
       {
           string managerName, managerSurname;
           StaffViewModel staffModel = new StaffViewModel();
           try
           {
               using (var db = new StaffListEntities())
               {
                   int managerId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == Id).ManagerId);
                   int depId = Convert.ToInt32(db.Staff.FirstOrDefault(ss => ss.Id == Id).DepartmentId);
                   if (managerId != 0)
                   {
                       managerName = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Name;
                       managerSurname = db.Staff.FirstOrDefault(ss => ss.Id == managerId).Surname;
                   }
                   else
                   {
                       managerName = "";
                       managerSurname = "";
                   }

                   Staff StaffDetail = db.Staff.FirstOrDefault(ss => ss.Id == Id);
                   staffModel.DateOfBirth = StaffDetail.DateOfBirth.Value.ToShortDateString();
                   staffModel.DepartmanName = db.Department.FirstOrDefault(ss => ss.Id == depId).Department1;
                   staffModel.ManagerName = managerName + " " + managerSurname;
                   staffModel.ManagerId = StaffDetail.ManagerId;
                   staffModel.Name = StaffDetail.Name;
                   staffModel.Surname = StaffDetail.Surname;
                   staffModel.Phone = StaffDetail.Phone;
                   staffModel.DepartmentId = StaffDetail.DepartmentId;
                   return staffModel;
               }

           }
           catch (Exception e)
           {
               return null;
               throw;
           }
       }
       /// <summary>
       /// İLGİLİ PERSONELİN FOTOĞRAFLARINI ÇEKEN METHOD.
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
       public static List<ImageTbl> getStaffAllImages(int Id)
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   List<ImageTbl> staffImageList = db.ImageTbl.Where(ss => ss.StaffId == Id).ToList();
                   return staffImageList;

               }
           }
           catch (Exception e)
           {

               throw e;
           }
       }


        #endregion

    }
}
