using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MejosDental.Pages
{
    public class HistoryModel : PageModel
    {
        public PatientProfile? Profile { get; set; }
        public List<MedicalRecord> MedicalHistory { get; set; } = new();

        public IActionResult OnGet()
        {
            var patientId = HttpContext.Session.GetString("P_ID");

            if (string.IsNullOrEmpty(patientId))
            {
                return RedirectToPage("/Login");
            }

            string connectionString = "server=localhost;user=root;password=;database=pup;";
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            // Fetch patient profile
            string profileQuery = "SELECT P_name, P_age, P_sex, P_number, P_address, P_Birth, P_weight, P_height FROM patient WHERE P_ID = @P_ID";
            using (var cmd = new MySqlCommand(profileQuery, conn))
            {
                cmd.Parameters.AddWithValue("@P_ID", patientId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Profile = new PatientProfile
                    {
                        P_name = reader["P_name"].ToString() ?? "",
                        P_age = Convert.ToInt32(reader["P_age"]),
                        P_sex = reader["P_sex"].ToString() ?? "",
                        P_number = reader["P_number"].ToString() ?? "",
                        P_address = reader["P_address"].ToString() ?? "",
                        P_Birth = Convert.ToDateTime(reader["P_Birth"]),
                        P_weight = Convert.ToDouble(reader["P_weight"]),
                        P_height = Convert.ToDouble(reader["P_height"])
                    };
                }
            }

            // Fetch medical history
            string historyQuery = @"SELECT M_Date, M_description, M_dentist_name, M_diagnosis, M_perform, M_result, M_medication, M_amount_paid 
                                    FROM medical_history 
                                    WHERE P_ID = @P_ID";
            using (var cmd = new MySqlCommand(historyQuery, conn))
            {
                cmd.Parameters.AddWithValue("@P_ID", patientId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MedicalHistory.Add(new MedicalRecord
                    {
                        M_Date = Convert.ToDateTime(reader["M_Date"]),
                        M_description = reader["M_description"].ToString() ?? "",
                        M_dentist_name = reader["M_dentist_name"].ToString() ?? "",
                        M_diagnosis = reader["M_diagnosis"].ToString() ?? "",
                        M_perform = reader["M_perform"].ToString() ?? "",
                        M_result = reader["M_result"].ToString() ?? "",
                        M_medication = reader["M_medication"].ToString() ?? "",
                        M_amount_paid = reader["M_amount_paid"].ToString() ?? ""
                    });
                }
            }

            return Page();
        }

        // Models
        public class PatientProfile
        {
            public string P_name { get; set; } = "";
            public int P_age { get; set; }
            public string P_sex { get; set; } = "";
            public string P_number { get; set; } = "";
            public string P_address { get; set; } = "";
            public DateTime P_Birth { get; set; }
            public double P_weight { get; set; }
            public double P_height { get; set; }
        }

        public class MedicalRecord
        {
            public DateTime M_Date { get; set; }
            public string M_description { get; set; } = "";
            public string M_dentist_name { get; set; } = "";
            public string M_diagnosis { get; set; } = "";
            public string M_perform { get; set; } = "";
            public string M_result { get; set; } = "";
            public string M_medication { get; set; } = "";
            public string M_amount_paid { get; set; } = "";
        }
    }
}
