using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController( UserManager<ApplicationUser> userManager, ApplicationDBContext context )
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDisplayNameById( string userid )
        {
            ApplicationUser _user = await _userManager.FindByIdAsync( userid );
            return Ok( _user );
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            IList<ApplicationUser> _list = await _userManager.Users.ToListAsync();
            IList<User> _userlist = new List<User>();
            foreach (ApplicationUser _user in _list)
            {
                _userlist.Add( await ConvetToUser( _user, string.Empty ) );
            }
            _userlist = _userlist.Where( u => u.Role != "admin" ).ToList();

            return Ok( _userlist );
        }

        [HttpGet]
        public async Task<IActionResult> GetUserListByRole( string role )
        {
            IList<ApplicationUser> _list = await _userManager.GetUsersInRoleAsync( role );
            IList<User> _userlist = new List<User>();
            foreach (ApplicationUser _user in _list)
            {
                _userlist.Add( await ConvetToUser( _user, role ) );
            }
            return Ok( _userlist );
        }

        private async Task<User> ConvetToUser( ApplicationUser applicationUser, string role )
        {
            User _user = new User();
            _user.Id = applicationUser.Id;
            _user.DisplayName = applicationUser.DisplayName;
            _user.Email = applicationUser.Email;
            if (string.IsNullOrEmpty( role ))
            {
                var _role = await _userManager.GetRolesAsync( applicationUser );
                _user.Role = _role[0];
            }
            else
            {
                _user.Role = role;
            }
            return _user;
        }
    }
}
