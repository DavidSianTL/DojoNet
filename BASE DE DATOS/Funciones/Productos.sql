-- Crear la tabla Productos
CREATE TABLE Productos (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    Precio DECIMAL(10,2),
    Categoria VARCHAR(30)
);

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
