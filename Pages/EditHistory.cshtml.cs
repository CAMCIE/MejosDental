using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MejosDental.Models;

namespace MejosDental.Pages
{
    public class EditHistoryModel : PageModel
    {
        [BindProperty] public MedicalHistory History { get; set; } = new();
        public void OnGet(int M_ID)
        {
            var conn = new MySqlConnection("server=localhost;user=root;password=;database=pup;");
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM medical_history WHERE M_ID=@mid", conn);
            cmd.Parameters.AddWithValue("@mid", M_ID);
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                History.M_ID = r.GetInt32("M_ID");
                History.M_Date = r.GetDateTime("M_Date");
                History.M_description = r.GetString("M_description");
                History.M_dentist_name = r.GetString("M_dentist_name");
                History.M_diagnosis = r.GetString("M_diagnosis");
                History.M_perform = r.GetString("M_perform");
                History.M_result = r.GetString("M_result");
                History.M_medication = r.GetString("M_medication");
                History.M_note = r.GetString("M_note");
                History.M_amount_paid = r.GetDecimal("M_amount_paid");
            }
            conn.Close();
        }

        public IActionResult OnPost()
        {
            var conn = new MySqlConnection("server=localhost;user=root;password=;database=pup;");
            conn.Open();
            using var cmd = new MySqlCommand(@"
                UPDATE medical_history SET
                M_description=@desc, M_dentist_name=@dentist,
                M_diagnosis=@diag, M_perform=@perf, M_result=@res,
                M_medication=@med, M_note=@note, M_amount_paid=@amt
                WHERE M_ID=@mid", conn);
            cmd.Parameters.AddWithValue("@desc", History.M_description);
            cmd.Parameters.AddWithValue("@dentist", History.M_dentist_name);
            cmd.Parameters.AddWithValue("@diag", History.M_diagnosis);
            cmd.Parameters.AddWithValue("@perf", History.M_perform);
            cmd.Parameters.AddWithValue("@res", History.M_result);
            cmd.Parameters.AddWithValue("@med", History.M_medication);
            cmd.Parameters.AddWithValue("@note", History.M_note);
            cmd.Parameters.AddWithValue("@amt", History.M_amount_paid);
            cmd.Parameters.AddWithValue("@mid", History.M_ID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToPage("Admin");
        }
    }
}
