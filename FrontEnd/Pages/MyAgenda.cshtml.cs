using System.Collections.Generic;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrontEnd.Pages
{
    public class MyAgendaModel : IndexModel
    {
        public MyAgendaModel(IApiClient apiClient, 
            IAuthorizationService authorizationService) : base(apiClient, authorizationService)
        {
        }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        protected override Task<List<SessionResponse>> GetSessionsAsync()
        {
            return _apiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
        }
    }
}