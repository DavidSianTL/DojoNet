use master;
go
	
drop database if exists DBTiendaElectronica;
go

create database DBTiendaElectronica;
go

use DBTiendaElectronica;
go

create table Usuarios(
	idUsuario int identity(1,1),
	usuario nvarchar(50) not null,
	contrasenia nvarchar(255) not null,
	fechaRegistro datetime default getdate(),
	primary key (idUsuario)
);

create table Proveedores(
	idProveedor int identity(1,1),
	nombre nvarchar(100) not null,
	telefono nvarchar(20),
    direccion nvarchar(200),
	primary key (idProveedor)
);

create table Categorias(
	idCategoria int identity(1,1),
	nombre nvarchar(100) not null,
	primary key (idCategoria)
);

create table Productos(
	idProducto int identity(1,1),
	nombre nvarchar(50) not null,
	descripcion nvarchar(150),
	stock int not null default 0,
	precio decimal(10,2) not null,
	idCategoria int not null,
	idProveedor int not null,
	primary key (idProducto),
	foreign key (idCategoria) references Categorias(idCategoria),
    foreign key (idProveedor) references Proveedores(idProveedor)
);

create table EncabezadoVenta(
    idVenta int identity(1,1),
    fechaVenta datetime default getdate(),
    total decimal(10,2) not null,
	idUsuario int not null,
    primary key (idVenta),
    foreign key (idUsuario) references Usuarios(idUsuario)
);

create table DetalleVenta(
    idDetalle int identity(1,1),
    cantidad int not null,
    precioUnitario decimal(10,2) not null,
    subtotal decimal(10,2) not null,
	idVenta int not null,
    idProducto int not null,
    primary key (idDetalle),
    foreign key (idVenta) references EncabezadoVenta(idVenta),
    foreign key (idProducto) references Productos(idProducto)
);

INSERT INTO Usuarios (usuario, contrasenia) 
VALUES 
('anaperez', 'clave123'),
('carlosl', 'abc456'),
('mariag', 'mypass789');

INSERT INTO Proveedores (nombre, telefono, direccion)
VALUES
('ElectroParts S.A.', '555-100-2000', 'Av. Tecnológica 123'),
('Componentes Digitales', '555-300-4000', 'Calle Circuito 456'),
('TecnoSuministros', '555-500-6000', 'Boulevard Innovación 789');

INSERT INTO Categorias (nombre)
VALUES
('Teclados'),
('Monitores'),
('Audio');

INSERT INTO Productos (nombre, descripcion, precio, stock, idCategoria, idProveedor)
VALUES
('Teclado Mecánico X9', 'Teclado gaming con switches azules', 899.99, 15, 1, 1),
('Monitor Curvo 27"', 'Resolución 4K, 144Hz', 5499.00, 8, 2, 2),
('Audífonos Inalámbricos Pro', 'Cancelación de ruido, 30h batería', 1299.50, 4, 3, 3),
('Teclado Inalámbrico Slim', 'Diseño delgado para oficina', 499.99, 20, 1, 2),
('Monitor Oficina 24"', 'Full HD, panel IPS', 2199.00, 12, 2, 1),
('Bocinas Bluetooth', 'Potencia 20W, resistentes al agua', 799.00, 7, 3, 3);

INSERT INTO EncabezadoVenta (idUsuario, fechaVenta, total)
VALUES
(1, '2024-05-01 10:30:00', 1799.98),
(2, '2024-05-02 15:45:00', 5499.00),
(3, '2024-05-03 12:20:00', 2098.50);

INSERT INTO DetalleVenta (idVenta, idProducto, cantidad, precioUnitario, subtotal)
VALUES
(1, 1, 1, 899.99, 899.99),  -- Teclado Mecánico X9
(1, 6, 1, 799.00, 799.00),   -- Bocinas Bluetooth
(2, 2, 1, 5499.00, 5499.00), -- Monitor Curvo 27"
(3, 3, 1, 1299.50, 1299.50),  -- Audífonos Inalámbricos
(3, 5, 1, 799.00, 799.00);     -- Monitor Oficina 24" 

-- Listar por tabla
select * from Usuarios;
select * from Proveedores;
select * from Productos;
select * from EncabezadoVenta;
select * from DetalleVenta;

-- Listar todos los productos con su categoría y proveedor.
select p.idProducto, p.nombre, p.descripcion, p.precio, p.stock, c.nombre, pr.nombre, pr.telefono 
	from Productos p 
	join Categorias c on p.idCategoria = c.idCategoria 
	join Proveedores pr on p.idProveedor = pr.idProveedor;

-- Mostrar el historial de ventas con nombre del usuario y total vendido.
select ev.idVenta, u.usuario, ev.fechaVenta, count(dv.idDetalle) as 'Consumo', p.nombre, ev.total 
	from EncabezadoVenta ev 
	join Usuarios u on ev.idUsuario = u.idUsuario 
	join DetalleVenta dv on ev.idVenta = dv.idVenta
	join Productos p on dv.idProducto = p.idProducto 
	group by ev.idVenta, u.usuario, ev.fechaVenta, p.nombre, ev.total order by ev.fechaVenta desc;

-- Obtener los productos con menos de 5 unidades en stock.
select * from Productos p where p.stock <= 5;

-- Calcular el total vendido por cada producto.
select pr.nombre, sum(dv.precioUnitario * dv.cantidad) as totalVendido 
	from DetalleVenta dv 
	join Productos pr on dv.idProducto = pr.idProducto group by dv.idProducto, pr.nombre;
go

-- Vista
create view Vista_VentasResumen as
	select u.usuario, ev.fechaVenta, dv.cantidad, p.nombre, dv.subtotal from Usuarios u
		join EncabezadoVenta ev on u.idUsuario = ev.idUsuario
		join DetalleVenta dv on ev.idVenta = dv.idVenta
		join Productos p on dv.idProducto = p.idProducto;
go

select * from Vista_VentasResumen;
go

-- Procedimiento almacenado
create procedure sp_RegistrarVentaSimple
    @idUsuario int,
    @idProducto int,
    @Cantidad int
as
begin
    set nocount on;
    
    declare @PrecioUnitario decimal(10,2);
    declare @Subtotal decimal(10,2);
    declare @idVenta int;
    
    -- Obtener el precio del producto
    select @PrecioUnitario = precio 
    from Productos 
    where idProducto = @idProducto;
    
    -- Calcular subtotal
    set @Subtotal = @Cantidad * @PrecioUnitario;
    
    -- 1. Insertar encabezado de venta
    insert into EncabezadoVenta (idUsuario, fechaVenta, total)
    values (@idUsuario, getdate(), @Subtotal);
    
    set @idVenta = SCOPE_IDENTITY();
    
    -- 2. Insertar detalle de venta
    insert into DetalleVenta (idVenta, idProducto, cantidad, precioUnitario, subtotal)
    values (@idVenta, @idProducto, @Cantidad, @PrecioUnitario, @Subtotal);
    
    -- 3. Actualizar stock
    update Productos
    set stock = stock - @Cantidad
    where idProducto = @idProducto;
    
    select 'Venta registrada correctamente' as Mensaje, @idVenta as NumeroVenta;
end;
	
go

exec sp_RegistrarVentaSimple 1, 1, 2;

select * from EncabezadoVenta;
select * from DetalleVenta;
select * from Productos;