using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace tarea1.Pages.Empleado
{
    // Clase de modelo de la página Index
    public class IndexModel : PageModel
    {
        // Lista para almacenar la información de los empleados
        public List<infoEmpleyee> listEmployee = new List<infoEmpleyee>();

        // Método ejecutado al recibir una solicitud GET
        public void OnGet()
        {
            try
            {
      
                // Cadena de conexión a la base de datos
                string connectionString = "server=tarea1.database.windows.net;user=Kevin;" +
                                          "database=TareaBases;password=Jk123456";

                // Establecer una conexión con la base de datos utilizando la cadena de conexión
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open(); // Abrir la conexión

                    // Crear un comando SQL para llamar al procedimiento almacenado "tablaEmpleado"
                    using (SqlCommand command = new SqlCommand("tablaEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Especificar que el comando es un procedimiento almacenado
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output; // Parámetro de salida

                        // Ejecutar el comando sin obtener resultados directos, ya que vamos a leer datos después
                        command.ExecuteNonQuery();

                        // Leer los datos devueltos por el procedimiento almacenado
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Iterar sobre cada fila devuelta por el lector de datos
                            while (reader.Read())
                            {
                                // Crear un objeto infoEmpleyee para almacenar la información del empleado
                                infoEmpleyee info = new infoEmpleyee();

                                // Obtener los valores de las columnas de la fila actual y asignarlos al objeto infoEmpleyee
                                info.id = reader.GetInt32(0);
                                info.Nombre = reader.GetString(1);
                                info.Salario = reader.GetDecimal(2);

                                // Agregar el objeto infoEmpleyee a la lista de empleados
                                listEmployee.Add(info);
                                Console.Write(info); // Escribir información del empleado en la consola (opcional)
                            }
                        }
                    }
                    sqlConnection.Close(); // Cerrar la conexión después de haber terminado de trabajar con ella
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Manejar cualquier excepción que pueda ocurrir e imprimir el mensaje en la consola
            }
        }
    }

    // Clase para representar la información de un empleado
    public class infoEmpleyee
    {
        public Int32 id; // Identificador del empleado
        public string Nombre; // Nombre del empleado
        public decimal Salario; // Salario del empleado
    }
}

