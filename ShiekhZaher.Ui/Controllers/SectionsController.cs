using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Extensions;
using Zaher.Ui.Helpers;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;
using Zaher.Ui.Utilities.Pagination;


namespace Zaher.Ui.Controllers
{
    public class SectionsController : Controller
    {
        private readonly IUoW uoW;
        private readonly IMapper mapper;

        public SectionsController(IMapper mapper, IUoW uoW)
        {
            this.mapper = mapper;
            this.uoW = uoW;
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Index()
        {
            var sections = await uoW.SectionsRepository.GetAllSectionsAsync();
            var model = mapper.Map<IEnumerable<ListSectionsViewModel>>(sections);

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id, int page, string search)
        {
            if (id == null)
            {
                return NotFound();
            }
            var section = await uoW.SectionsRepository.GetSectionByIdAsync((Guid)id);
            var qAs = await uoW.QAsRepository.GetAllQAsAsync();

            if (section == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
            var query = qAs.Where(q =>
            ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId) &&
                                              ((q.Answered == true && (
                                                 q.CaseNumber == search
                                              || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                              || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                              || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                              || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                              ) && (q.SectionId == section.Id))
                                              ||
                                              (
                                              (
                                              q.CaseNumber == search
                                              || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                              || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                              || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics()))
                                              && ((q.SectionId == section.Id))
                                              ))));
            var model = mapper.Map<DetailsSectionViewModel>(section);
            var qasModel = mapper.Map<IEnumerable<ListQAsViewModel>>(query).ToList();
            model.QAs = qasModel;
            model.QAsResult = model.QAs.OrderByDescending(m => m.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");
            return View(model);
            }
            else
            {
                var model = mapper.Map<DetailsSectionViewModel>(section);
                var qasModel = mapper.Map<IEnumerable<ListQAsViewModel>>(qAs).ToList();
                model.QAsResult = model.QAs.OrderByDescending(m => m.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");
                return View(model);
            }
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public IActionResult Create(Guid? fbookId)
        {
            var viewModel = new CreateSectionViewModel();
            if (fbookId is not null)
            {
                viewModel.FBookId = fbookId;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Create(CreateSectionViewModel viewModel)
        {
            var section = mapper.Map<Section>(viewModel);
            if (viewModel.FBookId is null)
            {
                ModelState.AddModelError("FBookId", "الرجاء اختيار القسم");
            }
            if (ModelState.IsValid)
            {
                await uoW.SectionsRepository.AddAsync(section);
                if (await uoW.SaveAsync())
                {
                    return RedirectToAction("Details", "FBooks", new { Id = section.FBookId, @notify = "Section-Created" });
                }

                return View(viewModel);
            }
            return View(viewModel);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var section = await uoW.SectionsRepository.GetSectionByIdAsync((Guid)id);
            if (section == null)
            {
                return NotFound();
            }

            var model = mapper.Map<EditSectionViewModel>(section);
            if (id == UnclassifiedIds.UnClassifiedSectionId || id == UnclassifiedIds.ToBeCheckedSectionId)
            {
                return PartialView("_CanNotEditSection", model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Edit(Guid id, EditSectionViewModel model)
        {
            var section = await uoW.SectionsRepository.GetSectionByIdAsync(id);

            if (await TryUpdateModelAsync(model, "", f => f.Title,
                                                     f => f.Description,
                                                     f => f.FBookId

                                                                    ))
                mapper.Map(model, section);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                    return RedirectToAction("Details", "FBooks", new {id = model.FBookId,  @notify = "Section-Edited" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await uoW.SectionsRepository.GetSectionByIdAsync((Guid)id);
            if (section == null)
            {
                return NotFound();
            }
            var model = mapper.Map<DeleteSectionViewModel>(section);
            if (id == UnclassifiedIds.UnClassifiedSectionId || id == UnclassifiedIds.ToBeCheckedSectionId)
            {
                return PartialView("_CanNotDeleteSection", model);
            }

            return View(model);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, DeleteSectionViewModel viewModel)
        {
            var section = await uoW.SectionsRepository.GetSectionByIdAsync(id);
            uoW.SectionsRepository.Remove(section);
            if (await uoW.SaveAsync())
            {
                return RedirectToAction("Details", "FBooks", new { id = section.FBookId, @notify = "Section-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }

        private bool SectionExists(Guid id)
        {
            return uoW.SectionsRepository.Any(id);
        }
        //[AllowAnonymous]
        //public async Task<IActionResult> SearchBar(string search, string type, int page, Guid? fbookId, Guid? sectionId)
        //{
        //    if (String.IsNullOrWhiteSpace(search))
        //    {
        //        return RedirectToAction("Details", "FBooks", new { id = fbookId });
        //    }

        //    if (type == "QAs")
        //    {
        //        var qAs = await uoW.QAsRepository.GetAllQAsAsync(true);


        //        var query = qAs.Where(q =>

        //        ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId)&&
        //                                            ((q.Answered == true && (
        //                                             q.CaseNumber == search
        //                                          || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                          || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                          || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                          || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                          || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                          ) && (q.Section.FBookId == fbookId))
        //                                          ||
        //                                          (
        //                                          (
        //                                          q.CaseNumber == search
        //                                          || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                          || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                          || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                          || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics()))
        //                                          && ((q.Section.FBookId == fbookId))
        //                                          ))));

        //        var model = mapper.Map<IEnumerable<ListQAsViewModel>>(query).OrderByDescending(q => q.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");
        //        ViewData["fbookId"] = fbookId;
        //        return PartialView("_QAsSearchResult", model);
        //    }
        //    if (type == "Sections")
        //    {
        //        var sections = await uoW.SectionsRepository.GetAllSectionsAsync();
        //        var query = sections.Where(s => (s.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                                        || s.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()))
        //                                                        && (s.FBookId == fbookId)
        //                                                        );

        //        return PartialView("_SectionsSearchResult", mapper.Map<IEnumerable<ListSectionsViewModel>>(query).AsQueryable().GetPagination(page, 9, ""));
        //    }
        //    else
        //    {
        //        return RedirectToAction("Details", "FBooks", fbookId);
        //    }
        //}
    }
}
