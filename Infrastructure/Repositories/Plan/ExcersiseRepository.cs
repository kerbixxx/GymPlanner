﻿using GymPlanner.Application.Interfaces.Repositories.Plan;
using GymPlanner.Domain.Entities.Plans;
using GymPlanner.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymPlanner.Infrastructure.Repositories.Plan
{
    public class ExcersiseRepository : Repository<Excersise>, IExcersiseRepository
    {
        private readonly PlanDbContext _db;
        public ExcersiseRepository(PlanDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
