using GymMonitorAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymMonitorAPI.DAL
{
    public class GymMachineDAL
    {
        private readonly string _connectionString;

        public GymMachineDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<GymMachineDTO> GetGymMachines()
        {
            var gymMachines = new List<GymMachineDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetGymMachines", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var gymMachine = new GymMachineDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Usage = reader.GetString(reader.GetOrdinal("Usage"))
                            };
                            gymMachines.Add(gymMachine);
                        }
                    }
                }
            }

            return gymMachines;
        }

        public void AddGymMachine(GymMachineDTO gymMachine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_AddGymMachine", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", gymMachine.Name);
                    command.Parameters.AddWithValue("@Description", gymMachine.Description);
                    command.Parameters.AddWithValue("@Usage", gymMachine.Usage);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateGymMachine(GymMachineDTO gymMachine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_UpdateGymMachine", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", gymMachine.Id);
                    command.Parameters.AddWithValue("@Name", gymMachine.Name);
                    command.Parameters.AddWithValue("@Description", gymMachine.Description);
                    command.Parameters.AddWithValue("@Usage", gymMachine.Usage);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGymMachine(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_DeleteGymMachine", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
