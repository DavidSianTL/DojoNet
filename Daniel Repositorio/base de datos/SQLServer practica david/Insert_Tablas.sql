
-- Candidatos
INSERT INTO Candidatos (Nombre, Email) VALUES
('Ana Torres', 'ana.torres@email.com'),
('Carlos Pérez', 'carlos.perez@email.com'),
('Luis Gómez', 'luis.gomez@email.com'),
('María López', 'maria.lopez@email.com'),
('Pedro Martínez', 'pedro.martinez@email.com');

-- Categorías
INSERT INTO Categorias (NombreCategoria) VALUES
('Lógica'),
('Algoritmos'),
('Bases de Datos'),
('Frontend'),
('Backend');

-- Lenguajes
INSERT INTO Lenguajes (NombreLenguaje) VALUES
('C#'),
('JavaScript'),
('Python'),
('SQL'),
('Java');

-- Ejercicios
INSERT INTO Ejercicios (Titulo, Descripcion, IdCategoria, IdLenguaje) VALUES
('Suma de números', 'Escriba una función que sume dos números', 1, 1),
('Buscar en arreglo', 'Buscar un número en un arreglo ordenado', 2, 3),
('Consulta JOIN', 'Escriba una consulta JOIN entre dos tablas', 3, 4),
('Formulario simple', 'Cree un formulario con validaciones', 4, 2),
('API REST básica', 'Cree un controlador que devuelva datos JSON', 5, 1);

-- Resultados
INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje) VALUES
(1, 1, '2024-05-01', 90),
(1, 2, '2024-05-02', 85),
(2, 3, '2024-05-01', 70),
(3, 4, '2024-05-03', 95),
(4, 5, '2024-05-01', 88);

-- Roles
INSERT INTO Roles (NombreRol) VALUES
('Administrador'),
('Instructor'),
('Candidato'),
('Evaluador');

-- Permisos
INSERT INTO Permisos (NombrePermiso) VALUES
('Ver Resultados'),
('Editar Ejercicios'),
('Asignar Pruebas'),
('Acceder Panel'),
('Enviar Ejercicio');

-- CandidatosRoles
INSERT INTO CandidatosRoles (IdCandidato, IdRol) VALUES
(1, 3),
(2, 3),
(3, 3),
(4, 2),
(4, 1),
(5, 4);

-- RolesPermisos
INSERT INTO RolesPermisos (IdRol, IdPermiso) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
(2, 1), (2, 2), (2, 3), (2, 5),
(3, 1), (3, 5),
(4, 1), (4, 3);