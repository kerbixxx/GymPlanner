﻿using GymPlanner.Application.Interfaces.Repositories;
using GymPlanner.Infrastructure.Contexts;
using GymPlanner.Domain.Entities.Plan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymPlanner.Infrastructure.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly ApplicationDbContext _db;
        public PlanRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async override Task<Plan> Get(int id)
        {
            return await _db.Plans.Include(p => p.planExcersiseFrequencies)
                    .ThenInclude(p=>p.Frequency)
                .Include(p=>p.planExcersiseFrequencies)
                    .ThenInclude(p=>p.Excersise).
                FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
