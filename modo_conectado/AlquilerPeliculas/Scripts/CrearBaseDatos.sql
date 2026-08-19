CREATE DATABASE AlquilerPeliculas;
GO

USE AlquilerPeliculas;
GO

CREATE TABLE Peliculas
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Director NVARCHAR(100) NOT NULL,
    Duracion INT NOT NULL
);
GO
