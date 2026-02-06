<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="wpAcceso.aspx.cs" Inherits="wpFrontendCenso.wpAcceso" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Acceso</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/site.css" rel="stylesheet" />
</head>

<body class="bg-light">
    <form id="form1" runat="server" class="container mt-5 p-5 border rounded bg-white shadow">
        <div class="text-center">
            <img src="imagenes/instagram.png" alt="logo" width="100" class="rounded-circle mb-3" />
            <h3 class="mb-4">Inicio de Sesión</h3>
        </div>
        <div class="form-group">
            <label>Usuario:</label>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label>Contraseña:</label>
            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        <asp:Button ID="Button1" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100 mt-3" OnClick="Button1_Click" />
        <asp:Label ID="lblResultado" runat="server" CssClass="text-danger mt-3 d-block text-center"></asp:Label>
        <!-- Bootstrap -->
<script src="bootstrap/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>

