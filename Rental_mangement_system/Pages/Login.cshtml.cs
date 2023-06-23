using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace Rental_mangement_system.Pages
{
    public class LoginModel : PageModel
    {
        public static bool loggedin;
        public static string current_user_id = "";
        public static string login_redirect = "/Properties_posting_pages/Add_house";
        public UserInfo userInfo = new UserInfo();
        public string error_message = "";
        public string success_message = "";
        public string email = "";
        public string password = "";

        public void OnGet()
        {
        }
        public void OnPost()
        {
            email= Request.Form["email"].ToString();
            password= Request.Form["password"].ToString();

            if (email.Length == 0 || password.Length == 0)
            {
                error_message = "All the fields are required";
                return;
            }
            else
            {

                try
                {
                    using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                    {
                        connection.Open();
                        string sql_query = "SELECT * FROM users WHERE email=@email ";

                        using (SqlCommand command = new SqlCommand(sql_query, connection))
                        {
                            command.Parameters.AddWithValue("@email", email);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    userInfo.user_id = reader.GetInt32(0).ToString();
                                    userInfo.name = reader.GetString(1);
                                    userInfo.email = reader.GetString(2);
                                    userInfo.password = reader.GetString(3);
                                    userInfo.phone = reader.GetString(4);

                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    error_message = e.Message + "---> database_connection for user";
                    return;
                }
            }
            if(email == userInfo.email && password == userInfo.password)
            {
                loggedin = true;
                current_user_id = userInfo.user_id;

                Console.WriteLine(loggedin);
                Response.Redirect(login_redirect);

            }

            userInfo.user_id = "";
            userInfo.email = "";
            userInfo.password = "";
            userInfo.name = "";
            userInfo.phone = "";


        }
    }
}
