using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class EnumStringHandler
    {
        public static string FormatString(this ORMType ormType) => ormType == ORMType.EntityFrameWork ? "Entity Framework" : $"{ormType}";
    }

    public enum ORMType
    {
        Dapper,
        EntityFrameWork
    }

    public enum MethodType
    {
        CREATE,
        READ,
        UPDATE,
        DELETE
    }
}