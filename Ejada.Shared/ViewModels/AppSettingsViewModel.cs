using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class AppSettingsViewModel
    {
        public string Secret { get; set; }
        public string BaseUrl { get; set; }
        public int TokenExpirationMinites { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public string Environment { get; set; }

    }
}
