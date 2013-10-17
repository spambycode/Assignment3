using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClassLibrary
{
    class BSTNode
    {
        public short LChildPtr { get; set; }
        public short RChildPtr { get; set; }
        public short DRP { get; set; }
        public string KeyValue { get; set; }

        public BSTNode(RawData RD)
        {
            KeyValue = RD.NAME;
            DRP = Convert.ToInt16(RD.ID);
            LChildPtr = -1;
            RChildPtr = -1;
        }

        public int CompareTo(string value)
        {
            return value.ToUpper().CompareTo(KeyValue.ToUpper());
        }
    }
}
