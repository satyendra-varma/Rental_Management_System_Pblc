using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Rental_mangement_system.Pages
{
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
            LoginModel.loggedin = false;
        }
    }
}
