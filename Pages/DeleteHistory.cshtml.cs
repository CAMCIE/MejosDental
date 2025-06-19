using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace MejosDental.Pages
{
    public class DeleteHistoryModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public int P_ID { get; set; }
        [BindProperty(SupportsGet = true)] public int M_ID { get; set; }

        public IActionResult OnGet()
        {
            if (P_ID <= 0 || M_ID <= 0)
            {
                TempData["Message"] = "Please select a valid appointment.";
                return RedirectToPage("Admin");
            }

            using var conn = new MySqlConnection("server=localhost;user=root;password=;database=pup;");
            conn.Open();

            string sql = "DELETE FROM medical_history WHERE P_ID=@pid AND M_ID=@mid";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pid", P_ID);
            cmd.Parameters.AddWithValue("@mid", M_ID);

            int affected = cmd.ExecuteNonQuery();
            conn.Close();

            TempData["Message"] = affected > 0
                ? "Medical history deleted successfully."
                : "No matching record found to delete.";

            return RedirectToPage("Admin");
        }
    }
}
