using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class Delegates
    {
        public delegate IDataService DataServiceResolver(ORMType ORM);
    }
}
