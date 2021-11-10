using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StaffList.WEBAPI.Controllers
{
    public class staffController : ApiController
    {

        public List<Staff> Get()
        {
            using (var database = new StaffListEntities())
            {
                var staffList = database.Staff.OrderByDescending(ss => ss.Id).ToList();
                return staffList;
            }
        }

        public Staff Get(int Id)
        {
            using (var database = new StaffListEntities())
            {
                Staff detail = database.Staff.Where(ss => ss.Id == Id).FirstOrDefault();
                return detail;
            }
        }

        public void Post(Staff s)
        {
            using (var db = new StaffListEntities())
            {
                db.Staff.Add(s);
                db.SaveChanges();
            }

        }

    }
}
