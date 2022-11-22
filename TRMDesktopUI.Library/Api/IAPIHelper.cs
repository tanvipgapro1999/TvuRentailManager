﻿using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        void LogOffUser();
        Task GetLoggedInUserInfo(string token);
    }
}