using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Constant
{
    public static class Constants
    {
        // https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0

        public const string DatabaseFilename = "SinarSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public const string PrimaryColor = "#4e73df";
        public const string SuccessColor = "#1cc88a";
        public const string InfoColor = "#36b9cc";
        public const string WarningColor = "#f6c23e";
        public const string DangerColor = "#e74a3b";
    }
}
