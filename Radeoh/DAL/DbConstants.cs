using System;
using System.IO;
using SQLite;

namespace Radeoh.DAL
{
    public static class DbConstants
    {
        public const string DatabaseFilename = "RadeohSQLite.db3";

        public const SQLiteOpenFlags Flags =
            // open in rw mode
            SQLiteOpenFlags.ReadWrite |
            // create if not exist
            SQLiteOpenFlags.Create |
            // enabled multithreaded cache
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}