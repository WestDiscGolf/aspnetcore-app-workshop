using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationSchemeProvider _authSchemeProvider;

        public LoginModel(IAuthenticationSchemeProvider authSchemeProvider)
        {
            _authSchemeProvider = authSchemeProvider;
        }

        public IEnumerable<AuthenticationScheme> AuthSchemes { get; set; }

        public async Task<IActionResult> OnGet()
        {
            AuthSchemes = await _authSchemeProvider.GetRequestHandlerSchemesAsync();

            return Page();
        }

        public IActionResult OnPost(string scheme)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = Url.Page("/Index") }, scheme);
        }
    }
}