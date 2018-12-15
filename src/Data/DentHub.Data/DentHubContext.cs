
using DentHub.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentHub.Web.Models
{
    public class DentHubContext : IdentityDbContext<DentHubUser>
    {
        public DentHubContext(DbContextOptions<DentHubContext> options)
            : base(options)
        {
        }

		public DbSet<DentHubUser> DentHubUsers;
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<Clinic> Clinics { get; set; }
		public DbSet<PatientFile> PatientFiles { get; set; }
		public DbSet<PatientRecord> PatientRecords { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Specialty> Specialties { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			builder.Entity<PatientRecord>()
				.HasKey(pr => new { pr.PatientId, pr.PatientFileId });

			builder.Entity<PatientRecord>()
				.HasOne(pr => pr.Patient)
				.WithMany(p => p.PatientRecords)
				.HasForeignKey(pr => pr.PatientId);

		}
	}
}
