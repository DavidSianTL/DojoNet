
-- Candidatos
INSERT INTO Candidatos (Nombre, Email) VALUES
('Ana Torres', 'ana.torres@email.com'),
('Carlos P�rez', 'carlos.perez@email.com'),
('Luis G�mez', 'luis.gomez@email.com'),
('Mar�a L�pez', 'maria.lopez@email.com'),
('Pedro Mart�nez', 'pedro.martinez@email.com');

-- Categor�as
INSERT INTO Categorias (NombreCategoria) VALUES
('L�gica'),
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
('Suma de n�meros', 'Escriba una funci�n que sume dos n�meros', 1, 1),
('Buscar en arreglo', 'Buscar un n�mero en un arreglo ordenado', 2, 3),
('Consulta JOIN', 'Escriba una consulta JOIN entre dos tablas', 3, 4),
('Formulario simple', 'Cree un formulario con validaciones', 4, 2),
('API REST b�sica', 'Cree un controlador que devuelva datos JSON', 5, 1);

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