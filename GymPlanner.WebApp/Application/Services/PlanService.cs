﻿using GymPlanner.Application.Configurations;
using GymPlanner.Application.Interfaces.Repositories.Plan;
using GymPlanner.Application.Interfaces.Services;
using GymPlanner.Application.Models.Plan;
using GymPlanner.Domain.Entities.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymPlanner.Application.Services
{
    public class PlanService : IPlanService
    {
        private readonly DefaultNamesOptions _options;
        private readonly IPlanRepository _planRepo;
        private readonly IPlanExerciseFrequencyRepository _pefRepo;
        private readonly IFrequencyService _frequencyService;
        private readonly IRatingService _ratingService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IExerciseService _exerciseService;
        public PlanService(IPlanRepository planRepo,
                           IPlanExerciseFrequencyRepository pefRepo,
                           IFrequencyService frequencyService,
                           IRatingService ratingService,
                           ISubscriptionService subscriptionService,
                           IExerciseService exerciseService,
                           DefaultNamesOptions options)
        {
            _planRepo = planRepo;
            _pefRepo = pefRepo;
            _frequencyService = frequencyService;
            _ratingService = ratingService;
            _subscriptionService = subscriptionService;
            _exerciseService = exerciseService;
            _options = options;
        }


        public async Task AddPlanAsync(Plan plan)
        {
            plan.CreatedAt = DateTime.Now;
            await _planRepo.AddAsync(plan);
            var frequencyId = await _exerciseService.AddExerciseToPlan(new ExerciseDto() { Name = _options.DEFAULT_EXERCISE_NAME,PlanId = plan.Id});
            var exerciseId = await _frequencyService.AddFrequencyToPlan(new FrequencyDto() { Name = _options.DEFAULT_FREQUENCY_NAME, PlanId = plan.Id });
            PlanExerciseFrequency pef = new()
            {
                PlanId = plan.Id,
                FrequencyId = frequencyId,
                ExerciseId = exerciseId,
                Description = _options.DEFAULT_DESCRIPTION
            };
            await _pefRepo.AddAsync(pef);
        }

        public async Task UpdatePlanAsync(Plan plan)
        {
            await _planRepo.UpdateAsync(plan);
        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            return await _planRepo.GetAll();
        }

        public async Task<List<GetPlansOnIndexDto>> GetFilteredPlans(string? tag, string? sortBy, string? sortOrder)
        {
            var plans = await _planRepo.GetAll();
            List<Plan> filteredPlans = new();
            if(tag != null)
            {
                filteredPlans = plans.Where(t => t.Tags.Any(t=>t.Contains(tag))).ToList();
            }
            else
            {
                filteredPlans = plans;
            }
            List<GetPlansOnIndexDto> plansDto = new();
            foreach(var plan in filteredPlans)
            {
                var planDto = new GetPlansOnIndexDto()
                {
                    UserId = plan.UserId,
                    PlanId = plan.Id,
                    FullDescription = plan.FullDescription,
                    MenuDescription = plan.MenuDescription,
                    CreatedAt = plan.CreatedAt,
                    Name = plan.Name,
                    Tags = plan.TagsDb,
                };
                planDto.AverageRating = await _ratingService.GetAverageRatingForPlan(plan.Id);
                plansDto.Add(planDto);
            }
            if (sortBy != null)
            {
                if (sortBy == "Rating")
                {
                    plansDto = sortOrder == "asc" ? plansDto.OrderBy(t => t.AverageRating).ToList() : plansDto.OrderByDescending(t => t.AverageRating).ToList();
                }
                if (sortBy == "CreatedDate")
                {
                    plansDto = sortOrder == "asc" ? plansDto.OrderBy(t => t.CreatedAt).ToList() : plansDto.OrderByDescending(t => t.CreatedAt).ToList();
                }
                //Сортировка по комментариям
            }
            return plansDto;
        }

        public async Task UpdatePlanAsync(PlanEditDto planDto)
        {
            var plan = new Plan()
            {
                Id = planDto.PlanId,
                Name = planDto.Name,
                UserId = planDto.UserId,
                FullDescription = planDto.FullDescription,
                MenuDescription = planDto.MenuDescription,
                TagsDb = planDto.TagsString
            };
            var trimmedTags = plan.Tags.Select(tag => tag.Trim()).ToArray();
            plan.Tags = trimmedTags;
            await _planRepo.UpdateAsync(plan);
            foreach (var pef in planDto.ExerciseFrequencies)
            {
                await _pefRepo.UpdateAsync(new PlanExerciseFrequency() { Id = pef.Id, ExerciseId = pef.ExerciseId, PlanId = planDto.PlanId, FrequencyId = pef.FrequencyId, Description = pef.Description });
            }
            foreach (var excersise in planDto.Exercises)
            {
                await _exerciseService.UpdateAsync(excersise);
            }
            foreach (var frequency in planDto.Frequencies)
            {
                await _frequencyService.UpdateAsync(frequency);
            }
            await _subscriptionService.NotifyAllSubscribers(plan.Id);
        }
        public async Task DeletePlanAsync(int id)
        {
            var plan = await _planRepo.GetAsync(id);
            await _planRepo.RemoveAsync(plan);
        }

        public async Task<PlanEditDto> GetPlanEditDtoAsync(int id)
        {
            var plan = await _planRepo.GetAsync(id);
            await CheckIfEmpty(plan);
            var excfreqList = new List<ExerciseFrequencyDto>();
            foreach (var pef in plan.planExersiseFrequencies)
            {
                var excfreq = new ExerciseFrequencyDto()
                {
                    Description = pef.Description,
                    ExerciseId = pef.Exercise.Id,
                    FrequencyId = pef.FrequencyId,
                    Id = pef.Id
                };
                excfreqList.Add(excfreq);
            }
            var planDto = new PlanEditDto()
            {
                PlanId = plan.Id,
                ExerciseFrequencies = excfreqList,
                Name = plan.Name,
                Exercises = plan.planExersiseFrequencies.Select(pef => pef.Exercise).Distinct().ToList(),
                Frequencies = plan.planExersiseFrequencies.Select(pef => pef.Frequency).Distinct().ToList(),
                UserId = plan.UserId,
                MenuDescription = plan.MenuDescription,
                FullDescription = plan.FullDescription,
                CreatedAt = plan.CreatedAt,
                TagsString = plan.TagsDb
            };
            return planDto;
        }

        private async Task CheckIfEmpty(Plan plan)
        {
            if (plan.planExersiseFrequencies.Count() == 0)
            {
                var frequencyId = await _exerciseService.AddExerciseToPlan(new ExerciseDto() { Name = _options.DEFAULT_EXERCISE_NAME, PlanId = plan.Id });
                var exerciseId = await _frequencyService.AddFrequencyToPlan(new FrequencyDto() { Name = _options.DEFAULT_FREQUENCY_NAME, PlanId = plan.Id });
                PlanExerciseFrequency pef = new()
                {
                    PlanId = plan.Id,
                    FrequencyId = frequencyId,
                    ExerciseId = exerciseId,
                    Description = _options.DEFAULT_DESCRIPTION
                };
                await _pefRepo.AddAsync(pef);
            }
        }

        public async Task<PlanDetailsDto> GetPlanDetailsDtoAsync(int id, int userId)
        {
            var plan = await _planRepo.GetAsync(id);
            var excfreqList = new List<ExerciseFrequencyDto>();
            foreach (var pef in plan.planExersiseFrequencies)
            {
                var excfreq = new ExerciseFrequencyDto()
                {
                    Description = pef.Description,
                    ExerciseId = pef.Exercise.Id,
                    FrequencyId = pef.FrequencyId,
                    Id = pef.Id
                };
                excfreqList.Add(excfreq);
            }
            var isSubbed = _subscriptionService.IsUserSubbedToPlan(userId, plan.Id);
            var planDto = new PlanDetailsDto()
            {
                PlanId = plan.Id,
                ExerciseFrequencies = excfreqList,
                Name = plan.Name,
                Exercises = plan.planExersiseFrequencies.Select(pef => pef.Exercise).Distinct().ToList(),
                Frequencies = plan.planExersiseFrequencies.Select(pef => pef.Frequency).Distinct().ToList(),
                UserId = plan.UserId,
                MenuDescription = plan.MenuDescription,
                FullDescription = plan.FullDescription,
                CreatedAt = plan.CreatedAt,
                TagsString = plan.TagsDb,
                IsSubscribed = isSubbed
            };
            return planDto;
        }
    }
}
