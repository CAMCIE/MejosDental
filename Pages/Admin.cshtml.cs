using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MejosDental.Models;

namespace MejosDental.Pages
{
    public class AdminModel : PageModel
    {
        public List<(Patient, List<MedicalHistory>)> PatientsWithHistory { get; set; } = new();

        public void OnGet()
        {
            var conn = new MySqlConnection("server=localhost;user=root;password=;database=pup;");
            conn.Open();

            var patients = new List<Patient>();
            using var cmd = new MySqlCommand("SELECT P_ID, P_name FROM patient", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                patients.Add(new Patient { P_ID = reader.GetInt32(0), P_name = reader.GetString(1) });
            reader.Close();

            foreach (var p in patients)
            {
                var list = new List<MedicalHistory>();
                using var cmd2 = new MySqlCommand(
                    "SELECT M_ID, M_Date FROM medical_history WHERE P_ID=@id ORDER BY M_Date DESC", conn);
                cmd2.Parameters.AddWithValue("@id", p.P_ID);
                using var r2 = cmd2.ExecuteReader();
                while (r2.Read())
                    list.Add(new MedicalHistory { M_ID = r2.GetInt32(0), M_Date = r2.GetDateTime(1) });
                r2.Close();
                PatientsWithHistory.Add((p, list));
            }

            conn.Close();
        }
    }
}
