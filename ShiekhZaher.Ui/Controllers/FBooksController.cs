using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Ui.Extensions;
using Zaher.Ui.Helpers;
using Zaher.Ui.Models.ViewModels.FBooksViewModels;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Controllers
{
   
    public class FBooksController : Controller
    {
        private readonly IUoW uoW;
        private readonly IMapper mapper;

        public FBooksController(IMapper mapper, IUoW uoW)
        {
            this.mapper = mapper;
            this.uoW = uoW;
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> Index(int page)
        //{
        //    var fbooks = await uoW.FBooksRepository.GetAllFBooksAsync();
        //    if (!
        //        (User.IsInRole(Roles.Admin)
        //     || User.IsInRole(Roles.SuperAdmin)
        //     || User.IsInRole(Roles.Editor)
        //     || User.IsInRole(Roles.Moderator)
        //        ))
        //    {
        //        fbooks = fbooks.Where(f => f.Id != UnclassifiedIds.UnClassifiedFbookId);
        //    }
        //    var model = mapper.Map<IEnumerable<ListFBooksViewModel>>(fbooks).AsQueryable().GetPagination(page, 9, "");

        //    return View(model);
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page, string type, string search)

        {
            var indexmodel = new IndexFBooksViewModel()
            {
                Type = type,
                Page = page,
                SearchBox = search
            };
            PaginationResult<ListFBooksViewModel> fbooksModel;
            PaginationResult<ListSectionsViewModel> sectionsModel;
            PaginationResult<ListQAsViewModel> qasModel;


            var fbooks = await uoW.FBooksRepository.GetAllFBooksAsync();

            if (!
                (User.IsInRole(Roles.Admin)
             || User.IsInRole(Roles.SuperAdmin)
             || User.IsInRole(Roles.Editor)
             || User.IsInRole(Roles.Moderator)
                ))
            {
                fbooks = fbooks.Where(f => f.Id != UnclassifiedIds.UnClassifiedFbookId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (type == "QAs")
                    {
                        var query = (await uoW.QAsRepository.GetAllQAsAsync()).Where(q =>
               ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId) &&

                                                  (
                                                  q.Answered == true && (
                                                    q.CaseNumber == search
                                                 || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 )
                                                 ||
                                                 (
                                                 q.CaseNumber == search
                                                 || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 ))));

                        qasModel = mapper.Map<IEnumerable<ListQAsViewModel>>(query).OrderByDescending(q => q.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");
                        indexmodel.QAs = qasModel;
                    }
                    else if (type == "Sections")
                    {
                        var query = (await uoW.SectionsRepository.GetAllSectionsAsync()).Where(s => s.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                                        || s.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()));
                        sectionsModel = mapper.Map<IEnumerable<ListSectionsViewModel>>(query).AsQueryable().GetPagination(page, 9, "");
                        indexmodel.Sections = sectionsModel;

                    }
                    else if (type == "FBooks")
                    {

                        fbooks = fbooks.Where(f => f.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                                 || f.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()));

                    }


                }

            }

            fbooksModel = mapper.Map<IEnumerable<ListFBooksViewModel>>(fbooks).AsQueryable().GetPagination(page, 9, "");



            indexmodel.FBooks = fbooksModel;


            return View(indexmodel);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id, string search, string type, int page)
        {

            if (id == null)
            {
                return NotFound();
            }



            var fbook = await uoW.FBooksRepository.GetFBookByIdAsync((Guid)id);

            if (fbook == null)
            {
                return NotFound();
            }


            var indexmodel = new IndexSectionsViewModel()
            {
                Type = type,
                Page = page,
                SearchBox = search,
                FBookId = (Guid)id,
                FBookTitle = fbook.Title
            };
            var sections = fbook.Sections;
            PaginationResult<ListSectionsViewModel> sectionsModel;
            PaginationResult<ListQAsViewModel> qasModel;

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (type == "QAs")
                    {
                        var query = (await uoW.QAsRepository.GetAllQAsAsync()).Where(q =>
               ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId) &&

                                                  (
                                                  q.Answered == true && (
                                                    q.CaseNumber == search
                                                 || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 )
                                                 ||
                                                 (
                                                 q.CaseNumber == search
                                                 || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                 || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                 ))));



                        qasModel = mapper.Map<IEnumerable<ListQAsViewModel>>(query).OrderByDescending(q => q.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");


                        indexmodel.QAs = qasModel;
                    }
                    else if (type == "Sections")
                    {
                        var query = (await uoW.SectionsRepository.GetAllSectionsAsync()).Where(s => s.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                                        || s.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()));
                        sectionsModel = mapper.Map<IEnumerable<ListSectionsViewModel>>(query).AsQueryable().GetPagination(page, 9, "");

                        if (!
                              (User.IsInRole(Roles.Admin)
                                || User.IsInRole(Roles.SuperAdmin)
                                || User.IsInRole(Roles.Editor)
                                || User.IsInRole(Roles.Moderator)))
                        {
                            sectionsModel.Results = sectionsModel.Results.Where(r => r.Id != UnclassifiedIds.UnClassifiedSectionId).AsQueryable().GetPagination(page, 9, "").Results;
                        }


                        indexmodel.Sections = sectionsModel;

                    }
                }

            }
            else
            {
            sectionsModel = mapper.Map<IEnumerable<ListSectionsViewModel>>(sections).AsQueryable().GetPagination(page, 9, "");
            indexmodel.Sections = sectionsModel;
            }

            return View(indexmodel);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public IActionResult Create()
        {
            var viewModel = new CreateFBookViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFBookViewModel viewModel)
        {
            var fbook = mapper.Map<FBook>(viewModel);

            if (ModelState.IsValid)
            {
                await uoW.FBooksRepository.AddAsync(fbook);
                if (await uoW.SaveAsync())
                {
                    return RedirectToAction(nameof(Index), new { @notify = "FBook-Created" });
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
            var fBook = await uoW.FBooksRepository.GetFBookByIdAsync((Guid)id);
            if (fBook == null)
            {
                return NotFound();
            }
            var model = mapper.Map<EditFBookViewModel>(fBook);

            if (id == UnclassifiedIds.UnClassifiedFbookId)
            {
                return PartialView("_CanNotEditFBook", model);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditFBookViewModel model, string triggeredFromDetails)
        {
            var fBook = await uoW.FBooksRepository.GetFBookByIdAsync(id);

            if (await TryUpdateModelAsync(model, "", f => f.Title,
                                                                    f => f.Description))
                mapper.Map(model, fBook);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();

                    return RedirectToAction(nameof(Index), new { @notify = "FBook-Edited" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FBookExists(id))
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
            if (id == UnclassifiedIds.UnClassifiedFbookId)
            {
                return PartialView("_CanNotDeleteFBook");
            }
            var fBook = await uoW.FBooksRepository.GetFBookByIdAsync((Guid)id);
            if (fBook == null)
            {
                return NotFound();
            }
            var model = mapper.Map<DeleteFBookViewModel>(fBook);

            return View(model);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, DeleteFBookViewModel viewModel)
        {
            var fbook = await uoW.FBooksRepository.GetFBookByIdAsync(id);
            uoW.FBooksRepository.Remove(fbook);
            if (await uoW.SaveAsync())
            {
                return RedirectToAction(nameof(Index), new { @notify = "FBook-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }

        private bool FBookExists(Guid id)
        {
            return uoW.FBooksRepository.Any(id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SearchBar(string search, string type, int page)
        {
            if (String.IsNullOrWhiteSpace(search))
            {
                return RedirectToAction(nameof(Index));
            }
            if (type == "QAs")
            {
                var qAs = await uoW.QAsRepository.GetAllQAsAsync();

                var query = qAs.Where(q =>
                ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId) &&

                                                   (
                                                   q.Answered == true && (
                                                     q.CaseNumber == search
                                                  || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                  || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                  || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                  || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                  || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                  )
                                                  ||
                                                  (
                                                  q.CaseNumber == search
                                                  || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                  || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
                                                  || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                  || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                  ))));

                var model = mapper.Map<IEnumerable<ListQAsViewModel>>(query).OrderByDescending(q => q.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");

                return PartialView("_QAsSearchResult", model);
            }
            if (type == "Sections")
            {
                var sections = await uoW.SectionsRepository.GetAllSectionsAsync();
                var query = sections.Where(s => s.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                                || s.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()));
                return PartialView("_SectionsSearchResult", mapper.Map<IEnumerable<ListSectionsViewModel>>(query).AsQueryable().GetPagination(page, 9, ""));
            }
            if (type == "FBooks")
            {
                var fBooks = await uoW.FBooksRepository.GetAllFBooksAsync();
                var query = fBooks.Where(f => f.Title.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                                         || f.Description.RemoveDiacritics().Contains(search.RemoveDiacritics()));

                return View(nameof(Index), mapper.Map<IEnumerable<ListFBooksViewModel>>(query).AsQueryable().GetPagination(page, 9, ""));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> UnAnsweredQuestions(string search, int page)
        {
            var qas = await uoW.QAsRepository.GetAllQAsAsync();

            if (!
             (User.IsInRole(Roles.Admin)
           || User.IsInRole(Roles.SuperAdmin)
           || User.IsInRole(Roles.Editor)
           || User.IsInRole(Roles.Moderator)
              ))
            {
                qas = qas.Where(q => q.SectionId != UnclassifiedIds.ToBeCheckedSectionId);
            }
            PaginationResult<ListQAsViewModel> model;
            if (String.IsNullOrWhiteSpace(search))
            {
                model = mapper.Map<IEnumerable<ListQAsViewModel>>(qas)
                    .Where(q => !q.Answered)
                    .AsQueryable().GetPagination(page, 5, "qas");
            }
            else
            {
                model = mapper.Map<IEnumerable<ListQAsViewModel>>(qas)
                    .Where(q => !q.Answered
                                            &&
                                            (
                                            q.CaseNumber == search
                                          || q.FirstName.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.LastName.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())))
                    .AsQueryable().GetPagination(page, 5, "qas");
            }
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> NewQuestions(string search, int page)
        {
            var qas = await uoW.QAsRepository.GetAllQAsAsync();

            if (!
              (User.IsInRole(Roles.Admin)
            || User.IsInRole(Roles.SuperAdmin)
            || User.IsInRole(Roles.Editor)
            || User.IsInRole(Roles.Moderator)
               ))
            {
                qas = qas.Where(q => q.SectionId != UnclassifiedIds.ToBeCheckedSectionId);
            }


            PaginationResult<ListQAsViewModel> model;
            if (String.IsNullOrWhiteSpace(search))
            {
                model = mapper.Map<IEnumerable<ListQAsViewModel>>(qas)
                    .Where(q => q.New)
                    .AsQueryable().GetPagination(page, 5, "qas");
            }
            else
            {
                model = mapper.Map<IEnumerable<ListQAsViewModel>>(qas)
                    .Where(q => q.New
                                            &&
                                            (
                                            q.CaseNumber == search
                                          || q.FirstName.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.LastName.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
                                          || q.Subject.RemoveDiacritics().Contains(search.RemoveDiacritics())))
                    .AsQueryable().GetPagination(page, 5, "qas");
            }
            return View(model);
        }
    }
}
