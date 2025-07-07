<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestConexion.aspx.cs" Inherits="AutoExpres.TestConexion" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Test de Conexión</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Prueba de Conexión a Base de Datos</h2>
            <asp:Button ID="btnProbar" runat="server" Text="Probar Conexión" OnClick="btnProbar_Click" />
            <br /><br />
            <asp:Label ID="lblResultado" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </form>
</body>
</html>
