using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneServer
{
    public class Entity
    {
        public string name;
        public int id;
        public UInt16 zoneIndex;

        public bool HasInventorySpace { get { throw new NotImplementedException(); } }

        public bool CanAddItem(Item item)
        {
            throw new NotImplementedException();
        }

        public bool AddItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
