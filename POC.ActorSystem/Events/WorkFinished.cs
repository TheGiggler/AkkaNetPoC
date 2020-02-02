using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.Events
{
    public class WorkFinished:IEvent
    {
        public int WorkID { get; set; }
    }
}
