using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Policy = "AdminOnly")]
public class AdminDashboardModel : PageModel
{
  public void OnGet()
    {
    }
}
