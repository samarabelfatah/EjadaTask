using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class DepartmentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> Manager { get; set; }
        public bool IsActive { get; set; }

    }
}
