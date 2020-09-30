using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ejada.Entities.Models
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public int Mobile { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }

        public long DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
