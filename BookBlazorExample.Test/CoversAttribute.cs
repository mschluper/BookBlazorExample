using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBlazorExample.Test
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class CoversAttribute : System.Attribute
    {
        /// <summary>
        /// Unique identifier of a requirement
        /// </summary>
        public string RequirementId { get; private set; }

        public CoversAttribute(string requirementId)
        {
            this.RequirementId = requirementId;
        }
    }
}
