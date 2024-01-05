using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class BasePageObj
    {
        public string State { get; set; }
        public Action<string> ChengStateCallBack { get; set; }
        public Func<Alert, Task> ShowAlert { get; set; }
        public Func<bool, Task> LoadHandler { get; set; }
        public Func<Result, Task> ErrorRequest { get; set; }

    }
}
