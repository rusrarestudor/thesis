using GymMonitorAPI.DAL;
using GymMonitorAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GymMonitorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserDAL _dal;

        public UserProfileController()
        {
            _dal = new UserDAL("YourConnectionStringHere");
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUserProfiles()
        {
            return _dal.GetUserProfiles();
        }

        [HttpPost]
        public IActionResult AddUserProfile(UserDTO userProfile)
        {
            _dal.AddUserProfile(userProfile);
            return CreatedAtAction(nameof(GetUserProfiles), new { id = userProfile.Id }, userProfile);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserProfile(int id, UserDTO userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _dal.UpdateUserProfile(userProfile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserProfile(int id)
        {
            _dal.DeleteUserProfile(id);
            return NoContent();
        }
    }
}
