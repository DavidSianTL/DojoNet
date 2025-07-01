use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBAutoExpress') is not null
begin
    alter database DBAutoExpress set single_user with rollback immediate;
    drop database DBAutoExpress;
end
go

-- Creamos la DB
create database DBAutoExpress;
go

-- Usamos la DB
use DBAutoExpress;
go

-- Tabla Usuarios
create table Usuarios(
    idUsuario int primary key identity(1,1),
    nombreCompleto varchar(50),
    usuario varchar(25),
    contrasenia nvarchar(max),
    token nvarchar(max)
);
go

-- Tabla Logs
create table Logs(
    idLog int primary key identity(1,1)
);
go

-- Tabla Bitácora
create table Bitacora(
    idBitacora int primary key identity(1,1)
);
go

-- Tabla Estados
create table Estados(
    idEstado int primary key identity(1,1),
    estado nvarchar(100),
    descripcion nvarchar(100)
);
go

-- Tabla Tipo de Vehículo
create table TipoVehiculo(
    idTipoVehiculo int primary key identity(1,1),
    tipo nvarchar(100),
    descripcion nvarchar(100)
);
go

-- Tabla Carros
create table Vehiculos(
    idVehiculo int primary key identity(1,1),
    marca nvarchar(100),
    modelo nvarchar(100),
    anio int,
    precio decimal(10,2),
    fk_IdTipoVehiculo int,
    fk_IdEstado int,
    constraint fk_Vehiculos_TipoVehiculo
        foreign key (fk_IdTipoVehiculo)
            references TipoVehiculo(idTipoVehiculo),
    constraint fk_Vehiculos_Estado
        foreign key (fk_IdEstado)
            references Estados(idEstado)
);
go

-- Vista para ver los vehículos y el tipo de estos
create procedure sp_ObtenerVehiculos
as
begin
    select v.idVehiculo, v.marca, v.modelo, v.anio, v.precio, v.fk_IdTipoVehiculo, tv.tipo, v.fk_IdEstado, e.estado 
    from Vehiculos v
    inner join TipoVehiculo tv on v.fk_IdTipoVehiculo = tv.idTipoVehiculo
    inner join Estados e on v.fk_IdEstado = e.idEstado
end
go

-- Inserts para Usuarios
insert into Usuarios (nombreCompleto, usuario, contrasenia)
values 
('José Peralta', 'jperalta', 'holamundo');
go

-- Inserts para Estados
insert into Estados (estado, descripcion)
values
('Disponible', 'Vehículo en inventario'),
('Vendido', 'Vehículo vendido'),
('Reservado', 'En proceso de venta'),
('Mantenimiento', 'Revisión o reparación'),
('Retirado', 'Vehículo retirado del inventario');
go

-- Inserts para Tipos de Vehículos
insert into TipoVehiculo (tipo, descripcion)
values
('Sedán', 'Vehículo de turismo de 4 puertas'),
('SUV', 'Vehículo deportivo utilitario'),
('Pickup', 'Vehículo con caja para carga'),
('Hatchback', 'Vehículo con maletera integrada'),
('Convertible', 'Vehículo con techo retráctil');

-- Inserts para Vehículos
insert into Vehiculos (marca, modelo, anio, precio, fk_IdTipoVehiculo, fk_IdEstado)
values
('Toyota', 'Corolla', 2020, 13500.00, 1, 1),
('Honda', 'CR-V', 2022, 24500.00, 2, 2),
('Ford', 'Ranger', 2019, 21000.00, 3, 1),
('Hyundai', 'i20', 2021, 12000.00, 4, 3),
('Mazda', 'MX-5', 2023, 28500.00, 5, 1);
go

-- Usamos la vista
exec sp_ObtenerVehiculos;
go

-- SP para autenticar el Usuario
create procedure sp_AutenticarUsuario
    @Usuario nvarchar(25),
    @Contrasenia nvarchar(max)
as
begin
    select idUsuario, nombreCompleto, usuario, token
    from Usuarios
    where usuario = @Usuario and contrasenia = hashbytes('SHA2_256', @Contrasenia)
end
go

-- SP para obtener el usuario por ID
create procedure sp_ObtenerUsuarioPorId
    @IdUsuario int
as
begin
    select idUsuario, nombreCompleto, usuario
    from Usuarios
    where idUsuario = @IdUsuario
end
go