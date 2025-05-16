USE [CursoresPablotorres]
GO

DECLARE @RC int
DECLARE @Nombre varchar(100)
DECLARE @Costo decimal(10,2)
DECLARE @Tipo varchar(50)
DECLARE @Stock int
DECLARE @FechaRegistro date

-- TODO: Set parameter values here.
SET @Nombre= 'Sprite';
SET @Costo= 10;
SET @Tipo = 'REFRESCO';
SET @Stock = 1 ;
SET @FechaRegistro = GETDATE();



EXECUTE @RC = [dbo].[sp_ingresar_producto] 
   @Nombre
  ,@Costo
  ,@Tipo
  ,@Stock
  ,@FechaRegistro
GO
USE CursoresPablotorres
GO
SELECT * FROM Productos






