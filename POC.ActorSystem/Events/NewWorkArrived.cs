using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.Events
{
    public class NewWorkArrived : IEvent
    {
        public int WorkID {get;set;}
    }
}
