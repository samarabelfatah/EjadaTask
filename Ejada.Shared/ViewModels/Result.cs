using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
