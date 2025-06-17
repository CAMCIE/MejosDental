namespace MejosDental.Models
{
    public class Patient
    {
        public int P_ID { get; set; }
        public string P_name { get; set; } = string.Empty;
    }

    public class MedicalHistory
{
    public int M_ID { get; set; } 
    public int P_ID { get; set; }
    public DateTime M_Date { get; set; }
    public string? M_description { get; set; }
    public string? M_dentist_name { get; set; }
    public string? M_diagnosis { get; set; }
    public string? M_perform { get; set; }
    public string? M_result { get; set; }
    public string? M_medication { get; set; }
    public string? M_note { get; set; }
    public decimal M_amount_paid { get; set; }
}

}
