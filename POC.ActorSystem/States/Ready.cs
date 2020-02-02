using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.States
{
    public class Ready:IState
    {
        public static readonly Ready Instance = new Ready();
    }
}
