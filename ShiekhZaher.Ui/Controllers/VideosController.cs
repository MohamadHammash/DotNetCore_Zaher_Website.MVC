using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Filters;
using Zaher.Ui.Helpers;
using Zaher.Ui.Models.ViewModels.VideosViewModels;

namespace Zaher.Ui.Controllers
{
    public class VideosController : Controller
    {
        private IMapper mapper;
        private readonly IUoW uoW;

        public VideosController(IMapper mapper, IUoW uoW)
        {
            this.mapper = mapper;
            this.uoW = uoW;
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Index()
        {
            var videos = await uoW.VideosRepository.GetAllVideosAsync();
            var models = mapper.Map<IEnumerable<ListVideosViewModel>>(videos);
            models.OrderByDescending(vl => vl.PublishingDate);

            return View(models);
        }

        [ModelNotNull]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var video = await uoW.VideosRepository.GetVideoByIdAsync((Guid)id);
            var model = mapper.Map<DetailsVideoViewModel>(video);
            return View(model);
        }
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public IActionResult Create(Guid? videosListId)
        {
           

            var createVideo = new CreateVideoViewModel();
            if (videosListId is not null)
            {
                createVideo.VideosListId = (Guid)videosListId;
            }

            return View(createVideo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Create(CreateVideoViewModel createVideo)
        {
            var video = mapper.Map<Video>(createVideo);
            video.PublishingDate = DateTime.Now;
            if (createVideo.VideosListId is null)
            {
                ModelState.AddModelError("VideosListId", "الرجاء اختيار قائمة التشغيل");
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            video.ApplicationUserId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Email == userEmail).FirstOrDefault().Id;

            if (ModelState.IsValid)
            {
                await uoW.VideosRepository.AddAsync(video);
                if (await uoW.SaveAsync())
                {
                    return RedirectToAction("Details", "VideosLists", new { Id = video.VideosListId, @notify = "Video-Created" });
                }
            }
            return View(createVideo);
        }
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await uoW.VideosRepository.GetVideoByIdAsync((Guid)id);
            if (video == null)
            {
                return NotFound();
            }

            var viewmodel = mapper.Map<EditVideoViewModel>(video);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (video.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToEditVideo", viewmodel);
                }
            }

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelNotNull, ModelValid]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Edit(Guid id, EditVideoViewModel viewmodel)
        {
            if (id != viewmodel.Id)
            {
                return NotFound();
            }

            var video = await uoW.VideosRepository.GetVideoByIdAsync(id);

            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (video.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToEditVideo", mapper.Map<EditVideoViewModel>(video));
                }
            }

            if (await TryUpdateModelAsync(viewmodel, "", v => v.Title,
                                                            v => v.VideosListId))
                mapper.Map(viewmodel, video);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index", "VideosLists", new { @notify = "Video-Edited" });
        }

        // GET: Videos/Delete/5
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await uoW.VideosRepository.GetVideoByIdAsync((Guid)id);

            if (video == null)
            {
                return NotFound();
            }

            var model = mapper.Map<DeleteVideoViewModel>(video);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (video.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToDeleteVideo", model);
                }
            }
            return View(model);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = Roles.SuperAdminAdminEditor)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, DeleteVideoViewModel viewmodel)
        {
            TempData["VideosListId"] = viewmodel.VideosListId;
            var temp = TempData["VideosListId"];
            var video = await uoW.VideosRepository.GetVideoByIdAsync(id);
            if ((!User.IsInRole(Roles.SuperAdmin) && (!User.IsInRole(Roles.Admin))))
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userid = (await uoW.UsersRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == userEmail).Id;
                if (video.ApplicationUserId != userid)
                {
                    return PartialView("_NotAllowedToDeleteVideo", mapper.Map<DeleteVideoViewModel>(video));
                }
            }


            uoW.VideosRepository.Remove(video);
            if (await uoW.SaveAsync())
            {
                return RedirectToAction("Details", "VideosLists", new { Id = (Guid)temp, @notify = "Video-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }

        private bool VideoExists(Guid id)
        {
            return uoW.VideosRepository.Any(id);
        }
    }
}
