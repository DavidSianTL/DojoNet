using Microsoft.Identity.Client;
using ProyectoDojoGeko.Dtos.Solicitudes;
using ProyectoDojoGeko.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Helper.Solicitudes
{
	public interface ISolicitudConverter
	{
		public List<SolicitudEncabezadoViewModel> ConverListResultToViewModel(List<SolicitudEncabezadoResult> solicitudesRList);
	}
	public class SolicitudConverter : ISolicitudConverter
	{
		public List<SolicitudEncabezadoViewModel> ConverListResultToViewModel(List<SolicitudEncabezadoResult> solicitudesRList)
		{
			var solicitudesVMList = new List<SolicitudEncabezadoViewModel>();

			foreach (var solicitudR in solicitudesRList)
			{
                var solicitudVM = new SolicitudEncabezadoViewModel()
                {
                    IdSolicitud = solicitudR.IdSolicitud,
                    IdEmpleado = solicitudR.IdEmpleado,
                    NombreEmpleado = solicitudR.NombreEmpleado,
                    DiasSolicitadosTotal = solicitudR.DiasSolicitadosTotal,
                    FechaIngresoSolicitud = solicitudR.FechaIngresoSolicitud,
                    SolicitudLider = solicitudR.SolicitudLider,
                    Observaciones = solicitudR.Observaciones,
                    Estado = solicitudR.Estado,
                    IdAutorizador = solicitudR.IdAutorizador,
                    FechaAutorizacion = solicitudR.FechaAutorizacion,
                    MotivoRechazo = solicitudR.MotivoRechazo
                };

                solicitudesVMList.Add(solicitudVM);
            }

            return solicitudesVMList;

		}
	}
}
