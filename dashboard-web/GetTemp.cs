using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace dashboard_web
{
    public class GetTemp
    {
        public GetTemp()
        {  
        }

        public Temperatur GetData(string connectionString)
        {
            string[] myVars = new string[3];
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var sql = "SELECT TOP (1) [dato],[tidspunkt],[grader] FROM Temperatur ORDER BY [dato] DESC";
            var command = new SqlCommand(sql, connection);
            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                myVars[0] = dataReader.GetValue(0).ToString();
                myVars[1] = dataReader.GetValue(1).ToString();
                myVars[2] = dataReader.GetValue(2).ToString();
            }
            connection.Close();

            Temperatur temperatur = new Temperatur(myVars[2], myVars[0], myVars[1]);

            return temperatur;
        }
    }

    public class Temperatur
    {
        public string Temp;
        public string Date;
        public string Time;

        public Temperatur(string temp, string date, string time)
        {
            Temp = temp;
            Date = date;
            Time = time;
        }
    }
}
