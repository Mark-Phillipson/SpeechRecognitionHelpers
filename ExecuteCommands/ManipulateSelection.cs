using System.Collections.Generic;

namespace ExecuteCommands
{
	public class ManipulateSelection
	{
		public string CreateSqlScriptForAllDatabases(string sql)
		{
			List<string> databases = new List<string> { "3EquityCourt", "39EssexDirectory", "3VB", "4StoneBuildings", "BrickCourtChambers", "CoramChambers", "FarringdonChambers", "HardwickeDirectories", "LawFirms", "SenateHouse" , "TwentyEssex" };
			var result = "";
			foreach (var database in databases)
			{
				result = $"{result}\r\nUSE [{database}]\r\n{sql}";
			}
			return result;
		}
	}
}
