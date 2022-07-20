using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Helpers;
using Zaher.Ui.Models.ViewModels.ApplicationUsersViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Controllers
{
    [Authorize(Roles = Roles.SuperAdmin)]
    public class ApplicationUsersController : Controller
    {
        private readonly IUoW uoW;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUsersController(IUoW uoW,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.uoW = uoW;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }






        public async Task<IActionResult> Index(int page)
        {
            var users = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u=> u.Role.ToLower() != "superadmin");
            var model = (mapper.Map<IEnumerable<ListApplicationUsersViewModel>>(users)
                .AsQueryable().GetPagination(page, 5, "qas"));

            return View(model);
        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await uoW.UsersRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = mapper.Map<DetailsApplicationUserViewModel>(user);

            return View(model);
        }





        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var user = await uoW.UsersRepository.GetUserByIdAsync(id);
            if (user == null) { return NotFound(); }


            var model = mapper.Map<EditApplicationUserViewModel>(user); 
            if (user.Role.ToLower() == "superadmin" || user.NormalizedEmail == "ZAHER.SH1392@GMAIL.COM")
            {
                return PartialView("_CanNotEditSuperAdmin", model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditApplicationUserViewModel viewModel)
        {
            var user = await uoW.UsersRepository.GetUserByIdAsync(id);
            if (await TryUpdateModelAsync(viewModel, "", u => u.FirstName,
                                               u => u.LastName,
                                               u => u.Email,
                                               u => u.Role))
                mapper.Map(viewModel, user);
            if (!String.IsNullOrWhiteSpace(user.Role))
            {

                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await userManager.AddToRoleAsync(user, user.Role);

            }

            if (ModelState.IsValid)
            {

                try
                {
                    await uoW.CompleteAsync();
                    return RedirectToAction(nameof(Index), new { @notify = "User-Edited" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
            }
            return View(viewModel);

        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await uoW.UsersRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            var model = mapper.Map<DeleteApplicationUserViewModel>(user);
            if (user.Role.ToLower() == "superadmin" || user.NormalizedEmail == "ZAHER.SH1392@GMAIL.COM")
            {
                return PartialView("_CanNotDeleteSuperAdmin", model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, DeleteApplicationUserViewModel viewModel)
        {
            var superAdminId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Role.ToLower() == "superadmin").FirstOrDefault().Id;
            var user = await uoW.UsersRepository.GetUserByIdAsync(id, true);

            uoW.UsersRepository.Remove(user);

            if (await uoW.SaveAsync())
            {
                return RedirectToAction(nameof(Index), new { @notify = "User-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }
        private bool UserExists(string id)
        {
            return uoW.UsersRepository.Any(id);
        }

       
    }
}
