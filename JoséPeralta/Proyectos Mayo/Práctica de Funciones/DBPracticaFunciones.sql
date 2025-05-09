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

-- Usar la base de datos recién creada
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
(1, 'Laptop Lenovo', 1200.00, 'Tecnología'),
(2, 'Smartphone Samsung', 900.00, 'Tecnología'),
(3, 'Teclado Logitech', 60.00, 'Tecnología'),
(4, 'Cafetera Oster', 85.00, 'Electrodomésticos'),
(5, 'Refrigeradora LG', 650.00, 'Electrodomésticos'),
(6, 'Batidora Philips', 85.00, 'Electrodomésticos'),
(7, 'Mouse HP', 25.00, 'Tecnología'),
(8, 'Monitor Dell', 250.00, 'Tecnología'),
(9, 'Aspiradora Samsung', 300.00, 'Electrodomésticos'),
(10, 'Tablet Huawei', 350.00, 'Tecnología');
go

select * from Empleados;
select * from Productos;

-- 1: Funciones Matemáticas Escalares
-- a) Mostrar el valor absoluto del precio de cada producto.
select abs(p.precio) as ValorAbsoluto from Productos p;
-- b) Redondear el precio hacia arriba y hacia abajo.
select ceiling(p.precio) as RedondeadoHaciaArriba, floor(p.precio) as RedondeadoHaciaAbajo from Productos p;
-- c) Calcular la raíz cuadrada y el cuadrado del precio.
select sqrt(p.precio) as RaizCuadrada, power(p.precio, 2) as Cuadrado from Productos p;
-- d) Obtener el signo del precio y un número aleatorio por producto.
select sign(p.precio) as Signo, round(rand(checksum(newid())),2) as NumeroAleatorio from Productos p;

-- 2: Funciones de Texto
-- a) Convertir todos los nombres a mayúsculas.
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
-- b) Calcular cuántos días lleva cada empleado en la empresa.
select e.fechaingreso, 
	datediff(day, e.fechaingreso, current_timestamp) as Dias from Empleados e;
-- c) Sumar 30 días a la fecha de ingreso.
select e.fechaingreso as SinDiasAgregados, 
	dateadd(day, 30, e.fechaingreso) as AgregaDias from Empleados e;
-- d) Mostrar solo el año de ingreso.
select year(e.fechaingreso) as AñoIngreso from Empleados e;

-- 4: Funciones Agregadas
-- a) Calcular el precio promedio de todos los productos.
select avg(p.precio) as PromedioGeneral from Productos p;
-- b) Mostrar el precio mínimo y máximo por categoría.
select max(p.precio) as PrecioMaximo, 
	min(p.precio) as PrecioMinimo from Productos p;
-- c) Contar cuántos productos hay por categoría.
select count(p.nombre) as ProductosPorCategoria 
	from Productos p group by p.Categoria;
-- d) Sumar el total de precios por categoría.
select sum(p.precio) as TotalPorCategoria 
	from Productos p group by p.Categoria;

-- 5: Funciones de Ventana
-- a) Obtener un ranking global por precio (descendente).
select p.nombre, p.precio, 
	rank() over (order by p.precio desc) as Ranking from Productos p;
-- b) Obtener un ranking por categoría usando RANK().
select p.categoria, p.nombre, p.precio, 
	rank() over (order by p.categoria desc) as Ranking from Productos p;
-- c) Mostrar el número de fila por categoría con ROW_NUMBER().
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
-- a) Aplica la función a la tabla Empleados.
create function fn_DevolverSalarioEmpleado(@SalarioMensual decimal(10,2))
returns decimal(10,2)
as
begin
	return @SalarioMensual * 13;

end;
go

select e.nombre, e.salario, dbo.fn_DevolverSalarioEmpleado(e.salario) as SalarioAnual from empleados e;
go

-- b) Modifica la función para aceptar un número de pagos como parámetro.
create function fn_DevolverSalarioEmpleadoParams(@SalarioMensual decimal(10,2), @CantidadPagos int)
returns decimal(10,2)
as
begin
	return @SalarioMensual * @CantidadPagos;

end;
go



-- c) Usa la función en una consulta que calcule el salario anual con 14 pagos.
select e.nombre, e.salario, 
	dbo.fn_DevolverSalarioEmpleadoParams(e.salario, 14) as SalarioAnual from empleados e;
go


-- d) Modifica la funciòn para que calcule el salario anual con prestaciones.
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