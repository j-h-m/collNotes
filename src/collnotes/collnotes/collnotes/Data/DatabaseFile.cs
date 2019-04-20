using System;
using System.IO;
using SQLite;

namespace collnotes.Data
{
    /// <summary>
    /// Database file.
    /// </summary>
    public static class DatabaseFile
    {
        private static SQLiteConnection sqliteConnection = null;

        /// <summary>
        /// Sets the connection to the SQLite DB file.
        /// </summary>
        private static void SetConnection()
        {
            string filePath = CreateDBFilePath();
            sqliteConnection = new SQLiteConnection(filePath);
        }

        /// <summary>
        /// Gets the connection to the SQLite DB file.
        /// </summary>
        /// <returns>The connection.</returns>
        public static SQLiteConnection GetConnection()
        {
            if (sqliteConnection != null)
            {
                return sqliteConnection;
            }
            else
            {
                SetConnection();
                return sqliteConnection;
            }
        }

        // no args method
        // returns string with file path for the sqlite database
        // will be different on iOS vs Android
        private static string CreateDBFilePath()
        {
            var sqliteFilename = "collNotes_database.db3";
#if __IOS__
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string folderPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
#else
            // Just use whatever directory SpecialFolder.Personal returns
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
            var path = Path.Combine(folderPath, sqliteFilename);
            return path;
        }
    }
}
