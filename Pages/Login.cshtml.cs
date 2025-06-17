using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MejosDental.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string connectionString = "server=localhost;user=root;password=;database=pup;";
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string authQuery = "SELECT * FROM account WHERE P_Email = @Email AND PASSWORD = @Password";
            using var cmd = new MySqlCommand(authQuery, conn);
            cmd.Parameters.AddWithValue("@Email", Input.Email);
            cmd.Parameters.AddWithValue("@Password", Input.Password);

            using var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close(); // ✅ Close the reader before reusing the connection

                // Reopen the connection if it has closed (safety check)
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string getPIdQuery = "SELECT P_ID FROM patient WHERE P_Email = @Email";
                using var pidCmd = new MySqlCommand(getPIdQuery, conn);
                pidCmd.Parameters.AddWithValue("@Email", Input.Email);

                var result = pidCmd.ExecuteScalar();
                if (result != null)
                {
                    HttpContext.Session.SetString("P_ID", result.ToString()!);
                    return RedirectToPage("/History");
                }
                else
                {
                    ErrorMessage = "Patient record not found.";
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }

        public class LoginInputModel
        {
            [Required, EmailAddress]
            public string? Email { get; set; }

            [Required]
            public string? Password { get; set; }
        }
    }
}
