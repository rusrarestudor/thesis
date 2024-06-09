using GymMonitorAPI.DAL;
using GymMonitorAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GymMonitorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly WorkoutDAL _dal;

        public WorkoutController()
        {
            _dal = new WorkoutDAL("YourConnectionStringHere");
        }

        [HttpGet]
        public ActionResult<IEnumerable<WorkoutDTO>> GetWorkouts()
        {
            return _dal.GetWorkouts();
        }

        [HttpPost]
        public IActionResult AddWorkout(WorkoutDTO workout)
        {
            _dal.AddWorkout(workout);
            return CreatedAtAction(nameof(GetWorkouts), new { id = workout.Id }, workout);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkout(int id, WorkoutDTO workout)
        {
            if (id != workout.Id)
            {
                return BadRequest();
            }

            _dal.UpdateWorkout(workout);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkout(int id)
        {
            _dal.DeleteWorkout(id);
            return NoContent();
        }
    }
}
