using System;
using System.Collections.Generic;
using System.Text;

namespace FluentMigrationApp
{
    internal class MigrationTypeStruct
    {
        public struct MigrationTypes
        {
            public const string Apply_Migration = "Apply_Migration";

            public const string Send_To_Development = "Send_To_Development";
            public const string Send_To_Test = "Send_To_Test";
            public const string Send_To_Production = "Send_To_Production";
        }
    }
}