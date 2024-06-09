namespace GymMonitorAPI.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public ICollection<WorkoutDTO> Workouts { get; set; }
    }

}
