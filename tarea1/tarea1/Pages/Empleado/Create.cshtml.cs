using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
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
            string AuxNombre = Request.Form["nombre"];
            string Auxsalario = Request.Form["salario"];

            if(ValidarNomSal(AuxNombre, Auxsalario) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                return;
            }

            info.Salario = decimal.Parse(Auxsalario);
            info.Nombre = AuxNombre;
            /*
            if (info.Nombre.Length == 0 || salario.Length == 0)
            {
                message = "Se necesita llenar todos los campos";
                return;
            }
            */
            try
            {
                string connectionString = "Data Source=LAPTOP-K8CP12F2;Initial Catalog=tarea1" +
                                          ";Integrated Security=True;Encrypt=False";
           
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using(SqlCommand command = new SqlCommand("registroEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.Parameters.AddWithValue("@salario", info.Salario);

                        command.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }

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
            //info.Salario = 0;
            //salario = "";
            message = "Se a creado correctamente el empleado";

            Response.Redirect("/Empleado/Index");
        }

       public bool ValidarNomSal(string nombre, string salario)
       {
            if (nombre.Length != 0 || salario.Length != 0)
            {
                if(Regex.IsMatch(nombre, @"^[a-zA-Z\s]+$") && Regex.IsMatch(salario, @"^[0-9]+$"))
                {
                    decimal AuxSalario = decimal.Parse(salario);
                    if (AuxSalario > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}

