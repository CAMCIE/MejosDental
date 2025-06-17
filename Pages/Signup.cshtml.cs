using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace MejosDental.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public SignUpInputModel Input { get; set; } = new SignUpInputModel();

        public class SignUpInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Range(0, 120)]
            public int Age { get; set; }

            [Required]
            public string Gender { get; set; } = string.Empty;

            [Required]
            public string Phone { get; set; } = string.Empty;

            [Required]
            public string Address { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }

            [Required]
            public decimal Weight { get; set; }

            [Required]
            public decimal Height { get; set; }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = "server=localhost;user id=root;password=;database=pup;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert into `account`
                    string insertAccount = "INSERT INTO account (P_Email, PASSWORD) VALUES (@Email, @Password)";
                    using (MySqlCommand cmd = new MySqlCommand(insertAccount, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Input.Email);
                        cmd.Parameters.AddWithValue("@Password", Input.Password); // Consider hashing
                        cmd.ExecuteNonQuery();
                    }

                    // Insert into `patient`
                    string insertPatient = @"INSERT INTO patient 
                        (P_Email, P_name, P_age, P_sex, P_number, P_address, P_Birth, P_weight, P_height, 
                        P_appointment, P_procedure, P_preferred_date, P_upload, M_ID) 
                        VALUES 
                        (@Email, @Name, @Age, @Gender, @Phone, @Address, @Birthday, @Weight, @Height, 
                        '0000-00-00 00:00:00', '', '0000-00-00', NULL, NULL)";

                    using (MySqlCommand cmd = new MySqlCommand(insertPatient, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Input.Email);
                        cmd.Parameters.AddWithValue("@Name", Input.Name);
                        cmd.Parameters.AddWithValue("@Age", Input.Age);
                        cmd.Parameters.AddWithValue("@Gender", Input.Gender);
                        cmd.Parameters.AddWithValue("@Phone", Input.Phone);
                        cmd.Parameters.AddWithValue("@Address", Input.Address);
                        cmd.Parameters.AddWithValue("@Birthday", Input.Birthday);
                        cmd.Parameters.AddWithValue("@Weight", Input.Weight);
                        cmd.Parameters.AddWithValue("@Height", Input.Height);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Database error: " + ex.Message);
                return Page();
            }
        }
    }
}
