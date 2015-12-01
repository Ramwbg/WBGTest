using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOBConsole
{
   public class ActivityUpdateMessage
    {
        public string AppId { get; set; }

        public string ActivityActionId { get; set; }

        public string ActivityId { get; set; }

        public string SourceId { get; set; }

        public string Comment { get; set; }

        public string AppData { get; set; }

        public ActivityTaskMessage TaskItem { get; set; }
    }
}
