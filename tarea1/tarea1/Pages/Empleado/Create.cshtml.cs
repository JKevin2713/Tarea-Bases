using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tarea1.Pages.Empleado
{
    public class CreateModel : PageModel
    {
        public infoEmpleyee info = new infoEmpleyee();
        public string message = "";
        public bool flag = false;
        public void OnGet()
        {
        }

        public void OnPost()
        {
            info.Nombre = Request.Form["nombre"];
            string salario = Request.Form["salario"];
            info.Salario = decimal.Parse(salario);

            if(info.Nombre.Length == 0 || salario.Length == 0)
            {
                message = "Se necesita llenar todos los campos";
                return;
            }
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1" +
                                          ";Integrated Security=True;Encrypt=False";

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("registroEmpleado", sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@nombre", info.Nombre);
                command.Parameters.AddWithValue("@salario", info.Salario);
                command.ExecuteNonQuery();
                /*
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    /*string sqlInfo = "INSERT INTO Empleado" +
                                    "(Nombre, Salario) VALUES" +
                                    "(@nombre, @salario);";
                    using (SqlCommand command = new SqlCommand(sqlInfo, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.Parameters.AddWithValue("@salario", info.Salario);

                        command.ExecuteNonQuery();
                    }
                   
                }
            */
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return;
            }



            flag = true;
            info.Nombre = "";
            info.Salario = 0;
            salario = "";
            message = "Se a creado correctamente el empleado";

            Response.Redirect("/Empleado/Index");
        }
    }
}
