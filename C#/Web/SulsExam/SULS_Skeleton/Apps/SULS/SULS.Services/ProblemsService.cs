using SULS.Data;
using SULS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SULS.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly SULSContext db;

        public ProblemsService(SULSContext db)
        {
            this.db = db;
        }

        public void Create(string name, int points)
        {
            var probelm = new Problem
            {
                Name = name,
                Points = points
            };

            this.db.Problems.Add(probelm);
            this.db.SaveChanges();
        }

        public List<ProblemInfoViewModel> GetAllProblems()
        {
            var count = db.Problems.Count();

            var problems = db.Problems.Select(x => new ProblemInfoViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Count = count
            }).ToList();

            return problems;
        }
    }
}
