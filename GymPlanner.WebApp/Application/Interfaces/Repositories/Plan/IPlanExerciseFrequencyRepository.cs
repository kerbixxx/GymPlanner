﻿using GymPlanner.Domain.Entities.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymPlanner.Application.Interfaces.Repositories.Plan
{
    public interface IPlanExerciseFrequencyRepository : IRepository<PlanExerciseFrequency>
    {
        Task<List<PlanExerciseFrequency>> GetByPlanId(int planId);
    }
}
