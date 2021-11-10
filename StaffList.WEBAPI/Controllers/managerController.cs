using StaffList.DataEntitiess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StaffList.WEBAPI.Controllers
{
    public class managerController : ApiController
    {

        public List<Staff> Get()
        {
            using (var db = new StaffListEntities())
            {
                var managerList = db.Staff.OrderByDescending(ss => ss.Id).ToList();
                return managerList;
            }
        }


    }
}
