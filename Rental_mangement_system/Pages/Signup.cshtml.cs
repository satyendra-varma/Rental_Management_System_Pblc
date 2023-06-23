using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace Rental_mangement_system.Pages
{
    public class SignupModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string error_message = "";
        public string success_message = "";

        public void OnGet()
        {
            
        }
        public void OnPost()
        {
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.password = Request.Form["password"];
            userInfo.phone = Request.Form["phone"];


            if ( userInfo.name.Length == 0 ||   userInfo.email.Length == 0 || userInfo.password.Length == 0 || userInfo.phone.Length == 0)
            {
                error_message = "All the fields are required";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                {
                    connection.Open();
                    string sql_query = "INSERT INTO users ( user_name, email, passwd, phone) VALUES (@name, @email, @password, @phone)";

                    using (SqlCommand command = new SqlCommand(sql_query, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@phone", userInfo.phone);
                        command.Parameters.AddWithValue("@password", userInfo.password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message + "---> database_connection for user";
                return;
            }


            userInfo.user_id = "";
            userInfo.name = "";
            userInfo.email = "";
            userInfo.phone = "";
            userInfo.password = "";

            success_message = "Acount created successfully";
            Response.Redirect("/Login");

        }
    }
    public class UserInfo
    {
        public string user_id;
        public string name;
        public string email;
        public string password;
        public string phone;
    }
}
