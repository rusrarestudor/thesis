using GymMonitorAPI.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymMonitorAPI.DAL
{
    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<UserDTO> GetUserProfiles()
        {
            var userProfiles = new List<UserDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetUserProfiles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userProfile = new UserDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Weight = reader.GetDouble(reader.GetOrdinal("Weight"))
                            };
                            userProfiles.Add(userProfile);
                        }
                    }
                }
            }

            return userProfiles;
        }

        public void AddUserProfile(UserDTO userProfile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_AddUserProfile", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Age", userProfile.Age);
                    command.Parameters.AddWithValue("@Weight", userProfile.Weight);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserProfile(UserDTO userProfile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_UpdateUserProfile", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", userProfile.Id);
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Age", userProfile.Age);
                    command.Parameters.AddWithValue("@Weight", userProfile.Weight);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUserProfile(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_DeleteUserProfile", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
