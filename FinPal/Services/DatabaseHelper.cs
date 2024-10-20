using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPal.Constant;

namespace FinPal.Services
{
    public static class DatabaseHelper
    {
        public static string GetDatabasePath(string dbName)
        {
            string path = FileSystem.AppDataDirectory;
            return Path.Combine(path, dbName);
        }
        public static void DeleteDatabase(string dbName)
        {
            string dbPath = GetDatabasePath(dbName);

            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }
    }
}
