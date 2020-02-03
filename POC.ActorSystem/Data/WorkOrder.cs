using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.Data
{
    public class WorkOrder:IData
    {
        public WorkOrder(int workId)
        {
            this.WorkId = workId;
        }

        public int WorkId { get; }
    }
}
