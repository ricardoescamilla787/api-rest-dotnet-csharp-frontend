using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using wpFrontendCenso.Models;
using System.Configuration;

namespace wpFrontendCenso
{
    public partial class wpAcceso : System.Web.UI.Page
    {
        protected async void Button1_Click(object sender, EventArgs e)
        {
            await cargaDatosApi();
        }

        private async Task cargaDatosApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string datos = @"{
                ""usuario"":""" + TextBox1.Text + @""",
                ""contraseña"":""" + TextBox2.Text + @"""
            }";

                    HttpContent contenido = new StringContent(datos, Encoding.UTF8, "application/json");
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/spvalidaracceso";

                    HttpResponseMessage respuesta = await client.PostAsync(urlApi, contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string resultado = await respuesta.Content.ReadAsStringAsync();
                        var json = Newtonsoft.Json.Linq.JObject.Parse(resultado);

                        // Verificamos si la respuesta contiene datos
                        if (json["spValidarAcceso"] != null && json["spValidarAcceso"].HasValues)
                        {
                            string valor = json["spValidarAcceso"][0]["1"]?.ToString() ?? "0";

                            if (valor == "1")
                            {
                                // Usuario válido
                                string nombre = json["spValidarAcceso"][0]["usu_nombre_completo"]?.ToString();
                                string tipo = json["spValidarAcceso"][0]["tip_descripcion"]?.ToString();
                                string foto = json["spValidarAcceso"][0]["usu_ruta"]?.ToString();

                                // Guardar en sesión
                                Session["usuario"] = TextBox1.Text;
                                Session["nombre"] = nombre;
                                Session["tipo"] = tipo;
                                Session["foto"] = foto;
                                lblResultado.CssClass = "text-success";
                                lblResultado.Text = $"Bienvenido {nombre} ({tipo})";
                                await Task.Delay(500);
                                Response.Redirect("wpUsuarios.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                lblResultado.CssClass = "text-danger";
                                lblResultado.Text = "Acceso denegado, verifique usuario o contraseña.";
                            }
                        }
                        else
                        {
                            lblResultado.CssClass = "text-danger";
                            lblResultado.Text = "Error: respuesta vacía del servidor.";
                        }
                    }
                    else
                    {
                        lblResultado.CssClass = "text-danger";
                        lblResultado.Text = "Error en la respuesta del servidor.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblResultado.CssClass = "text-danger";
                lblResultado.Text = "Error al conectar con la API: " + ex.Message;
            }
        }


    }
}
