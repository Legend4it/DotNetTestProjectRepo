using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botek
{
    interface ITaskBase:IDisposable
    {
        int? GetTaskId();
    }
}
