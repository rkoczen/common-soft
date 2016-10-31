using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;


namespace IphoneSMSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderWithBackup = "e11a838b48784d7e6856f02e5de4c0253eea14c9";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Apple Computer\MobileSync\Backup\"+folderWithBackup;

            SQLiteConnection conn = new SQLiteConnection("Data Source=" + path + @"\3d0d7e5fb2ce288813306e4d4636395e047a3d28");
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM message LEFT JOIN handle ON handle.ROWID=message.handle_id", conn);
            conn.Open();
            SQLiteDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
            DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, 0);            
            // The timestamp is based on UNIX Epoch, so this will get you the current date and time
            DateTime timeStamp = UnixBase.AddSeconds(Int32.Parse(reader["date"].ToString()) + 978307200);
            int isfromme = int.Parse(reader["is_from_me"].ToString());
            string from = string.Empty;
                if (isfromme != 1)
                {
                    // The address of the sender
                    from = reader["id"].ToString();
                }
                else
                {
                    from = "Me";
                }

                // The actual "text" of the message
            string text = reader["text"].ToString();
            // The ID of the message
            string id = reader["ROWID"].ToString();

                Console.WriteLine(timeStamp.Date.ToString()+" "+timeStamp.TimeOfDay.ToString()+" "+from+" "+text);
                Console.ReadKey();
            }

        }
    }
}
