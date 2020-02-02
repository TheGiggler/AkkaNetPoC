using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.Messages
{
    public class DoSomeWork:IMessage
    {
        public int WorkID { get; private set; }
        public DoSomeWork(int workId)
        {
            WorkID = workId;
        }
    }
}
