using AutoMapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Utilities.Pagination;

namespace Zaher.Ui.Controllers
{
    public class QAsController : Controller
    {
        private readonly IUoW uoW;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;
        public QAsController(IUoW uoW, IMapper mapper, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            this.uoW = uoW;
            this.mapper = mapper;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Index(string search, int page)
        {
            var qAs = await uoW.QAsRepository.GetAllQAsAsync();
            var model = mapper.Map<IEnumerable<ListQAsViewModel>>(qAs);

            if (!String.IsNullOrWhiteSpace(search))
            {
                model = model.Where(m =>
                   m.FirstName.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics())
                || m.LastName.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics())
                || m.FullName.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics())
                || m.Question.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics())
                || m.CaseNumber == search
                || m.Subject.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics()));
            }

            var paginationResult = model.AsQueryable().GetPagination(page, 5, "qas");

            return View(paginationResult);
        }

        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var qA = await uoW.QAsRepository.GetQAByIdAsync((Guid)id);

        //    if (qA == null)
        //    {
        //        return NotFound();
        //    }
        //    var model = mapper.Map<DetailsQAViewModel>(qA);

        //    return View(model);
        //}

        [AllowAnonymous]
        public IActionResult Create()
        {
            var model = new CreateQAViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateQAViewModel qA)
        {
            var qaS = await uoW.QAsRepository.GetAllQAsAsync();
            var qa = mapper.Map<QA>(qA);
            qa.PublishingDate = DateTime.Now;
            qa.SectionId = UnclassifiedIds.UnClassifiedSectionId;

            qa.CaseNumber = Randomizer.GetAUniqueString(qaS.Select(q => q.CaseNumber).ToList(), (int)Num(qaS), true);

            while (qa.CaseNumber.StartsWith("0"))
            {
                qa.CaseNumber = Randomizer.GetAUniqueString(qaS.Select(q => q.CaseNumber).ToList(), (int)Num(qaS) + 1, true);
            }

            if (ModelState.IsValid)
            {
                await uoW.QAsRepository.AddAsync(qa);
                if (await uoW.SaveAsync())
                {
                    return RedirectToAction("Index", "FBooks", new { @notify = "QA-Created" });
                }

                return View(qA);
            }

            return View(qA);
        }

        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qA = await uoW.QAsRepository.GetQAByIdAsync((Guid)id);
            if (qA == null)
            {
                return NotFound();
            }
            var model = mapper.Map<EditQAViewModel>(qA);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Edit(Guid id, EditQAViewModel viewModel)
        {
            var qA = await uoW.QAsRepository.GetQAByIdAsync(id);
            if (await TryUpdateModelAsync(viewModel, "", q => q.FirstName,
                                                        q => q.LastName,
                                                        q => q.Question,
                                                        q => q.Answer,
                                                        q => q.Subject
                                                        ))
                mapper.Map(viewModel, qA);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                    return RedirectToAction("Index", "FBooks", new { @notify = "QA-Edited" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QAExists(id))
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
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var qaToBeDeleted = await uoW.QAsRepository.GetQAByIdAsync((Guid)id);
            if (qaToBeDeleted is null)
            {
                return NotFound();
            }
            var model = mapper.Map<DeleteQAViewModel>(qaToBeDeleted);
            return View(model);
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, DeleteQAViewModel viewModel)
        {
            var qaToBeRemoved = await uoW.QAsRepository.GetQAByIdAsync(id);
            uoW.QAsRepository.Remove(qaToBeRemoved);

            if (await uoW.SaveAsync())
            {
                return RedirectToAction("Details", "Sections", new { id = qaToBeRemoved.SectionId, @notify = "QA-Deleted" });
            }
            else
            {
                return StatusCode(500);
            }
        }
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Answer(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qA = await uoW.QAsRepository.GetQAByIdAsync((Guid)id);
            if (qA == null)
            {
                return NotFound();
            }
            var model = mapper.Map<AnswerQAViewModel>(qA);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.SuperAdminAdmin)]
        public async Task<IActionResult> Answer(Guid id, AnswerQAViewModel viewModel)

        {
            var qA = await uoW.QAsRepository.GetQAByIdAsync(id);
            if (await TryUpdateModelAsync(viewModel, "", q => q.Answer,
                                                         q=>q.SectionId))

                mapper.Map(viewModel, qA);

            CheckIfAnswerIsValid(viewModel);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();

                    if (!String.IsNullOrWhiteSpace(qA.Email))
                    {
                        await emailSender.SendEmailAsync(
                            qA.Email,
                              $"تمت الإجابة عن سؤالك المتعلق ب {qA.Subject}",
                              $"<p>السلام عليكم ورحمة الله وبركاته</p>"
                           + $"<p>الأخ المكرم / الأخت المكرمة {qA.FirstName} {qA.LastName}</p>"
                           + $"<p>لقد تمت بفضل الله الإجابة عن سؤالكم ويمكنكم البحث عن السؤال في الموقع</p>"
                           + $"<p>عن طريق اسمكم أو عن طريق موضوع أو محتوى السؤال أو عن طريق رقم السؤال</p>"
                           + $"رقم السؤال : {qA.CaseNumber}"
                            );
                       
                    }

                    return RedirectToAction("Details","Sections", new {id = UnclassifiedIds.UnClassifiedSectionId, @notify = "QA-Answered" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QAExists(id))
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

        private void CheckIfAnswerIsValid(AnswerQAViewModel viewModel)
        {
            HtmlDocument document = new HtmlDocument();
            if (!String.IsNullOrEmpty(viewModel.Answer))
            {
                document.LoadHtml(viewModel.Answer);
                var textvalue = HtmlEntity.DeEntitize(document.DocumentNode.InnerText);
                var stringWithoutNewLine = textvalue.Replace("\r\n", "");

                var isEmpty = String.IsNullOrWhiteSpace(textvalue);

                if (isEmpty)
                {
                    ModelState.AddModelError("Answer", "الرجاء تعبئة الحقل");
                }

                if (stringWithoutNewLine.Length < 4)
                {
                    ModelState.AddModelError("Answer", "الرجاء أجب ب 4 أحرف كحد أدنى");
                }
            }
            else
            {
                ModelState.AddModelError("Answer", "الرجاء تعبئة الحقل");
            }
        }

        private bool QAExists(Guid id)
        {
            return uoW.QAsRepository.Any(id);
        }

        private double Num(IEnumerable<QA> qAs)
        {
            double num;
            if (qAs.Count() < 1)
            {
                num = 1;
            }
            else
            {
                num = Math.Floor(Math.Log10(qAs.Count()) + 1);
            }
            return num;
        }
        //[AllowAnonymous]
        //public async Task<IActionResult> SearchBar(string search, int page, Guid? fbookId, Guid? sectionId)
        //{
        //    if (String.IsNullOrWhiteSpace(search))
        //    {
        //        return RedirectToAction("Details", "Sections", new { id = sectionId });
        //    }
        //    var qAs = await uoW.QAsRepository.GetAllQAsAsync();
        //    var query = qAs.Where(q =>
        //    ((q.SectionId != UnclassifiedIds.ToBeCheckedSectionId)&&


        //                                      (  (q.Answered == true && (
        //                                         q.CaseNumber == search
        //                                      || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                      || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                      || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                      || q.Answer.RemoveDiacritics().Contains(search.RemoveDiacritics())
        //                                      ) && (q.SectionId == sectionId))
        //                                      ||
        //                                      (
        //                                      (
        //                                      q.CaseNumber == search
        //                                      || q.FirstName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                      || q.LastName.RemoveDiacritics().StartsWith(search.RemoveDiacritics())
        //                                      || q.Question.RemoveDiacritics().Contains(search.RemoveDiacritics()))
        //                                      && ((q.SectionId == sectionId))
        //                                      ))));
        //    var model = mapper.Map<IEnumerable<ListQAsViewModel>>(query).OrderByDescending(q => q.PublishingDate).AsQueryable().GetPagination(page, 5, "qas");
        //    //var fid = (await uoW.SectionsRepository.GetSectionByIdAsync(sectionId)). 

                    
        //    ViewData["sectionId"] = sectionId;
        //    return PartialView("_QAsSearchResult", model);
        //}

        [Authorize(Roles = Roles.AllRoles)]
        public async Task<IActionResult> MoveToBeChecked(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qA = await uoW.QAsRepository.GetQAByIdAsync((Guid)id);
            if (qA == null)
            {
                return NotFound();
            }
            var model = mapper.Map<MoveQAViewModel>(qA);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.AllRoles)]
        public async Task<IActionResult> MoveToBeChecked(Guid id, MoveQAViewModel viewModel)
        {
            var qA = await uoW.QAsRepository.GetQAByIdAsync(id);
            if (await TryUpdateModelAsync(viewModel, "", q => q.SectionId
                                                        ))

                viewModel.SectionId = UnclassifiedIds.ToBeCheckedSectionId;
                mapper.Map(viewModel, qA);

            if (ModelState.IsValid)
            {
                try
                {
                    await uoW.CompleteAsync();
                    return RedirectToAction("Index", "FBooks", new { @notify = "QA-Moved" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QAExists(id))
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


    }
}
