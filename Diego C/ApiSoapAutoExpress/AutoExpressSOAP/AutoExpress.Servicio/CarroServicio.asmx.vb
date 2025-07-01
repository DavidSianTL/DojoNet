Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports AutoExpress.Entidades
Imports AutoExpress.Datos
Imports AutoExpress.Negocio
Imports System.Configuration

<System.Web.Services.WebService(Namespace:="http://autoexpress.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class CarroServicio
    Inherits System.Web.Services.WebService

    Private ReadOnly _carroService As CarroService
    Private ReadOnly _marcaDAO As MarcaDAO
    Private ReadOnly _modeloDAO As ModeloDAO

    Public Sub New()
        Dim connString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim carroDao As New CarroDAO(connString)
        Dim marcaDao As New MarcaDAO(connString)
        Dim modeloDao As New ModeloDAO(connString)

        _carroService = New CarroService(carroDao)
        _marcaDAO = marcaDao
        _modeloDAO = modeloDao
    End Sub

    <WebMethod()>
    Public Function ListarVehiculos() As List(Of Carro)
        Return _carroService.ObtenerCarros()
    End Function

    <WebMethod()>
    Public Sub AgregarVehiculo(carro As Carro)
        _carroService.AgregarCarro(carro)
    End Sub

    <WebMethod()>
    Public Sub EditarVehiculo(carro As Carro)
        _carroService.EditarCarro(carro)
    End Sub

    <WebMethod()>
    Public Sub EliminarVehiculo(id As Integer)
        _carroService.EliminarCarro(id)
    End Sub

    ' NUEVOS MÉTODOS PARA MARCA

    <WebMethod()>
    Public Function AgregarMarca(nombre As String) As Integer
        Dim existente = _marcaDAO.ObtenerPorNombre(nombre)
        If existente IsNot Nothing Then
            Return existente.Id
        Else
            Return _marcaDAO.Agregar(New Marca With {.Nombre = nombre})
        End If
    End Function

    ' NUEVOS MÉTODOS PARA MODELO

    <WebMethod()>
    Public Function AgregarModelo(nombre As String, marcaId As Integer) As Integer
        Dim existente = _modeloDAO.ObtenerPorNombreYMarca(nombre, marcaId)
        If existente IsNot Nothing Then
            Return existente.Id
        Else
            Return _modeloDAO.Agregar(New Modelo With {.Nombre = nombre, .MarcaId = marcaId})
        End If
    End Function

End Class
