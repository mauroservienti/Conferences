using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB02SampleMessages.Events
{
    public interface IHaveDoneSomething
    {
        string JobDone { get; set; }
    }
}
