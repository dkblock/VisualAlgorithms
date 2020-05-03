using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.AppMiddleware
{
    public class AccessManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> HasAdminAccess(string userId)
        {
            var accessedUsers = await _userManager.GetUsersInRoleAsync("admin");
            return accessedUsers.Select(u => u.Id).Contains(userId);
        }
    }
}
