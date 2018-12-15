namespace DentHub.Data.Models
{
	public class PatientRecord
	{
		public virtual DentHubUser Patient { get; set; }
		public string PatientId { get; set; }

		public virtual PatientFile PatientFile { get; set; }
		public int PatientFileId { get; set; }

	}
}