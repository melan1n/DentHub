using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DentHub.Data
{
	public class DentHubContextFactory : IDesignTimeDbContextFactory<DentHubContext>
	{
		public DentHubContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var builder = new DbContextOptionsBuilder<DentHubContext>();

			var connectionString = configuration.GetConnectionString("DefaultConnection");

			builder.UseSqlServer(connectionString);

			// Stop client query evaluation
			builder.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

			return new DentHubContext(builder.Options);
		}
	}
}
