﻿using GymPlanner.Infrastructure.Contexts;
using GymPlanner.Domain.Entities.Plans;
using Microsoft.EntityFrameworkCore;
using GymPlanner.Application.Interfaces.Repositories.Plan;

namespace GymPlanner.Infrastructure.Repositories.Plan
{
    public class PlanRepository : Repository<Domain.Entities.Plans.Plan>, IPlanRepository
    {
        public PlanRepository(PlanDbContext db) : base(db)
        {
        }

        public override async Task<Domain.Entities.Plans.Plan> GetAsync(int id)
        {
            return await _db.Plans.Include(p => p.planExersiseFrequencies)
                    .ThenInclude(p => p.Frequency)
                .Include(p => p.planExersiseFrequencies)
                    .ThenInclude(p => p.Exercise)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Domain.Entities.Plans.Plan>> GetAll()
        {
            return await _db.Plans.ToListAsync();
        }
    }
}
