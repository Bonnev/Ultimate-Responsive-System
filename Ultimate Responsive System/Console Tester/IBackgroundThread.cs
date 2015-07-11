using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Tester
{
    interface IBackgroundThread
    {
        string Name { get; set; }

        string Description { get; set; }
    }
}
