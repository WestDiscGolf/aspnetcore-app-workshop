using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConferenceDTO;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        protected readonly IAuthorizationService _authorizationService;

        public IndexModel(IApiClient apiClient, IAuthorizationService authorizationService)
        {
            _apiClient = apiClient;
            _authorizationService = authorizationService;
        }

        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }

        public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; }
        
        public bool IsAdmin { get; set; }

        public int CurrentDayOffset { get; set; }

        protected virtual Task<List<SessionResponse>> GetSessionsAsync()
        {
            return _apiClient.GetSessionsAsync();
        }

        public async Task OnGet(int day = 0)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "Admin");
            IsAdmin = result.Succeeded;
            
            CurrentDayOffset = day;

            var sessions = await GetSessionsAsync();

            var startDate = sessions.Min(s => s.StartTime?.Date);
            var endDate = sessions.Max(s => s.EndTime?.Date);

            var numberOfDays = ((endDate - startDate)?.Days) + 1;

            DayOffsets = Enumerable.Range(0, numberOfDays ?? 0)
                .Select(offset => (offset, (startDate?.AddDays(offset))?.DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(g => g.Key);
        }
    }
}
