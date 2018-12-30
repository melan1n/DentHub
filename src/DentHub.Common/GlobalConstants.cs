using System;
using System.Collections.Generic;

namespace DentHub.Common
{
	public static class GlobalConstants
	{
		public const string AdministratorRoleName = "Administrator";

		public const string DentistRoleName = "Dentist";

		public const string PatientRoleName = "Patient";

		public const string AdminUserName = "admin@denthub.com";

		public const string AdminPassword = "123456";

		public static string[][] Specialties =
		{
			new[] { "Dental public health", "The study of dental epidemiology and social health policies" }, 
			new[] { "Endodontics", "Root canal therapy and study of diseases of the dental pulp" },
			new[] { "Oral and maxillofacial pathology", "The study, diagnosis, and sometimes the treatment of oral and maxillofacial related diseases" },
			new[] { "Oral and maxillofacial radiology", "The study and radiologic interpretation of oral and maxillofacial diseases" },
			new[] { "Oral and maxillofacial surgery", "Extractions, implants, and facial surgery" },
			new[] { "Orthodontics and dentofacial orthopedics", "The straightening of teeth and modification of midface and mandibular growth" },
			new[] { "Periodontics", "Study and treatment of diseases of the periodontium (non-surgical and surgical) as well as placement and maintenance of dental implants" },
			new[] { "Pediatric dentistry", "Dentistry limited to child patients" },
			new[] { "Prosthodontics", "Dentures, bridges and the restoration of implants. Some prosthodontists further their training in \"oral and maxillofacial prosthodontics\", which is the discipline concerned with the replacement of missing facial structures, such as ears, eyes, noses, etc." },
		};

		public static string[][] Clinics =
		{
			new[] {"1", "Dental Health", "Evlogi Georgiev 47", "Sofia", "Bulgaria", "1021", "09:00 - 18:00"},
			new[] {"2", "Teeth Of Wonder", "Stara Planina 12", "Plovdiv", "Bulgaria", "5068", "09:00 - 19:00" },
			new[] {"3", "Medica", "Pirin 25", "Varna", "Bulgaria", "7000", "09:00 - 18:30" }
		};

		public static string[][] Dentists =
		{
			new[] { "Dentist","dentist1@test.com", "123456", "Viktor", "Dimitrov", "Dental Health", "Prosthodontics"},
			new[] { "Dentist","dentist2@test.com", "123456", "Ivo", "Draganov", "Dental Health", "Oral and maxillofacial surgery"},
			new[] { "Dentist","dentist3@test.com", "123456", "Cvetelina", "Yosifova", "Teeth Of Wonder", "Pediatric dentistry"},
			new[] { "Dentist","dentist4@test.com", "123456", "Dragana", "Nedeleva", "Medica", "Periodontics"},
		};

		public static string[][] Patients =
		{
			new[] { "Patient","patient1@test.com", "123456", "Zafir", "Yanin", "8512064593"},
			new[] { "Patient","patient2@test.com", "123456", "Jana", "Zagorkina", "6305205435"},
			new[] { "Patient","patient3@test.com", "123456", "Tanja", "Ivanova", "7509095133"},
		};


	}
}
