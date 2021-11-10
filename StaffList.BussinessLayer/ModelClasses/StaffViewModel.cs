using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffList.BussinessLayer.ModelClasses
{
    public partial class StaffViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DateOfBirth { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string DepartmanName { get; set;}
        public string ManagerName { get; set; }
    }
}
