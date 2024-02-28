using System;
using MySql.Data.MySqlClient;
using Model;
using System.Collections.Generic;

namespace TareaBD.DB
{
    public class Empleado
    {
        public void InsertarEmpleado(string nombre, decimal salario, string connectionString)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "INSERT INTO Empleado (Nombre, Salario) VALUES (@nombre, @salario)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@salario", salario);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<string[]> VerEmpleados(string connectionString)
        {
            List<string[]> empleados = new List<string[]>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT id, Nombre, Salario FROM Empleado";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] empleado = new string[]
                            {
                                reader["id"].ToString(),
                                reader["Nombre"].ToString(),
                                reader["Salario"].ToString()
                            };

                            empleados.Add(empleado);
                        }
                    }
                }
            }

            return empleados;
        }
    }
}
