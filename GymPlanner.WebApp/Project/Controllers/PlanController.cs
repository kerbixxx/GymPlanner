﻿using GymPlanner.Application.Interfaces.Repositories;
using GymPlanner.Application.Interfaces.Repositories.Plan;
using GymPlanner.Application.Interfaces.Services;
using GymPlanner.Application.Models.Plan;
using GymPlanner.Domain.Entities.Plans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;

namespace GymPlanner.WebUI.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IUserRepository _userRepo;
        private readonly IRatingService _ratingService;
        public PlanController(IPlanService planService, IUserRepository userRepo, IRatingService ratingService)
        {
            _planService = planService;
            _userRepo = userRepo;
            _ratingService = ratingService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var planDto = await _planService.GetPlanEditDtoAsync(id);
            return View(planDto);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var planDto =  await _planService.GetPlanEditDtoAsync(id);
            return View(planDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PlanEditDto planDto)
        {
            if (ModelState.IsValid)
            {
                await _planService.UpdatePlanAsync(planDto);
                TempData["SuccessMessage"] = "План успешно обновлен.";
                return RedirectToAction("Edit", new { id = planDto.PlanId });
                //return RedirectToAction("Index", "Profile", new { Id = planDto.UserId });
            }
            return await Edit(planDto);
        }

        public async Task<IActionResult> GetFilteredPlans(string tag, string sortBy, string sortOrder)
        {
            var filteredPlans = await _planService.GetFilteredPlans(tag, sortBy, sortOrder);
            return PartialView("_PlanListPartial", filteredPlans);
        }

        public IActionResult AddExerciseModal(int planId)
        {
            var dto = new ExerciseDto() { PlanId = planId, Name = "" };
            return PartialView("AddExerciseModal",dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExerсise(ExerciseDto dto)
        {
            await _planService.AddExerciseToPlan(dto);
            return RedirectToAction("Edit", new { Id = dto.PlanId});
        }

        [HttpPost]
        public async Task<IActionResult> RatePlan(int planId, string userName, int vote)
        {
            var user = await _userRepo.FindByNameAsync(userName);
            if (user == null) return BadRequest();
            await _ratingService.RatePlanAsync(planId, user.Id, vote);
            return Ok();
        }
        public IActionResult AddFrequencyModal(int planId)
        {
            var dto = new FrequencyDto() { PlanId = planId, Name = "" };
            return PartialView("AddFrequencyModal", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFrequency(FrequencyDto dto)
        {
            await _planService.AddFrequencyToPlan(dto);
            return RedirectToAction("Edit", new { Id = dto.PlanId });
        }

        [Authorize]
        public IActionResult Create()
        {
            var plan = new Plan();
            return View(plan);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plan plan)
        {
            var user = await _userRepo.FindByNameAsync(User.Identity.Name);
            plan.UserId = user.Id;
            if (ModelState.IsValid)
            {
                await _planService.AddPlanAsync(plan);
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFrequency(int frequencyId)
        {
            await _planService.DeleteFrequencyFromPlan(frequencyId);
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExercise(int exerciseId)
        {
            await _planService.DeleteExerciseFromPlan(exerciseId);
            return Json(new { success = true});
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            await _planService.DeletePlanAsync(planId);
            return Json(new { success = true });
        }
    }
}