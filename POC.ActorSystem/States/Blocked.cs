using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.States
{
    public class Blocked:IState
    {
        public static readonly Blocked Instance = new Blocked();
    }
}
