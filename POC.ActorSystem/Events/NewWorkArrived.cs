using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaNET.POC.Events
{
    public class NewWorkArrived
    {
        public NewWorkArrived(int workId)
        {
            this.WorkId = workId;
        }
        public int WorkId {get;}
    }
}
