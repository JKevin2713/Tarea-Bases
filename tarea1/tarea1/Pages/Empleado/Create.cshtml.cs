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
            string auxNombre = Request.Form["nombre"];
            string auxSalario = Request.Form["salario"];
            int resultCode = 0;

            if(ValidarNomSal(auxNombre, auxSalario) == false)
            {
                message = "Error en los datos, revise los datos ingresados";
                return;
            }

            info.Salario = decimal.Parse(auxSalario);
            info.Nombre = auxNombre;

            try
            {
                string connectionString = "server=tarea1.database.windows.net;user=Kevin;" +
                                          "database=PruebasTarea;password=Jk123456";
           
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using(SqlCommand command = new SqlCommand("registroEmpleado", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        command.Parameters.AddWithValue("@nombre", info.Nombre);
                        command.Parameters.AddWithValue("@salario", info.Salario);
                        

                        // Parámetro de salida
                        command.Parameters.Add("@OutResulTCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        resultCode = Convert.ToInt32(command.Parameters["@OutResulTCode"].Value);
                        Console.WriteLine("Código de resultado: " + resultCode);
                    }
                    sqlConnection.Close();
                }
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return;
            }

            if(resultCode == 50006)
            {
                message = "Error, el empleado que desea agregar ya existe";
            }
            else
            {
                flag = true;
                message = "Se a creado correctamente el empleado";
            }
            /*
            flag = true;
            info.Nombre = "";
            info.Salario = 0;
            salario = "";
            message = "Se a creado correctamente el empleado";
            Response.Redirect("/Empleado/Index");
            */
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

