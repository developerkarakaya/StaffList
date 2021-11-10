using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaffList.DataEntitiess;
using System.Security.Cryptography;
namespace StaffList.BussinessLayer.BussinessHelper
{
   public static class UtilPanelManager
   {

       #region Departman işlemleri
       public static List<Department> getDepartment()
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   List<Department> DepartmentList = db.Department.OrderByDescending(ss => ss.Id).ToList();
                   return DepartmentList;
               }
           }
           catch (Exception e)
           {

               throw e;
           }
       }

       public static void DepartmentAdd(Department dep)
       {
           try
           {
               using (var db = new StaffListEntities())
               {

                   if (dep.Id > 0)
                   {
                       var updateDep = db.Department.FirstOrDefault(ss => ss.Id == dep.Id);
                       updateDep.Department1 = dep.Department1;
                       db.SaveChanges();
                   }
                   else
                   {
                       db.Department.Add(dep);
                       db.SaveChanges();
                   }


               }
           }
           catch (Exception)
           {

               throw;
           }
       }

       #endregion

       #region manager işlemleri
       public static List<Staff> getManagerList()
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   List<Staff> ManagerList = db.Staff.OrderByDescending(ss => ss.Id).ToList();
                   return ManagerList;
               }
           }
           catch (Exception e)
           {
               throw e;
           }
       }
       #endregion

       #region staff işlemleri
       public static void StaffAdd(Staff personel)
       {
           try
           {
               using (var db = new StaffListEntities())
               {
                   db.Staff.Add(personel);
                   db.SaveChanges();
               }
           }
           catch (Exception)
           {

               throw;
           }
       }
       #endregion
       #region Şifreleme işlemleri
       public static string MD5Olustur(string text)
       {
           MD5 md5 = new MD5CryptoServiceProvider();

           md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

           byte[] result = md5.Hash;

           StringBuilder strBuilder = new StringBuilder();
           for (int i = 0; i < result.Length; i++)
           {
               strBuilder.Append(result[i].ToString("x2"));
           }

           return strBuilder.ToString();
       }
       #endregion


       #region account işlemleri
       public static Account getAccount()
       {
           using (var db = new StaffListEntities())
           {
               var result = db.Account.FirstOrDefault();
               return result;
           }
       }
       #endregion

   }
}
