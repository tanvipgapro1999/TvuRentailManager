﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        // For Capture Data
        public DateTime CreatedDate { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }

        public void ResetUserModel() 
        {
            Token = "";
            Id = "";
            LastName = "";
            FirstName = "";
            EmailAddress = "";
            CreatedDate = DateTime.MinValue;
        }
    }
}
