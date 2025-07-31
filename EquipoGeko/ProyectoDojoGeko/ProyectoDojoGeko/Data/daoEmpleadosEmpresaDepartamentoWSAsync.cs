using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Empleados;

namespace ProyectoDojoGeko.Data
{
    public class daoEmpleadosEmpresaDepartamentoWSAsync
    {
        private readonly string _connectionString;

        public daoEmpleadosEmpresaDepartamentoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los empleados de una empresa
        public async Task<List<EmpleadosEmpresaViewModel>> ObtenerEmpleadosEmpresaAsync()
        {
            var empleados = new List<EmpleadosEmpresaViewModel>();
            string procedure = "sp_ObtenerEmpleadosEmpresa";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        // Obtener un empleado de una empresa por su ID
        public async Task<EmpleadosEmpresaViewModel> ObtenerEmpleadoEmpresaPorIdAsync(int idEmpleadoEmpresa)
        {
            string procedure = "sp_ObtenerEmpleadoEmpresaPorId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEmpleadoEmpresa", idEmpleadoEmpresa);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Obtener empleados de una empresa por ID del empleado
        public async Task<EmpleadosEmpresaViewModel> ObtenerEmpleadoPorIdAsync(int idEmpleado)
        {
            string procedure = "sp_ObtenerEmpleadoEmpresaPorIdEmpleado";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FK_IdEmpleado", idEmpleado);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Obtener empleados de un departamento
        public async Task<List<EmpleadosDepartamentoViewModel>> ObtenerEmpleadosDepartamentoAsync()
        {
            var empleados = new List<EmpleadosDepartamentoViewModel>();
            string procedure = "sp_ListarEmpleadosDepartamento";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosDepartamentoViewModel
                            {
                                IdEmpleadosDepartamento = reader.GetInt32(reader.GetOrdinal("IdEmpleadosDepartamento")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento"))
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        // Obtener empleado de un departamento por ID
        public async Task<List<EmpleadosDepartamentoViewModel>> ObtenerEmpleadoDepartamentoPorIdAsync(int idEmpleadoDepartamento)
        {
            var empleados = new List<EmpleadosDepartamentoViewModel>();
            string procedure = "sp_ObtenerEmpleadoDepartamentoPorId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEmpleadosDepartamento", idEmpleadoDepartamento);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosDepartamentoViewModel
                            {
                                IdEmpleadosDepartamento = reader.GetInt32(reader.GetOrdinal("IdEmpleadoDepartamento")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento"))
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        // Insertar empleado en empresa
        public async Task<int> InsertarEmpleadoEmpresaAsync(EmpleadosEmpresaViewModel empleado)
        {
            string procedure = "sp_InsertarEmpleadosEmpresa";
            var parametros = new[]
            {
                new SqlParameter("@FK_IdEmpresa", empleado.FK_IdEmpresa),
                new SqlParameter("@FK_IdEmpleado", empleado.FK_IdEmpleado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Insertar empleado en departamento
        public async Task<int> InsertarEmpleadoDepartamentoAsync(EmpleadosDepartamentoViewModel empleadoDepartamento)
        {
            string procedure = "sp_InsertarEmpleadosDepartamento";
            var parametros = new[]
            {
                new SqlParameter("@FK_IdEmpleado", empleadoDepartamento.FK_IdEmpleado),
                new SqlParameter("@FK_IdDepartamento", empleadoDepartamento.FK_IdDepartamento)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar asignación de empleado a departamento
        public async Task<int> ActualizarEmpleadoDepartamentoAsync(EmpleadosDepartamentoViewModel empleadoDepartamento)
        {
            string procedure = "sp_ActualizarEmpleadoDepartamento";
            var parametros = new[]
            {
                new SqlParameter("@IdEmpleadosDepartamento", empleadoDepartamento.IdEmpleadosDepartamento),
                new SqlParameter("@FK_IdEmpleado", empleadoDepartamento.FK_IdEmpleado),
                new SqlParameter("@FK_IdDepartamento", empleadoDepartamento.FK_IdDepartamento)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Eliminar asignación de empleado a departamento
        public async Task<int> EliminarEmpleadoDepartamentoAsync(int idEmpleadoDepartamento)
        {
            string procedure = "sp_EliminarEmpleadoDepartamento";
            var parametros = new[]
            {
                new SqlParameter("@IdEmpleadosDepartamento", idEmpleadoDepartamento)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}