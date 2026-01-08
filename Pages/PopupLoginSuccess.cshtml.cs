using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PopupLoginSuccessModel : PageModel
{
    public IActionResult OnGet()
    {
        // če ni prijavljen, ga vrni na login
        if (User.Identity?.IsAuthenticated != true)
            return Redirect("/Identity/Account/Login");

        return Page();
    }
}
