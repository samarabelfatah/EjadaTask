using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Entities.Common
{
    public class ExceptionLog
    {
        /// <summary>
        /// Id
        /// </summary>

        public long Id { get; set; }

        /// <summary>
        /// The method which had exception
        /// </summary>

        public string Method { get; set; }

        /// <summary>
        /// Parameters used if any
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Exception Details
        /// </summary>
        public string Exception { get; set; }


        /// <summary>
        /// Logged User name - while exception happen - if any
        /// </summary>
 
        public string UserName { get; set; }


    }
}
