using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Ejada.Shared
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderingProperty
    {
        [Display(Name = "Ascending", ResourceType = typeof(Resources))]
        Ascending = 1,
        [Display(Name = "Descending", ResourceType = typeof(Resources))]
        Descending = 2
    }
}
