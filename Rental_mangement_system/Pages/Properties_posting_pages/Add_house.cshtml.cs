using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Rental_mangement_system.Pages
{
    public class Add_houseModel : PageModel
    {
        public List<PropertyInfo> propertiesList = new List<PropertyInfo>();
        public void OnGet()
        {
            using (SqlConnection connection = new SqlConnection("Database Connection string here"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM properties WHERE usr_id=@user_id ", connection))
                {
                    command.Parameters.AddWithValue("@user_id", LoginModel.current_user_id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PropertyInfo propertyInfo = new PropertyInfo();
                            propertyInfo.property_id= reader.GetInt32(0).ToString();
                            propertyInfo.rent= reader.GetInt32(1).ToString();
                            propertyInfo.img_url = reader.GetString(2);
                            propertyInfo.property_name = reader.GetString(3);
                            propertyInfo.address = reader.GetString(4);
                            propertyInfo.description = reader.GetString(5);
                            propertyInfo.beds = reader.GetInt32(6).ToString();
                            propertyInfo.baths = reader.GetInt32(7).ToString();
                            propertyInfo.user_id = reader.GetInt32(8).ToString();
                            propertyInfo.phone = reader.GetString(9);
                            propertyInfo.start_date = reader.GetDateTime(10).ToString();
                            propertyInfo.time_created = reader.GetDateTime(11).ToString();

                            propertiesList.Add(propertyInfo);
                            
                        }
                    }
                }
            }
        }
    }
    public class PropertyInfo
    {
        public string property_id="";
        public string rent="";
        public string property_name = "";
        public string img_url = "";
        public string description = "";
        public string user_id = "";
        public string phone = "";
        public string address = "";
        public string start_date = "";
        public string beds = "";
        public string baths = "";
        public string time_created = "";

    }
}
