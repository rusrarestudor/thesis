using GymMonitorAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymMonitorAPI.DAL
{
    public class WorkoutDAL
    {
        private readonly string _connectionString;

        public WorkoutDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<WorkoutDTO> GetWorkouts()
        {
            var workouts = new List<WorkoutDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetWorkouts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var workout = new WorkoutDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
                            };
                            workouts.Add(workout);
                        }
                    }
                }
            }

            return workouts;
        }

        public void AddWorkout(WorkoutDTO workout)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_AddWorkout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Date", workout.Date);
                    command.Parameters.AddWithValue("@Description", workout.Description);
                    command.Parameters.AddWithValue("@UserProfileId", workout.UserProfileId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWorkout(WorkoutDTO workout)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_UpdateWorkout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", workout.Id);
                    command.Parameters.AddWithValue("@Date", workout.Date);
                    command.Parameters.AddWithValue("@Description", workout.Description);
                    command.Parameters.AddWithValue("@UserProfileId", workout.UserProfileId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWorkout(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_DeleteWorkout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
