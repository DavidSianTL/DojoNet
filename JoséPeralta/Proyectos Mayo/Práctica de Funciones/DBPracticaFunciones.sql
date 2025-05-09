use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBPracticaFunciones') is not null
begin
    alter database DBPracticaFunciones set single_user with rollback immediate;
    drop database DBPracticaFunciones;
end
go

-- Crear la base de datos
create database DBPracticaFunciones;
go

-- Usar la base de datos reci�n creada
use DBPracticaFunciones;
go

-- Crear tabla de empleados
CREATE TABLE Empleados (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    FechaIngreso DATE,
    Salario DECIMAL(10,2),
    Departamento VARCHAR(30)
);

-- Crear la tabla Productos
CREATE TABLE Productos (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    Precio DECIMAL(10,2),
    Categoria VARCHAR(30)
);

-- Insertar datos de ejemplo
INSERT INTO Empleados (ID, Nombre, FechaIngreso, Salario, Departamento)
VALUES 
(1, 'Ana', '2020-03-01', 1500, 'Ventas'),
(2, 'Luis', '2019-06-15', 1800, 'TI'),
(3, 'Marta', '2022-01-20', 2000, 'Ventas'),
(4, 'Carlos', '2021-09-10', 1700, 'Marketing'),
(5, 'Elena', '2018-02-25', 2100, 'TI');

-- Insertar datos de ejemplo
INSERT INTO Productos (ID, Nombre, Precio, Categoria)
VALUES
(1, 'Laptop Lenovo', 1200.00, 'Tecnolog�a'),
(2, 'Smartphone Samsung', 900.00, 'Tecnolog�a'),
(3, 'Teclado Logitech', 60.00, 'Tecnolog�a'),
(4, 'Cafetera Oster', 85.00, 'Electrodom�sticos'),
(5, 'Refrigeradora LG', 650.00, 'Electrodom�sticos'),
(6, 'Batidora Philips', 85.00, 'Electrodom�sticos'),
(7, 'Mouse HP', 25.00, 'Tecnolog�a'),
(8, 'Monitor Dell', 250.00, 'Tecnolog�a'),
(9, 'Aspiradora Samsung', 300.00, 'Electrodom�sticos'),
(10, 'Tablet Huawei', 350.00, 'Tecnolog�a');
go

select * from Empleados;
select * from Productos;

-- 1: Funciones Matem�ticas Escalares
-- a) Mostrar el valor absoluto del precio de cada producto.
select abs(p.precio) as ValorAbsoluto from Productos p;
-- b) Redondear el precio hacia arriba y hacia abajo.
select ceiling(p.precio) as RedondeadoHaciaArriba, floor(p.precio) as RedondeadoHaciaAbajo from Productos p;
-- c) Calcular la ra�z cuadrada y el cuadrado del precio.
select sqrt(p.precio) as RaizCuadrada, power(p.precio, 2) as Cuadrado from Productos p;
-- d) Obtener el signo del precio y un n�mero aleatorio por producto.
select sign(p.precio) as Signo, round(rand(checksum(newid())),2) as NumeroAleatorio from Productos p;

-- 2: Funciones de Texto
-- a) Convertir todos los nombres a may�sculas.
select upper(p.nombre) as NombresMayuscula from Productos p;
-- b) Obtener la longitud del nombre del producto.
select len(p.nombre) as LongitudNombre from Productos p;
-- c) Extraer las primeras 5 letras del nombre del producto.
select substring(p.nombre, 1, 5) as Substrayendo from Productos p;
-- d) Reemplazar la palabra 'Samsung' por 'LG'.
select replace(p.nombre, 'Samsung', 'LG') as Reemplazo 
	from Productos p where p.Nombre like '%Samsung%';

-- 3: Funciones de Fecha
-- a) Mostrar la fecha actual del sistema.
select current_timestamp as FechaHoraActual;
-- b) Calcular cu�ntos d�as lleva cada empleado en la empresa.
select e.fechaingreso, 
	datediff(day, e.fechaingreso, current_timestamp) as Dias from Empleados e;
-- c) Sumar 30 d�as a la fecha de ingreso.
select e.fechaingreso as SinDiasAgregados, 
	dateadd(day, 30, e.fechaingreso) as AgregaDias from Empleados e;
-- d) Mostrar solo el a�o de ingreso.
select year(e.fechaingreso) as A�oIngreso from Empleados e;

-- 4: Funciones Agregadas
-- a) Calcular el precio promedio de todos los productos.
select avg(p.precio) as PromedioGeneral from Productos p;
-- b) Mostrar el precio m�nimo y m�ximo por categor�a.
select max(p.precio) as PrecioMaximo, 
	min(p.precio) as PrecioMinimo from Productos p;
-- c) Contar cu�ntos productos hay por categor�a.
select count(p.nombre) as ProductosPorCategoria 
	from Productos p group by p.Categoria;
-- d) Sumar el total de precios por categor�a.
select sum(p.precio) as TotalPorCategoria 
	from Productos p group by p.Categoria;

-- 5: Funciones de Ventana
-- a) Obtener un ranking global por precio (descendente).
select p.nombre, p.precio, 
	rank() over (order by p.precio desc) as Ranking from Productos p;
-- b) Obtener un ranking por categor�a usando RANK().
select p.categoria, p.nombre, p.precio, 
	rank() over (order by p.categoria desc) as Ranking from Productos p;
-- c) Mostrar el n�mero de fila por categor�a con ROW_NUMBER().
select p.categoria, p.nombre, p.precio, 
	row_number() over (partition by p.categoria order by p.precio) as FilaCategoria 
		from Productos p;
-- d) Comparar los resultados de RANK vs DENSE_RANK.
select p.categoria, p.nombre, p.precio, 
	rank() over (order by p.precio desc) as RankPrecio, 
		dense_rank() over (order by p.precio desc) as DenseRankPrecio 
			from Productos p;
go

-- 6: Funciones Definidas por el Usuario (UDF)
-- a) Aplica la funci�n a la tabla Empleados.
create function fn_DevolverSalarioEmpleado(@SalarioMensual decimal(10,2))
returns decimal(10,2)
as
begin
	return @SalarioMensual * 13;

end;
go

select e.nombre, e.salario, dbo.fn_DevolverSalarioEmpleado(e.salario) as SalarioAnual from empleados e;
go

-- b) Modifica la funci�n para aceptar un n�mero de pagos como par�metro.
create function fn_DevolverSalarioEmpleadoParams(@SalarioMensual decimal(10,2), @CantidadPagos int)
returns decimal(10,2)
as
begin
	return @SalarioMensual * @CantidadPagos;

end;
go



-- c) Usa la funci�n en una consulta que calcule el salario anual con 14 pagos.
select e.nombre, e.salario, 
	dbo.fn_DevolverSalarioEmpleadoParams(e.salario, 14) as SalarioAnual from empleados e;
go


-- d) Modifica la funci�n para que calcule el salario anual con prestaciones.
create function fn_SalarioAnualConPrestaciones(
    @SalarioMensual decimal(10,2),
    @CantidadPagos int,
    @PorcentajePrestaciones decimal(5,2)  -- por ejemplo: 0.30 = 30%
)
returns decimal(10,2)
as
begin
    declare @SalarioBase decimal(10,2) = @SalarioMensual * @CantidadPagos;
    declare @Prestaciones decimal(10,2) = @SalarioBase * @PorcentajePrestaciones;
    return @SalarioBase + @Prestaciones;
end;
go

select e.nombre, e.salario, dbo.fn_SalarioAnualConPrestaciones(e.salario, 14, 0.30) as SalarioConPrestaciones from empleados e;
go