using EastonPartners.Domain.Entities;
using EastonPartners.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoverCore.Abstractions.Seeder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EastonPartners.Web.Seeder
{
    public class PartnerSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        public int Priority => 100;

        public PartnerSeeder (ApplicationDbContext context)
        {
            _context = context;
        }

        public string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(nameof(EastonPartners.Web)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public List<T> ReadAll<T>(string resource)
        {
            string data = ReadResource(resource);

            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        public async Task SeedAsync()
        {
            await Task.Delay(100);

            if (!_context.PartnerType.Any())
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PartnerType ON");

                var records = ReadAll<PartnerType>("partnertypes.json");

                _context.PartnerType.AddRange(records);
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PartnerType OFF");

                transaction.Commit();
            }
            
			if (!_context.Partner.Any())
			{
				await using var transaction = await _context.Database.BeginTransactionAsync();

				_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Partner ON");

				var records = ReadAll<Partner>("partners.json");

				_context.Partner.AddRange(records);
				_context.SaveChanges();

				_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Partner OFF");

				transaction.Commit();
			}
            
			Console.WriteLine("Seeding partners");
        }
    }
}
