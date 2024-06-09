using GymMonitorAPI.DAL;
using GymMonitorAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GymMonitorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymMachineController : ControllerBase
    {
        private readonly GymMachineDAL _dal;

        public GymMachineController()
        {
            _dal = new GymMachineDAL("YourConnectionStringHere");
        }

        [HttpGet]
        public ActionResult<IEnumerable<GymMachineDTO>> GetGymMachines()
        {
            return _dal.GetGymMachines();
        }

        [HttpPost]
        public IActionResult AddGymMachine(GymMachineDTO gymMachine)
        {
            _dal.AddGymMachine(gymMachine);
            return CreatedAtAction(nameof(GetGymMachines), new { id = gymMachine.Id }, gymMachine);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGymMachine(int id, GymMachineDTO gymMachine)
        {
            if (id != gymMachine.Id)
            {
                return BadRequest();
            }

            _dal.UpdateGymMachine(gymMachine);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGymMachine(int id)
        {
            _dal.DeleteGymMachine(id);
            return NoContent();
        }
    }
}
