using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;

namespace tarea1.Pages.Empleado
{
    public class IndexModel : PageModel
    {
        public List<infoEmpleyee> listEmployee = new List<infoEmpleyee>();
        public void OnGet()
        {
            try
            {
                string connectionString = "server=tarea1.database.windows.net;user=Kevin" +
                                          ";database=PruebasTarea;password=Jk123456";

                using (SqlConnection  sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand("tablaEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                infoEmpleyee info = new infoEmpleyee();
                                info.id = reader.GetInt32(0);
                                info.Nombre = reader.GetString(1);
                                info.Salario = reader.GetDecimal(2);

                                listEmployee.Add(info);
                                Console.Write(info);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class infoEmpleyee
    {
        public Int32 id;
        public string Nombre;
        public decimal Salario;

    }
}
