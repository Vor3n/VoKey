using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetServer.Entities
{
    public class FindableObject:Entity
    {
        public string color { get; set; }
        public FindableObject(string p)
        {
            // TODO: Complete member initialization
            color = p;
        }

        public FindableObject()
        {

        }
    }
}
