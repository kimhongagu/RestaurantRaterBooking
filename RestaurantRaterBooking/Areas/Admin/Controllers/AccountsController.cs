using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using System;
using System.Text;
using X.PagedList;

namespace RestaurantRaterBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
	{
		private readonly Models.AppContext _context;
		private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public AccountsController(Models.AppContext context, 
			IWebHostEnvironment environment, 
			UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
		{
			_context = context;
			_environment = environment;
			_userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }

		[HttpGet]
		public async Task<IActionResult> Index(string searchText, int? page)
		{
			int pageSize = 10;
			int pageNumber = page ?? 1;

			IEnumerable<ApplicationUser> users = await _context.Users.ToListAsync();

			if (!string.IsNullOrEmpty(searchText))
			{
				users = users.Where(x => x.UserName.Contains(searchText));
			}

			ViewData["ListAccount"] = users;
			var pagedListRestaurants = await users.ToPagedListAsync(pageNumber, pageSize);
			return View(pagedListRestaurants);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(string email, string password)
		{
            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, password);
            var roleId = "7715b01e-3872-4559-b05b-185dc0625f41";
            var userId = await _userManager.GetUserIdAsync(user);
            var userRole = new IdentityUserRole<string> { UserId = userId, RoleId = roleId };
            _context.UserRoles.Add(userRole);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Accounts", new { area = "RestaurantUser" });
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        [HttpPost]
		public async Task<IActionResult> UpdateStatus(string userId, string status)
		{
			try
			{
				var user = await _context.Users.FindAsync(userId);

				if (user == null)
				{
					return NotFound();
				}

				if (status == "Active")
				{
					user.LockoutEnabled = true;
					user.LockoutEnd = null;
					
				}
				else if (status == "Blocked")
				{
					user.LockoutEnabled = false;
					user.LockoutEnd = DateTimeOffset.MaxValue;
				}

				_context.Update(user);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error updating status: {ex.Message}");
			}
		}
	}
}
