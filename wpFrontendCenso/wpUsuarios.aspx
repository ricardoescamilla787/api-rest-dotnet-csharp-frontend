<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="wpUsuarios.aspx.cs" Inherits="wpFrontendCenso.wpUsuarios" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Panel de Usuarios</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/site.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container mt-5">
    <div class="card shadow p-4">

        <!-- ENCABEZADO DE SESIÓN -->
        <div class="row align-items-center mb-3">
            <div class="col-md-2">
                <asp:Image ID="imgUsuario" runat="server" CssClass="img-thumbnail rounded-circle" Width="100px" Height="100px" />
            </div>
            <div class="col-md-10">
                <h4><asp:Label ID="lblNombre" runat="server" CssClass="fw-bold"></asp:Label></h4>
                <p class="text-muted"><asp:Label ID="lblTipo" runat="server"></asp:Label></p>
                <p><asp:Label ID="lblUsuario" runat="server" CssClass="text-secondary"></asp:Label></p>
                <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar sesión" CssClass="btn btn-danger btn-sm" OnClick="btnCerrarSesion_Click" />
            </div>
        </div>

        <hr />

        <!-- CRUD DE USUARIOS -->
        <h5>Gestión de Usuarios</h5>
        <div class="row mb-3">
            <div class="col-md-4">
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Buscar usuario..." />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnCargar" runat="server" Text="Ver todos" CssClass="btn btn-secondary w-100" OnClick="btnCargar_Click" />
            </div>
        </div>

        <asp:Label ID="lblInfo" runat="server" CssClass="text-info"></asp:Label>
        <asp:Label ID="Label1" runat="server" CssClass="alert d-block mt-3"></asp:Label>

        <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-striped mt-3"
            AutoGenerateColumns="False" DataKeyNames="Clave"
            OnRowCommand="gvUsuarios_RowCommand">
            <Columns>

                <asp:BoundField DataField="Clave" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                <asp:BoundField DataField="Rol" HeaderText="Rol" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning me-1"
                            CommandName="Editar" CommandArgument='<%# Eval("Clave") %>' />
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"
                            CommandName="Eliminar" CommandArgument='<%# Eval("Clave") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <!-- EDICION -->
<div class="card mt-4">
    <div class="card-body">
        <h5 class="card-title">Editar usuario</h5>
        <div class="row">
            <!-- Fila 1 -->
            <div class="col-md-3">
                <label>ID:</label>
                <asp:TextBox ID="txtIdEdit" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>
            <div class="col-md-3">
                <label>Nombre:</label>
                <asp:TextBox ID="txtNombreEdit" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-3">
                <label>Apellido Paterno:</label>
                <asp:TextBox ID="txtApellidoPaternoEdit" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-3">
                <label>Apellido Materno:</label>
                <asp:TextBox ID="txtApellidoMaternoEdit" runat="server" CssClass="form-control" />
            </div>
        </div>
        
        <div class="row mt-2">
            <!-- Fila 2 -->
            <div class="col-md-4">
                <label>Usuario:</label>
                <asp:TextBox ID="txtUsuarioEdit" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Contraseña (dejar vacío para no cambiar):</label>
                <asp:TextBox ID="txtContraseñaEdit" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
            <div class="col-md-4">
                <label>Tipo de Usuario:</label>
                <asp:DropDownList ID="ddlTipoEdit" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>
        
        <div class="row mt-3">
            <div class="col-md-12">
                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" 
                    CssClass="btn btn-success me-2" OnClick="btnActualizar_Click" />
                <asp:Button ID="btnCancelarEdit" runat="server" Text="Cancelar" 
                    CssClass="btn btn-secondary" OnClick="btnCancelarEdit_Click" />
            </div>
        </div>
    </div>
</div>


        <hr />

        <h5>Registrar nuevo usuario</h5>
        <div class="row">
            <div class="col-md-4">
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control mb-2" placeholder="Nombre" />
                <asp:TextBox ID="txtPaterno" runat="server" CssClass="form-control mb-2" placeholder="Apellido paterno" />
                <asp:TextBox ID="txtMaterno" runat="server" CssClass="form-control mb-2" placeholder="Apellido materno" />
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtUsuarioNuevo" runat="server" CssClass="form-control mb-2" placeholder="Usuario" />
                <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control mb-2" placeholder="Contraseña" />
                <asp:TextBox ID="txtRuta" runat="server" CssClass="form-control mb-2" placeholder="Ruta de foto" />
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select mb-2"></asp:DropDownList>
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn btn-success w-100" OnClick="btnRegistrar_Click" />
            </div>
        </div>

    </div>
</div>
        <script src="bootstrap/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
