using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteCommands
{
    public class ManipulateSelection
    {
        public string CreateSqlScriptForAllDatabases(string sql)
        {
            List<string> databases = new List<string> { "3EquityCourt", "39EssexDirectory", "3VB", "4StoneBuildings", "BrickCourtChambers", "CoramChambers", "FarringdonChambers", "HardwickeDirectories", "LawFirms", "SenateHouse" };
            var result = "";
            foreach (var database in databases)
            {
                result = $"{result}\r\nUSE [{database}]\r\n{sql}";
            }
            return result;
        }
    }
}
