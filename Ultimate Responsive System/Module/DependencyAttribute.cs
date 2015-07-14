using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateResponsiveSystem.Module
{
    public class DependencyAttribute : Attribute
    {
        public int Priority { get; set; }

        public DependencyAttribute()
        {
            
        }

        public DependencyAttribute(int priority)
        {
            Priority = priority;
        }
    }
}
