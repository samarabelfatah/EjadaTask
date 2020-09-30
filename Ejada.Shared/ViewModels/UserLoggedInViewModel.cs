using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class UserLoggedInViewModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public string Token { get; set; }
        //public string RefreshToken { get; set; }

        public List<RoleViewModel> RoleViewModel { get; set; }
        public string RefreshToken { get; set; }
        // public DateTime TokenExpiredDate { get; set; }
    }
}
