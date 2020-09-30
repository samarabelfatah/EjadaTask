using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Shared.ViewModels
{
    public class SearchCriteriaViewModel
    {
        public long? PageNumber { get; set; }
        public long? PageSize { get; set; }
        public string SearchText { get; set; }

        public string FieldName { get; set; }
        public OrderingProperty OrderingProperty { get; set; }

    }
}
