using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System;

namespace MejosDental.Pages
{
    public class AppointmentModel : PageModel
    {
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Procedure { get; set; } = string.Empty;
        [BindProperty] public DateTime Date { get; set; } = DateTime.Today;

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid input.";
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=pup;";
            using MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                // Check if the email exists in the patient table
                string query = "SELECT P_ID FROM patient WHERE P_Email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Email);

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int p_id = Convert.ToInt32(result);

                    string insertQuery = @"
                        INSERT INTO medical_history (
                            M_Date, M_description, P_ID, 
                            M_dentist_name, M_diagnosis, 
                            M_perform, M_result, M_medication, 
                            M_note, M_amount_paid
                        ) 
                        VALUES (
                            @Date, @Description, @P_ID, 
                            'To Be Assigned', 'Pending', 
                            'Pending', 'Pending', 'None', 
                            'No Note Yet', 0.00)";

                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Date", Date);
                    insertCmd.Parameters.AddWithValue("@Description", Procedure);
                    insertCmd.Parameters.AddWithValue("@P_ID", p_id);

                    insertCmd.ExecuteNonQuery();

                    SuccessMessage = "Appointment request submitted successfully.";
                    Email = string.Empty;
                    Procedure = string.Empty;
                    Date = DateTime.Today;
                }
                else
                {
                    ErrorMessage = "No matching patient found. Please create an account.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
        }
    }
}
