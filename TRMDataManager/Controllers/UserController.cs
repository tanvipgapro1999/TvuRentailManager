using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // Get call
        [HttpGet]
        public UserModel GetById()
        {
            // Read and get UserId
            string UserId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(UserId).First();
        }
        // Read Roles from website
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers() 
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };
                    foreach (var r in user.Roles)
                    {
                       u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).FirstOrDefault().Name);
                    }
                    output.Add(u);
                }
            }
            return output;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles() 
        {
            using (var context = new ApplicationDbContext()) 
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);

                return roles;
            }
        }
        [Authorize(Roles = "Admin")]
        // HttpPost is used to send data to a server to create/update a resource
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public void AddARoles(UserRolePairModel pairing)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(pairing.UserId, pairing.RoleName);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveARoles(UserRolePairModel pairing)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.RemoveFromRole(pairing.UserId, pairing.RoleName);
            }
        }
    }
}
