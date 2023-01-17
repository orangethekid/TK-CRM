using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Helpers;
using TakraonlineCRM.Server.Models;
using TakraonlineCRM.Shared.Models;
using TakraonlineCRM.Shared.User;

namespace TakraonlineCRM.Server.Controllers
{
    [Route( "api/[controller]/[action]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController( UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDBContext context )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login( LoginRequest request )
        {
            ApplicationUser appUser = await _userManager.FindByNameAsync( request.UserName );
            if (appUser == null)
                return BadRequest( "User does not exist" );

            var singInResult = await _signInManager.CheckPasswordSignInAsync( appUser, request.Password, false );
            if (!singInResult.Succeeded)
                return BadRequest( "Invalid password" );

            await _signInManager.SignInAsync( appUser, request.RememberMe );
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Register( UserRequest parameters )
        {
            ApplicationUser appUser = new ApplicationUser
            {
                DisplayName = parameters.DisplayName,
                UserName = parameters.UserName,
                Email = parameters.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync( appUser, parameters.Password );

            if (!result.Succeeded)
            {
                return BadRequest( result.Errors.FirstOrDefault()?.Description );
            }
            else
            {
                await _userManager.AddToRoleAsync( appUser, parameters.Role );
            }

            //return await Login( new LoginRequest
            //{
            //    UserName = parameters.UserName,
            //    Password = parameters.Password
            //} );
            return Ok();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<CurrentUser> CurrentUserInfo()
        {
            ApplicationUser appUser = await _userManager.FindByNameAsync( User.Identity.Name );
            var role = await _userManager.GetRolesAsync( appUser );

            return new CurrentUser
            {
                Id = appUser.Id,
                DisplayName = appUser.DisplayName,
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Role = role[0],
                Claims = User.Claims
                .ToDictionary( c => c.Type, c => c.Value )
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userlist = await (from appUser in _context.Users
                                  join userRoles in _context.UserRoles on appUser.Id equals userRoles.UserId
                                  join role in _context.Roles on userRoles.RoleId equals role.Id
                                  //select new { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.Name })
                                  select new User
                                  {
                                      Id = appUser.Id,
                                      DisplayName = appUser.DisplayName,
                                      Role = role.Name,
                                      Email = appUser.Email,
                                      UserName = appUser.UserName
                                  })
                        .ToListAsync();
            return Ok( userlist );
        }

        [HttpGet]
        public async Task<IActionResult> Get( [FromQuery] PaginationDTO pagination )
        {
            var queryable = _context.Users.AsQueryable();
            await HttpContext.InsertPaginationParameterInResponse( queryable, pagination.QuantityPerPage );
            var userList = await (from appUser in queryable.Paginate( pagination )
                                  join userRoles in _context.UserRoles on appUser.Id equals userRoles.UserId
                                  join role in _context.Roles on userRoles.RoleId equals role.Id
                                  //select new { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.Name })
                                  select new User
                                  {
                                      Id = appUser.Id,
                                      DisplayName = appUser.DisplayName,
                                      Role = role.Name,
                                      Email = appUser.Email,
                                      UserName = appUser.UserName
                                  })
                        .ToListAsync();
            return Ok( userList );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByRole( string roleName )
        {
            var userlist = await (from appUser in _context.Users
                                  join userRoles in _context.UserRoles on appUser.Id equals userRoles.UserId
                                  join role in _context.Roles on userRoles.RoleId equals role.Id
                                  where role.Name == roleName
                                  select new User
                                  {
                                      Id = appUser.Id,
                                      DisplayName = appUser.DisplayName,
                                      Role = role.Name,
                                      Email = appUser.Email,
                                      UserName = appUser.UserName
                                  })
                                    .ToListAsync();
            return Ok( userlist );
        }

        [HttpGet]
        public async Task<IActionResult> GetOneByUserId( Guid userId )
        {
            string id = Convert.ToString( userId );
            if (string.IsNullOrEmpty( id ))
            {
                return NoContent();
            }

            ApplicationUser appUser = await _userManager.FindByIdAsync( id );
            IList<string> role = await _userManager.GetRolesAsync( appUser );
            User user = new User
            {
                Id = appUser.Id,
                DisplayName = appUser.DisplayName,
                Role = role[0],
                Email = appUser.Email,
                UserName = appUser.UserName
            };

            return Ok( user );
        }

        [HttpGet]
        public async Task<IActionResult> GetOneByEmail( string email )
        {
            if (email == null)
            {
                return NoContent();
            }

            ApplicationUser appUser = await _userManager.FindByEmailAsync( email );
            if (appUser != null)
            {
                IList<string> role = await _userManager.GetRolesAsync( appUser );
                User user = new User
                {
                    Id = appUser.Id,
                    DisplayName = appUser.DisplayName,
                    Role = role[0],
                    Email = appUser.Email,
                    UserName = appUser.UserName
                };

                return Ok( user );
            }
            else
            {
                return Ok( new User() );
            }
        }

        public async Task<IActionResult> GetOneByUserName( string userName )
        {
            if (userName == null)
            {
                return NoContent();
            }

            ApplicationUser appUser = await _userManager.FindByNameAsync( userName );
            if (appUser != null)
            {
                IList<string> role = await _userManager.GetRolesAsync( appUser );
                User user = new User
                {
                    Id = appUser.Id,
                    DisplayName = appUser.DisplayName,
                    Role = role[0],
                    Email = appUser.Email,
                    UserName = appUser.UserName
                };

                return Ok( user );
            }
            else
            {
                return Ok( new User() );
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditUser( UserEditRequest parameters )
        {
            if (parameters.Id == null)
            {
                return NoContent();
            }

            ApplicationUser appUser = await _userManager.FindByIdAsync( parameters.Id );
            if (appUser == null)
            {
                return NotFound();
            }
            var rolesAppUser = await _userManager.GetRolesAsync( appUser );

            appUser.DisplayName = parameters.DisplayName;
            appUser.UserName = parameters.UserName;
            appUser.Email = parameters.Email;
            await _userManager.UpdateAsync( appUser );

            if (rolesAppUser[0] != parameters.Role)
            {
                await _userManager.RemoveFromRoleAsync( appUser, rolesAppUser[0] );
                await _userManager.AddToRoleAsync( appUser, parameters.Role );
            }

            if (!string.IsNullOrWhiteSpace( parameters.NewPassword ))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync( appUser );
                await _userManager.ResetPasswordAsync( appUser, token, parameters.NewPassword );
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser( string userId )
        {
            if (userId == null)
            {
                return NoContent();
            }

            ApplicationUser appUser = await _userManager.FindByIdAsync( userId );
            var rolesForUser = await _userManager.GetRolesAsync( appUser );

            using (var transaction = _context.Database.BeginTransaction())
            {
                if (rolesForUser.Count > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        await _userManager.RemoveFromRoleAsync( appUser, item );
                    }
                }

                await _userManager.DeleteAsync( appUser );
                transaction.Commit();
            }
            return NoContent();
        }
    }
}