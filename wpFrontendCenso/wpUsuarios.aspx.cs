using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using Newtonsoft.Json.Linq;

namespace wpFrontendCenso
{
    public partial class wpUsuarios : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Validar que haya sesión activa
            if (Session["usuario"] == null)
            {
                Response.Redirect("wpAcceso.aspx", false); 
                Context.ApplicationInstance.CompleteRequest(); 
                return;
            }

            if (!IsPostBack)
            {
                lblNombre.Text = Session["nombre"].ToString();
                lblTipo.Text = "Rol: " + Session["tipo"].ToString();
                lblUsuario.Text = "Usuario: " + Session["usuario"].ToString();

                if (Session["foto"] != null)
                {
                    imgUsuario.ImageUrl = Session["foto"].ToString();
                }
                else
                {
                    imgUsuario.ImageUrl = "imagenes/default.jpg";
                }
                await CargarTiposUsuario();
                await CargarUsuarios();
                await CargarTiposUsuarioParaEdicion();
            }
        }

        // Funcion para cargar usuarios
        private async Task CargarTiposUsuario()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/vwtipousuario";
                    HttpResponseMessage response = await client.GetAsync(urlApi);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(json);

                        ddlTipo.DataSource = ds.Tables[0];
                        ddlTipo.DataTextField = "descripcion";
                        ddlTipo.DataValueField = "clave";
                        ddlTipo.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al cargar tipos de usuario: " + ex.Message;
            }
        }
        private async Task CargarTiposUsuarioParaEdicion()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/vwtipousuario";
                    HttpResponseMessage response = await client.GetAsync(urlApi);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(json);

                        if (ddlTipoEdit != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ddlTipoEdit.DataSource = ds.Tables[0];
                            ddlTipoEdit.DataTextField = "descripcion";
                            ddlTipoEdit.DataValueField = "clave";
                            ddlTipoEdit.DataBind();

                            ddlTipoEdit.Items.Insert(0, new ListItem("-- Seleccionar Tipo --", ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ddlTipoEdit != null)
                {
                    ddlTipoEdit.Items.Add(new ListItem("Administrador", "1"));
                    ddlTipoEdit.Items.Add(new ListItem("Supervisor", "2"));
                    ddlTipoEdit.Items.Add(new ListItem("Usuario", "3"));
                }

                lblInfo.Text += "<br/>Error cargando tipos: " + ex.Message;
            }
        }

        private async Task CargarTipoUsuarioSeleccionado(string id, GridViewRow row)
        {
            try
            {
                
                if (ddlTipoEdit.Items.Count <= 1) 
                {
                    await CargarTiposUsuarioParaEdicion();
                }

                if (row.Cells.Count > 3)
                {
                    string rolCompleto = row.Cells[3].Text;

                    if (rolCompleto.Contains("-"))
                    {
                        string tipoClave = rolCompleto.Split('-')[0].Trim();

                        ListItem item = ddlTipoEdit.Items.FindByValue(tipoClave);
                        if (item != null)
                        {
                            ddlTipoEdit.SelectedValue = tipoClave;
                            return;
                        }
                    }
                }

                ddlTipoEdit.SelectedValue = "2"; 

            }
            catch (Exception ex)
            {

                lblInfo.Text += "<br/>⚠️ No se pudo cargar el tipo: " + ex.Message;
            }
        }

        private async Task CargarUsuarios()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/vwrptusuario";
                    HttpResponseMessage response = await client.GetAsync(urlApi);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(json);

                        gvUsuarios.DataSource = ds.Tables[0];
                        gvUsuarios.DataBind();

                        lblInfo.Text = "Usuarios cargados correctamente.";
                    }
                    else
                    {
                        lblInfo.Text = "Error al obtener usuarios del servidor.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al conectar con la API: " + ex.Message;
            }
        }

        // Botones cargar y buscar
        protected async void btnCargar_Click(object sender, EventArgs e)
        {
            await CargarUsuarios();
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string filtro = txtFiltro.Text.Trim();
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/vwrptusuariofiltro?nomFiltro=" + filtro;

                    HttpResponseMessage response = await client.GetAsync(urlApi);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(json);

                        gvUsuarios.DataSource = ds.Tables[0];
                        gvUsuarios.DataBind();

                        lblInfo.Text = "Filtro aplicado: " + filtro;
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al conectar con la API: " + ex.Message;
            }
        }

        // Boton para registrar usuario
        protected async void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var nuevoUsuario = new
                    {
                        nombre = txtNombre.Text,
                        apellidoPaterno = txtPaterno.Text,
                        apellidoMaterno = txtMaterno.Text,
                        usuario = txtUsuarioNuevo.Text,
                        contraseña = txtContraseña.Text,
                        ruta = txtRuta.Text,
                        tipo = ddlTipo.SelectedValue
                    };

                    string datos = JsonConvert.SerializeObject(nuevoUsuario);
                    HttpContent contenido = new StringContent(datos, Encoding.UTF8, "application/json");

                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/spinsusuario";
                    HttpResponseMessage response = await client.PostAsync(urlApi, contenido);

                    if (response.IsSuccessStatusCode)
                    {
                        lblInfo.CssClass = "text-success";
                        lblInfo.Text = "Usuario registrado correctamente.";
                        await CargarUsuarios();
                    }
                    else
                    {
                        lblInfo.CssClass = "text-danger";
                        lblInfo.Text = "Error al registrar usuario.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al conectar con la API: " + ex.Message;
            }
        }


        protected async void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string id = e.CommandArgument.ToString();

                    using (HttpClient client = new HttpClient())
                    {
                        string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "full/usuario/delete?id=" + id;
                        HttpResponseMessage response = await client.DeleteAsync(urlApi);

                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();

                            // Parsear el JSON 
                            JObject objRespuesta = JObject.Parse(result);

                            bool statusExec = objRespuesta["statusExec"] != null && (bool)objRespuesta["statusExec"];
                            int ban = objRespuesta["ban"] != null ? (int)objRespuesta["ban"] : 0;
                            string msg = objRespuesta["msg"]?.ToString() ?? "Sin mensaje";

                            if (statusExec && ban == 1)
                            {
                                lblInfo.CssClass = "alert alert-success";
                                lblInfo.Text = "✅ " + msg;
                                await CargarUsuarios(); 
                            }
                            else
                            {
                                lblInfo.CssClass = "alert alert-warning";
                                lblInfo.Text = "⚠️ " + msg;
                            }
                        }
                        else
                        {
                            lblInfo.CssClass = "alert alert-danger";
                            lblInfo.Text = "❌ Error al eliminar usuario: " + response.ReasonPhrase;
                        }
                    }
                }
                else if (e.CommandName == "Editar")
                {
                    string id = e.CommandArgument.ToString();
                    txtIdEdit.Text = id;

                    // Buscar la fila con el ID
                    foreach (GridViewRow row in gvUsuarios.Rows)
                    {
                        if (row.Cells[0].Text == id)
                        {
                            string nombreCompleto = row.Cells[1].Text; 

                            string[] partes = nombreCompleto.Trim().Split(' ');

                            if (partes.Length >= 1)
                                txtNombreEdit.Text = partes[0]; 

                            if (partes.Length >= 2)
                                txtApellidoPaternoEdit.Text = partes[1]; 

                            if (partes.Length >= 3)
                            {
                                txtApellidoMaternoEdit.Text = string.Join(" ", partes, 2, partes.Length - 2); 
                            }

                            // Obtener usuario
                            if (row.Cells.Count > 2)
                                txtUsuarioEdit.Text = row.Cells[2].Text;

                            lblInfo.CssClass = "alert alert-success";
                            lblInfo.Text = "✅ Editando: " + nombreCompleto;

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.CssClass = "alert alert-danger";
                lblInfo.Text = "❌ Error al procesar acción: " + ex.Message;
            }
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDAR que el ID no esté vacío
                if (string.IsNullOrEmpty(txtIdEdit.Text))
                {
                    lblInfo.CssClass = "alert alert-danger";
                    lblInfo.Text = "❌ ERROR: No hay ID de usuario para actualizar. Primero seleccione un usuario para editar.";
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    string urlApi = ConfigurationManager.AppSettings["ApiBaseUrl"] + "api/usuario/update";

                    string debugInfo = $"ID: {txtIdEdit.Text}, Nombre: {txtNombreEdit.Text}, Usuario: {txtUsuarioEdit.Text}";

                    var usuarioActualizado = new
                    {
                        cve = txtIdEdit.Text,
                        nombre = txtNombreEdit.Text,
                        apellidoPaterno = txtApellidoPaternoEdit.Text ?? "",
                        apellidoMaterno = txtApellidoMaternoEdit.Text ?? "",
                        usuario = txtUsuarioEdit.Text,
                        contraseña = txtContraseñaEdit.Text ?? "",
                        ruta = "imagenes/fotos/" + txtIdEdit.Text + ".jpg",
                        tipo = ddlTipoEdit.SelectedValue ?? "2" 
                    };

                    string datos = JsonConvert.SerializeObject(usuarioActualizado);

                    HttpContent contenido = new StringContent(datos, Encoding.UTF8, "application/json");

                    HttpResponseMessage respuesta = await client.PutAsync(urlApi, contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string result = await respuesta.Content.ReadAsStringAsync();
                        lblInfo.Text = "Respuesta: " + result;

                        JObject json = JObject.Parse(result);
                        bool statusExec = json["statusExec"] != null && (bool)json["statusExec"];
                        int ban = json["ban"] != null ? (int)json["ban"] : 0;

                        if (statusExec && ban == 1)
                        {
                            lblInfo.CssClass = "alert alert-success";
                            lblInfo.Text = "✅ " + (json["msg"]?.ToString() ?? "Usuario actualizado correctamente.");
                            await CargarUsuarios();
                        }
                        else
                        {
                            lblInfo.CssClass = "alert alert-warning";
                            lblInfo.Text = "⚠️ " + (json["msg"]?.ToString() ?? "No se pudo actualizar el usuario.");
                        }
                    }
                    else
                    {
                        string errorContent = await respuesta.Content.ReadAsStringAsync();
                        lblInfo.CssClass = "alert alert-danger";
                        lblInfo.Text = "❌ Error HTTP: " + (int)respuesta.StatusCode + " - " + respuesta.ReasonPhrase +
                                      "<br/>Detalles: " + errorContent;
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.CssClass = "alert alert-danger";
                lblInfo.Text = "❌ Error: " + ex.Message;
            }
        }

        protected void btnCancelarEdit_Click(object sender, EventArgs e)
        {
            LimpiarCamposEdicion();
            lblInfo.CssClass = "alert alert-info";
            lblInfo.Text = "Edición cancelada.";
        }

        private void LimpiarCamposEdicion()
        {
            txtIdEdit.Text = "";
            txtNombreEdit.Text = "";
            txtApellidoPaternoEdit.Text = "";
            txtApellidoMaternoEdit.Text = "";
            txtUsuarioEdit.Text = "";
            txtContraseñaEdit.Text = "";

            if (ddlTipoEdit != null && ddlTipoEdit.Items.Count > 0)
            {
                ddlTipoEdit.SelectedIndex = 0;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("wpAcceso.aspx", false); 
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}