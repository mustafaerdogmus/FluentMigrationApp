using System;
using System.Collections.Generic;
using System.Text;

namespace FluentMigrationApp
{
    internal class MigrationTypeStruct
    {
        public struct MigrationTypes
        {
            public const string Send_To_Production = "Send_To_Production";
            public const string Not_Send_To_Production = "Not_Send_To_Production";
        }
    }
}