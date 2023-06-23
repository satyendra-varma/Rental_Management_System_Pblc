using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Rental_mangement_system.Pages.Properties_posting_pages
{
    public class Edit_propertyModel : PageModel
    {
        public PropertyInfo propertyInfo = new PropertyInfo();
        public String error_message = "";
        public String success_message = "";
        public String id ="";
        public void OnGet()
        {
            id = "" + Request.Query["property_id"];
            try
            {

                using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM properties WHERE property_id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                propertyInfo.property_id = reader.GetInt32(0).ToString();
                                propertyInfo.rent = reader.GetInt32(1).ToString();
                                propertyInfo.property_name = reader.GetString(3);
                                propertyInfo.address = reader.GetString(4);
                                propertyInfo.description = reader.GetString(5);
                                propertyInfo.beds = reader.GetInt32(6).ToString();
                                propertyInfo.baths = reader.GetInt32(7).ToString();
                                propertyInfo.user_id = reader.GetInt32(8).ToString();
                                propertyInfo.phone = reader.GetString(9);
                                propertyInfo.start_date = reader.GetDateTime(10).ToString();
                                propertyInfo.time_created = reader.GetDateTime(11).ToString();

                            }
                        }
                    }
                }

            }
            catch(Exception e)
            {
                error_message = e.Message;
            }

        }
        public void OnPost()
        {
            propertyInfo.property_id = Request.Form["property_id"];
            propertyInfo.property_name = Request.Form["property-name"];
            propertyInfo.rent = Request.Form["rent"];
            propertyInfo.description = Request.Form["description"];
            propertyInfo.address = Request.Form["address"];
            propertyInfo.start_date = Request.Form["start_date"];
            propertyInfo.phone = Request.Form["phone_no."];
            propertyInfo.beds = Request.Form["beds"];
            propertyInfo.baths = Request.Form["baths"];

            if (propertyInfo.property_name.Length == 0 || propertyInfo.rent.Length == 0 || propertyInfo.description.Length == 0 || propertyInfo.address.Length == 0 || propertyInfo.start_date.Length == 0 || propertyInfo.phone.Length == 0|| propertyInfo.beds.Length == 0 || propertyInfo.baths.Length == 0)
            {
                error_message = "All the fields are required";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=209.23.9.19,1433;Initial Catalog=RMSDB;Persist Security Info=True;User ID=SA;Password=<Satya@4a7>"))
                {
                    connection.Open();
                    string sql_query = "UPDATE properties SET rent=@rent, property_name=@property_name, address=@address, description=@description, beds=@beds, baths=@baths, phone_no=@phone_no, start_date=@start_date WHERE property_id=@property_id";

                    using (SqlCommand command = new SqlCommand(sql_query, connection))
                    {
                        command.Parameters.AddWithValue("@property_id", propertyInfo.property_id);
                        command.Parameters.AddWithValue("@rent", propertyInfo.rent);
                        command.Parameters.AddWithValue("@property_name", propertyInfo.property_name);
                        command.Parameters.AddWithValue("@address", propertyInfo.address);
                        command.Parameters.AddWithValue("@description", propertyInfo.description);
                        command.Parameters.AddWithValue("@beds", propertyInfo.beds);
                        command.Parameters.AddWithValue("@baths", propertyInfo.baths);
                        command.Parameters.AddWithValue("@phone_no", propertyInfo.phone);
                        command.Parameters.AddWithValue("@start_date", propertyInfo.start_date);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message + "---> database_connection error";
                return;
            }

            Response.Redirect("/Properties_posting_pages/Add_house");
        }
    }
}
