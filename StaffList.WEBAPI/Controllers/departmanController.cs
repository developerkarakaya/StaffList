using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StaffList.WEBAPI.Controllers
{
    public class departmanController : ApiController
    {

        public List<Department> GetDepartmanAll()
        {
            using (var db = new StaffListEntities())
            {
                var depList = db.Department.OrderByDescending(ss => ss.Id).ToList();
                return depList;
            }
        }

        public List<Department> GetDepartmanAll(int id)
        {
            using (var db = new StaffListEntities())
            {
                var depList = db.Department.OrderByDescending(ss => ss.Id).ToList();
                return depList;
            }
        }
        [HttpPost]
        public void post(Department dep)
        {
            using (var db = new StaffListEntities())
            {
                if (dep != null)
                {
                    db.Department.Add(dep);
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        public void Delete(int? id)
        {
            using (var db = new StaffListEntities())
            {
                if (id != null)
                {
                    Department deletedDep = db.Department.FirstOrDefault(ss => ss.Id == Convert.ToInt32(id));
                    db.Department.Remove(deletedDep);
                    db.SaveChanges();
                }
            }
        }



    }
}
