using EastonPartners.Infrastructure.Persistence.DbContexts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EastonPartners.Web.Seeder
{
	public class PartnerToJSON
	{
		private readonly ApplicationDbContext _context;

		public PartnerToJSON(ApplicationDbContext context)
		{
			_context = context;
		}

		public void copyToJSON()
		{
			//Retrieve data from the database
			var data = _context.Partner.ToList();

			//Directory to save the file
			var filePath = ".\\Seeder\\partners.json";

			//Serialize data to JSON
			var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

			//Save JSON data to a file
			File.WriteAllText(filePath, jsonData);
		}
	}
}
