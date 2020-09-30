﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleEnglishName { get; set; }
        public string RoleArabicName { get; set; }
        public bool IsActive { get; set; }
    }
}
