using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTutorial
{
    static class Cache
    {
        //int -> id of the group and list of strings -> list of tokens (of devices in that group)
        public static Dictionary<int, List<string>> devices;

        public static void GetAllDevice()
        {
            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=FaciDB;Integrated Security=true");
            conn.Open();

            const string SQL_LOAD_ALL_DEVICES =
                @"SELECT ClientDeviceToken, [Group] FROM clientdevicetable";

            using (SqlCommand cmd = new SqlCommand(SQL_LOAD_ALL_DEVICES, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        devices = new Dictionary<int, List<string>>();
                        while (reader.Read())
                        {
                            string token = reader["ClientDeviceToken"].ToString();
                            int group = Int32.Parse(reader["Group"].ToString());
                            if (devices.ContainsKey(group))
                            {
                                List<string> listOfTokens;
                                devices.TryGetValue(group, out listOfTokens);
                                listOfTokens.Add(token);
                                devices[group] = listOfTokens;
                            }
                            else
                            {
                                List<string> listOfTokens = new List<string>();
                                listOfTokens.Add(token);
                                devices.Add(group, listOfTokens);
                            }
                        }
                    }
                }
            }
        }
    }
}
