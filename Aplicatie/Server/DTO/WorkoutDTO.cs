namespace GymMonitorAPI.DTO
{
    public class WorkoutDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int UserProfileId { get; set; }
        public UserDTO UserProfile { get; set; }
    }

}
