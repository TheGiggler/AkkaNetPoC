using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaNET.POC.Data
{
    public class Uninitialized : IData
    {
        public static Uninitialized Instance = new Uninitialized();

        private Uninitialized() { }
    }

}
