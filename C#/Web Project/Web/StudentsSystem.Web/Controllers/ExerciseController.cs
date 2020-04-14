namespace StudentsSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Exercise;

    public class ExerciseController : BaseController
    {
        private readonly IExercisesService exercisesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExerciseController(IExercisesService exercisesService, UserManager<ApplicationUser> userManager)
        {
            this.exercisesService = exercisesService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var exercise = this.exercisesService.GetAllExercise().Where(x => x.UserId == user.Id);

            var viewModel = new AllViewModel
            {
                Exercises = exercise,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(string homeworkId)
        {
            var inputModel = new CreateInputModel
            {
                HomeworkId = homeworkId,
            };
            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.exercisesService.CreateExerciseAsync(user.Id, inputModel);
            return this.Redirect("/Exercise/All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(string id)
        {
            var viewModel = this.exercisesService.GetExerciseById<DetailsViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string exerciseId)
        {
            var inputModel = this.exercisesService.GetExerciseById<EditInputModel>(exerciseId);

            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string exerciseId, EditInputModel inputModel)
        {
            var exercise = this.exercisesService.GetById(exerciseId);
            inputModel.Id = exercise.Id;
            inputModel.HomeworkId = exercise.HomeworkId;

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.exercisesService.UpdateAsync(inputModel);
            return this.Redirect("/Exercise/Details?id=" + inputModel.Id);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            await this.exercisesService.DeleteByIdAsync(id);
            return this.Redirect("/Exercise/All");
        }
    }
}
