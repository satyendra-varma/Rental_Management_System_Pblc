using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Rental_mangement_system.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<PropertyInfo> completePropertiesList = new List<PropertyInfo>();
        public FilterInfo filterInfo = new FilterInfo();
        public bool filtersvisibility = false;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public string error_message = "";
        public string success_message = "";

        public void OnGet()
        {

            completePropertiesList.Clear();
            if (filterInfo.rentmin != "" && filterInfo.rentmax != "")
            {
                if (Int64.Parse(filterInfo.rentmin) > Int64.Parse(filterInfo.rentmax))
                {
                    filterInfo.rentmin = "0";
                    filterInfo.rentmax = "1000000";
                }
            }
            else
            {
                if (filterInfo.rentmin == "")
                {
                        
                    filterInfo.rentmin = "0";
                        
                }
                if (filterInfo.rentmax == "")
                {
                    filterInfo.rentmax = "1000000";
                }
            }
            string beds = "";
            string baths = "";
            beds = (filterInfo.beds != "") ? " AND beds = " + filterInfo.beds : "";
            baths = (filterInfo.baths != "") ? " AND baths = " + filterInfo.baths : "";
            /*               if (filterInfo.beds != "" && filterInfo.baths != "")
                            {
                                bedbath = " AND beds = " + filterInfo.beds + " AND baths =" + filterInfo.baths;
                            }
                            else if (filterInfo.beds == "" && filterInfo.baths != "")
                            {
                                bedbath = " AND baths =" + filterInfo.baths;
                            }
                            else if (filterInfo.beds != "" && filterInfo.baths == "")
                            {
                                bedbath = " AND beds = " + filterInfo.beds;
                            }
                            else if (filterInfo.beds == "" && filterInfo.baths == "")
                            {
                                bedbath = "";
                            }*/
            try
            {
                using (SqlConnection connection = new SqlConnection("Database Connection string here"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM properties WHERE (address LIKE @searchstring OR property_name LIKE @searchstring OR description LIKE @searchstring) AND (rent BETWEEN @min AND @max)" + beds + baths + ";", connection))
                    {
                        command.Parameters.AddWithValue("@searchstring", '%' + filterInfo.searchstring + '%');
                        command.Parameters.AddWithValue("@min", filterInfo.rentmin);
                        command.Parameters.AddWithValue("@max", filterInfo.rentmax);
                        /*if (filterInfo.beds != "" && filterInfo.baths != "")
                        {
                            //string bedbath = " AND beds = " + filterInfo.beds + " AND baths =" + filterInfo.baths;
                            command.Parameters.AddWithValue("@bedbath", bedbath);
                        }
                        else if (filterInfo.beds == "" && filterInfo.baths != "")
                        {
                            command.Parameters.AddWithValue("@bedbath", " AND baths =" + filterInfo.baths);
                        }
                        else if (filterInfo.beds != "" && filterInfo.baths == "")
                        {
                            command.Parameters.AddWithValue("@bedbath", " AND beds = " + filterInfo.beds);
                        }
                        else if (filterInfo.beds == "" && filterInfo.baths == "")
                        {
                            command.Parameters.AddWithValue("@bedbath", "");
                        }*/
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PropertyInfo propertyInfo = new PropertyInfo();
                                propertyInfo.property_id = reader.GetInt32(0).ToString();
                                propertyInfo.rent = reader.GetInt32(1).ToString();
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

                                completePropertiesList.Add(propertyInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message + "---> database_connection";
                return;
            }
        }
        public void OnPost()
        {
            completePropertiesList.Clear();
            filterInfo.searchstring = Request.Form["search"];
            filterInfo.rentmin = Request.Form["minrent"].ToString();
            filterInfo.rentmax = Request.Form["maxrent"].ToString();
            filterInfo.beds = Request.Form["beds"].ToString();
            filterInfo.baths = Request.Form["baths"].ToString();
            OnGet();
        }
        /*       public void showfilters()
               {
                   filters = true;
               }
               public void hidefilters()
               {
                   filters = false;
               }*/
    }
    public class FilterInfo
    {
        public string searchstring = "";
        public string rentmin = "";
        public string rentmax = "";
        public string beds = "";
        public string baths = "";

    }

}

