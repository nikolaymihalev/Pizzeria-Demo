﻿using BellaVitaPizzeria.Core.Contracts;
using BellaVitaPizzeria.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bella_Vita_Pizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly ICartService cartService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(
            IAdminService _adminService,
            RoleManager<IdentityRole> _roleManager,
            UserManager<IdentityUser> _userManager,
            ICartService _cartService)
        {
            adminService = _adminService;
            roleManager = _roleManager;
            userManager = _userManager;
            cartService = _cartService;
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var model = await adminService.GetAllUsersAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            var model = new RoleModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await roleManager.RoleExistsAsync(model.Name) == false)
            {
                var role = new IdentityRole()
                {
                    Name = model.Name,
                };

                await roleManager.CreateAsync(role);
            }

            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> AllRoles()
        {
            var model = await adminService.GetAllRolesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string username, string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                var user = await userManager.FindByNameAsync(username);

                if (user != null)
                {
                    if (await userManager.IsInRoleAsync(user, roleName) == false)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }

            return RedirectToAction(nameof(AllUsers));
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var model = await cartService.GetOrdersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            await cartService.CompleteOrderAsync(id);

            return RedirectToAction(nameof(Orders));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            await cartService.DeleteOrderAsync(id);

            return RedirectToAction(nameof(Orders));
        }

        [HttpGet]
        public async Task<IActionResult> FindPurchase(int purchaseId) 
        {
            var model = await cartService.GetPurchaseByIdAsync(purchaseId);

            if (model == null) 
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics() 
        {
            var model = await adminService.GetStatisticsAsync();

            return View(model);
        }
    }
}
