// ExerciseController - для добавления и редактирования упражнений.
using Microsoft.AspNetCore.Mvc;
using Sportify.Models;
using Sportify.Services;
using System.Threading.Tasks;

namespace Sportify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // Получение списка всех упражнений
        [HttpGet]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _exerciseService.GetAllExercisesAsync();
            return Ok(exercises);
        }

        // Получение упражнения по Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        // Добавление нового упражнения
        [HttpPost]
        public async Task<IActionResult> CreateExercise([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _exerciseService.AddExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, exercise);
        }

        // Обновление существующего упражнения
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return BadRequest("Id параметр и Id упражнения не совпадают.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingExercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (existingExercise == null)
            {
                return NotFound();
            }

            await _exerciseService.UpdateExerciseAsync(exercise);
            return NoContent(); // Возвращает статус 204 (No Content), указывающий на успешное обновление
        }

        // Удаление упражнения
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
    }
}
