using SULS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.Services
{
    public interface IProblemsService
    {
        List<ProblemInfoViewModel> GetAllProblems();

        void Create(string name, int points);
    }
}
