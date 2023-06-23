using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.IO;
using Rental_mangement_system.ImageUploadService;

namespace Rental_mangement_system.Pages
{
    public class Create_new_propertyModel : PageModel
    {
        private readonly ILogger<Create_new_propertyModel> _logger;
        private readonly ImageUploadService.ImageUploadService imageUploadService;

        public Create_new_propertyModel(ILogger<Create_new_propertyModel> logger, ImageUploadService.ImageUploadService imageUploadService)
        {
            _logger = logger;
            this.imageUploadService = imageUploadService;
        }

        public PropertyInfo propertyInfo = new PropertyInfo();
        public string usr_id = LoginModel.current_user_id;
        public string error_message = "";
        public string success_message = "";
        public static string ImgPath = "";


        public void OnGet()
        {
            if (LoginModel.loggedin == false)
            {
                LoginModel.login_redirect = "/Properties_posting_pages/Create_new_property";
                Response.Redirect("/Login");
                return;
            }
        }
        public async void OnPost(IFormFile image)
        {

            if (image != null)
            {
                ImgPath = await imageUploadService.UploadImageAsync(image);
                while (ImgPath == null)
                {
                    success_message = "waiting";
                    Thread.Sleep(100);
                }
            }

            propertyInfo.user_id = usr_id;
            propertyInfo.property_name = Request.Form["property-name"];
            propertyInfo.rent = Request.Form["rent"];
            propertyInfo.img_url = ImgPath;
            propertyInfo.description = Request.Form["description"];
            propertyInfo.address = Request.Form["address"];
            propertyInfo.start_date = Request.Form["start_date"];
            propertyInfo.phone = Request.Form["phone_no."];
            propertyInfo.beds = Request.Form["beds"];
            propertyInfo.baths = Request.Form["baths"];
            propertyInfo.time_created = DateTime.Now.ToString();


            if (propertyInfo.property_name.Length == 0 ||propertyInfo.rent.Length == 0 || propertyInfo.description.Length == 0|| propertyInfo.address.Length == 0 || propertyInfo.start_date.Length == 0|| propertyInfo.phone.Length == 0)
            {
                error_message = "All the fields are required";
                return;
            }

            try
            {

                using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                {
                    connection.Open();
                    string sql_query = "INSERT INTO properties ( rent, img, property_name, address, description, beds, baths, usr_id, phone_no, start_date, time_created) VALUES (@rent, @img, @property_name, @address, @description, @beds, @baths, @usr_id, @phone_no, @start_date, @time_created)";
                    
                    using(SqlCommand command = new SqlCommand(sql_query, connection))
                    {
                        command.Parameters.AddWithValue("@rent", propertyInfo.rent);
                        command.Parameters.AddWithValue("@img", propertyInfo.img_url);
                        command.Parameters.AddWithValue("@property_name", propertyInfo.property_name);
                        command.Parameters.AddWithValue("@address", propertyInfo.address);
                        command.Parameters.AddWithValue("@description", propertyInfo.description);
                        command.Parameters.AddWithValue("@beds", propertyInfo.beds);
                        command.Parameters.AddWithValue("@baths", propertyInfo.baths);
                        command.Parameters.AddWithValue("@usr_id", propertyInfo.user_id);
                        command.Parameters.AddWithValue("@phone_no", propertyInfo.phone);
                        command.Parameters.AddWithValue("@start_date", propertyInfo.start_date);
                        command.Parameters.AddWithValue("@time_created", propertyInfo.time_created);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                error_message=e.Message +"---> database_connection";
                return;
            }

            propertyInfo.property_id = "";
            propertyInfo.property_name = "";
            propertyInfo.rent = "";
            propertyInfo.img_url = "";
            propertyInfo.description = "";
            propertyInfo.address = "";
            propertyInfo.start_date = "" ;
            propertyInfo.phone = "";
            propertyInfo.beds = "";
            propertyInfo.baths = "";
            propertyInfo.time_created = "";
            propertyInfo.user_id = "";
            ImgPath = "";

            //success_message = "New property created successfully";
            Response.Redirect("/Properties_posting_pages/Add_house");
        }
    }
}
