using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MejosDental.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MejosDental.Pages
{
    public class AddHistoryModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AddHistoryModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public MedicalHistory History { get; set; } = new MedicalHistory();

        [BindProperty(SupportsGet = true)]
        public int P_ID { get; set; }

        public IActionResult OnGet()
        {
            History.P_ID = P_ID;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Model state is invalid.";
                return Page();
            }

            try
            {
                string? connStr = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connStr))
                {
                    TempData["Error"] = "Connection string not found.";
                    return Page();
                }

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"INSERT INTO medical_history 
                (P_ID, M_Date, M_description, M_dentist_name, M_diagnosis, M_perform, M_result, M_medication, M_note, M_amount_paid) 
                VALUES 
                (@P_ID, @M_Date, @M_description, @M_dentist_name, @M_diagnosis, @M_perform, @M_result, @M_medication, @M_note, @M_amount_paid)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@P_ID", History.P_ID);
                        cmd.Parameters.AddWithValue("@M_Date", History.M_Date);
                        cmd.Parameters.AddWithValue("@M_description", History.M_description ?? "");
                        cmd.Parameters.AddWithValue("@M_dentist_name", History.M_dentist_name ?? "");
                        cmd.Parameters.AddWithValue("@M_diagnosis", History.M_diagnosis ?? "");
                        cmd.Parameters.AddWithValue("@M_perform", History.M_perform ?? "");
                        cmd.Parameters.AddWithValue("@M_result", History.M_result ?? "");
                        cmd.Parameters.AddWithValue("@M_medication", History.M_medication ?? "");
                        cmd.Parameters.AddWithValue("@M_note", History.M_note ?? "");
                        cmd.Parameters.AddWithValue("@M_amount_paid", History.M_amount_paid);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            TempData["Error"] = "No rows inserted.";
                            return Page();
                        }
                    }
                }

                TempData["Success"] = "Added successfully.";
                return RedirectToPage("Admin");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Exception: " + ex.Message;
                return Page();
            }
        }

    }
}
