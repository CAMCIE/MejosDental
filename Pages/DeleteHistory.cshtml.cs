using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace MejosDental.Pages
{
    public class DeleteHistoryModel : PageModel
    {
        [BindProperty] public int P_ID { get; set; }
        [BindProperty] public int M_ID { get; set; }

        public IActionResult OnPost()
        {
            if (M_ID <= 0) return RedirectToPage("Admin");

            var conn = new MySqlConnection("server=localhost;user=root;password=;database=pup;");
            conn.Open();
            using var cmd = new MySqlCommand(
                "DELETE FROM medical_history WHERE P_ID=@pid AND M_ID=@mid", conn);
            cmd.Parameters.AddWithValue("@pid", P_ID);
            cmd.Parameters.AddWithValue("@mid", M_ID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToPage("Admin");
        }
    }
}
