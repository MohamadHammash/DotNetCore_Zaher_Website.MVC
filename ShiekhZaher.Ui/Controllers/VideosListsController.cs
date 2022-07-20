using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Extensions;
using Zaher.Ui.Filters;
using Zaher.Ui.Helpers;
using Zaher.Ui.Models.ViewModels.VideosListsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Controllers
{
    public class VideosListsController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        private readonly IUoW uoW;

        public VideosListsController(IUoW uoW, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.uoW = uoW;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string search, int page)
        {
            var videosLists = await uoW.VideosListsRepository.GetAllVideosListsAsync();

            var model = mapper.Map<IEnumerable<ListVideosListsViewModel>>(videosLists);

            if (!String.IsNullOrWhiteSpace(search))
            {
                model = model.Where(m => m.ListName.RemoveDiacritics().ToLower().StartsWith(search.RemoveDiacritics().ToLower()));
            }

            var paginationResult = model.AsQueryable().GetPagination(page, 9, "videosLists");

            return View(paginationResult);
        }

        [ModelNotNull]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id, string search, int page)
        {
            if (id is null)
            {
                return NotFound();
            }

            var videosList = await uoW.VideosListsRepository.GetVideosListByIdAsync((Guid)id);
            if (videosList is null)
            {
                return NotFound();
            }
            var model = mapper.Map<DetailsVideosListViewModel>(videosList);

            if (!String.IsNullOrWhiteSpace(search))
            {
                model.VideosResult = model.Videos
                    .OrderByDescending(v => v.PublishingDate)
                    .Where(v => v.Title.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics())).AsQueryable().GetPagination(page, 9, "videos");
            }
            else
            {
                model.VideosResult = model.Videos
                    .OrderByDescending(v => v.PublishingDate)
                    .AsQueryable().GetPagination(page, 9, "videos");
            }

            return View(model);
        }
        [Authorize(Roles =Roles.SuperAdminAdminEditor)]
        public IActionResult Create()
        {
            var viewmodel = new CreateVideosListViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Create(CreateVideosListViewModel viewmodel)
        {
            var videosList = mapper.Map<VideosList>(viewmodel);
            string uniqueFileName = null;

            if (viewmodel.Photo is not null)
            {
                string[] allowedextension = { ".jpg", ".jpeg", ".png", ".jfif" };
                var extension = Path.GetExtension(viewmodel.Photo.FileName);
                if (!allowedextension.Contains(extension))
                {
                    ModelState.AddModelError("Photo", "الملف المحمل ليس صورة , الرجاء تحميل صورة");
                }

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + viewmodel.Photo.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Use CopyTo() method provided by IFormFile interface to
                // copy the file to wwwroot/images folder

                // Fortunately FileStream implements IDisposable, so it's easy to wrap all the code inside a using statement
                // in order to ensure that the file won't be left open

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewmodel.Photo.CopyToAsync(stream);
                }
                // Here (outside the using statement) stream is not accessible and it has been closed (also if
                // an exception is thrown and stack unrolled
                videosList.ImagePath = uniqueFileName;
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                videosList.ApplicationUserId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Email == userEmail).FirstOrDefault().Id;
            }
            if (ModelState.IsValid)
            {
                await uoW.VideosListsRepository.AddAsync(videosList);

                if (await uoW.SaveAsync())
                {
                    return RedirectToAction(nameof(Index), new { @notify = "VList-Created" });
                }
                return View(viewmodel);
            }
            return View(viewmodel);
        }

        [ModelNotNull]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var videoslist = await uoW.VideosListsRepository.GetVideosListByIdAsync((Guid)id);

            if (videoslist == null)
            {
                return NotFound();
            }
            var model = mapper.Map<EditVideosListViewModel>(videoslist);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (videoslist.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToEditVideosList", model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Edit(Guid id, EditVideosListViewModel viewmodel)
        {
            var videosList = await uoW.VideosListsRepository.GetVideosListByIdAsync(id);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (videosList.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToEditVideosList", mapper.Map<EditVideosListViewModel>(videosList));
                }
            }
            if (await TryUpdateModelAsync(viewmodel, "", vl => vl.ListName))
                mapper.Map(viewmodel, videosList);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideosListExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index), new { @notify = "VList-Edited" });
        }

        [ModelNotNull]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var videoslistToBeRemoved = await uoW.VideosListsRepository.GetVideosListByIdAsync((Guid)id);
            var model = mapper.Map<DeleteVideosListViewModel>(videoslistToBeRemoved);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (videoslistToBeRemoved.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToDeleteVideosList",model);
                }
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        [ModelNotNull]
        public async Task<IActionResult> DeleteConfirmed(Guid id, DeleteVideosListViewModel viewModel)
        {
            var videoslistToBeRemoved = await uoW.VideosListsRepository.GetVideosListByIdAsync(id);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (videoslistToBeRemoved.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToDeleteVideosList", await uoW.VideosListsRepository.GetVideosListByIdAsync(id));
                }
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", $"{videoslistToBeRemoved.ImagePath}");

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            uoW.VideosListsRepository.Remove(videoslistToBeRemoved);
            if (await uoW.SaveAsync())
            {
                return RedirectToAction(nameof(Index), new { @notify = "VList-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }

        private bool VideosListExists(Guid id)
        {
            return uoW.VideosListsRepository.Any(id);
        }
    }
}
