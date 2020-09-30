using Ejada.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class EmployeeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Mobile { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
