using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ejada.Entities.Models
{
    public class Department
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> ManagerId { get; set; }
        public List<Employee> Employee { get; set; }
        public bool IsActive { get; set; }
        
    }
}
