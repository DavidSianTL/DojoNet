CREATE DATABASE tiendaDB;

GO

USE tiendaDB;


CREATE TABLE Empleados(
	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	puesto NVARCHAR(50) NOT NULL,
	rendimiento NVARCHAR(50) NOT NULL,
	salario DECIMAL(10,2) NOT NULL,
	fechaIngreso DATE DEFAULT GETDATE()
);

CREATE TABLE Productos(
	id INT PRIMARY KEY IDENTITY(1,1),
	nombre NVARCHAR(50) NOT NULL,
	costo DECIMAL(10,2) NOT NULL,
	tipo NVARCHAR(50) NOT NULL,
	stock INT NOT NULL,
	fechaRegistro DATE DEFAULT GETDATE()
);



CREATE TABLE Log_empleados(
	id INT PRIMARY KEY IDENTITY(1,1),
	fk_id_empleado INT NULL,
	nombre_empleado NVARCHAR(50) NOT NULL,
	accion NVARCHAR(100) NOT NULL,
	mensaje_error NVARCHAR(250),
	fecha_registro DATETIME DEFAULT GETDATE(),

	FOREIGN KEY (fk_id_empleado) REFERENCES Empleados(id)	
);



CREATE TABLE Log_productos(
	id INT PRIMARY KEY IDENTITY(1,1),
	fk_id_producto INT NULL,
	nombre_producto NVARCHAR(50) NOT NULL,
	accion NVARCHAR(100) NOT NULL,
	mensaje_error NVARCHAR(250),
	fecha_registro DATETIME DEFAULT GETDATE(),

	FOREIGN KEY (fk_id_producto) REFERENCES Productos(id)	
);
