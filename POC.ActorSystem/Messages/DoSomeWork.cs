using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaNET.POC.Messages
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
