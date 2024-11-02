// WorkoutSessionController - для ведения и завершения тренировок.
using Microsoft.AspNetCore.Mvc;
using Sportify.Models;
using Sportify.Services.Interfaces;

namespace Sportify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutSessionController : Controller
    {
        private readonly IWorkoutService _service;
        public WorkoutSessionController(IWorkoutService workout) 
        {
            _service = workout;
        }
        // Плучение тренировок по id пользователя
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutsByUserIdAsync(int id)
        {
            var workouts = await _service.GetAllUserWorkouts(id);
            return Ok(workouts);
        }

        // Получение тренировки по id
        [HttpGet("Workout/{id}")]
        public async Task<IActionResult> GetWorkoutByIdAsync(int id)
        {
            try
            {
                var workout = await _service.GetWorkoutById(id);
                if(workout == null)
                {
                    return BadRequest("Тренування не знайдено!");
                }
                return Ok(workout);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Помилка на сервері");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutAsync([FromBody] Workout workout)
        {
            try
            {
                if (await _service.CreateWorkoutAsync(workout))
                {
                    return Ok("Тренування додано");
                }
                return BadRequest("Помилка створення тренування");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Помилка на сервері");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] Workout workout)
        {
            try
            {
                if(await _service.UpdateWorkout(id, workout))
                {
                    return NoContent();
                }
                return BadRequest("Помилка оновлення тренування");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Помилка на сервері");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            try
            {
                if(await _service.DeleteWorkout(id))
                {
                    return NoContent();
                }
                return BadRequest("Помилка видалення тренування");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Помилка на сервері");
            }
        }
    }
}
