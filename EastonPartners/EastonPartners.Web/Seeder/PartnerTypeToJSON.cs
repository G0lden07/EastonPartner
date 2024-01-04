using EastonPartners.Infrastructure.Persistence.DbContexts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EastonPartners.Web.Seeder
{
    public class PartnerTypeToJSON
    {
        private readonly ApplicationDbContext _context;

        public PartnerTypeToJSON(ApplicationDbContext context)
        {
            _context = context;
        }

        public void copyToJSON()
        {
            //Retrieve data from the database
            var data = _context.PartnerType.ToList();

            //Directory to save the file
            var filePath = ".\\Seeder\\partnertypes.json";

            //Serialize data to JSON
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

            //Save JSON data to a file
            File.WriteAllText(filePath, jsonData);
        }
    }
}
