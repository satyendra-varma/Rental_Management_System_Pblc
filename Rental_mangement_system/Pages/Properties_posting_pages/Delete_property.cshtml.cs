using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rental_mangement_system.ImageDeleteService;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Rental_mangement_system.Pages.Properties_posting_pages
{
    public class Delete_propertyModel : PageModel
    {
        private readonly ImageDeleteService.ImageDeleteService imageDeleteService;

        public Delete_propertyModel(ImageDeleteService.ImageDeleteService imageDeleteService)
        {
            this.imageDeleteService = imageDeleteService;
        }


        public PropertyInfo propertyInfo = new PropertyInfo();
        public String error_message = "";
        public String success_message = "";
        public String id = "";
        
        public void OnGet()
        {
            string id = "" + Request.Query["property_id"];
            string imgUrl = "";
            try
            {
                using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                {
                    connection.Open();
                    /*
                    using (SqlCommand command = new SqlCommand("SELECT * FROM properties WHERE property_id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                imgUrl = reader.GetString(2);
                                Console.WriteLine(imgUrl);
                            }
                        }
                    }
                    string founder = imgUrl;
                    // Remove all characters after first 25 chars  
                    string rmvdURL = founder.Remove(0, 45);

                    /*int pos = imgUrl.IndexOf("https://rmsbucket.s3.us-west-2.amazonaws.com/");
                    var imgs3Path = imgUrl.Remove(pos);*/
                  //  var deletemsg = imageDeleteService.DeleteImageAsync(rmvdURL);
                    /*while (deletemsg == true)
                    {
                        success_message = "waiting to delete image file";
                        Thread.Sleep(100);
                    }*/
                    using (SqlCommand command = new SqlCommand("DELETE FROM properties WHERE property_id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message;
            }
            Response.Redirect("/Properties_posting_pages/Add_house");
        }

    }
}
