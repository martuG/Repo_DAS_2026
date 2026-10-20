-- Catálogo de ejemplo para el patrón Composite.
-- El precio total no se guarda: lo calcula Caja.Precio() en la aplicación.
--
-- Pack cables          = 200 + 3500 + 2500                         = 6200
-- Kit escritorio       = 800 + 9000 + 4000 + 6200                  = 20000
-- Pedido de bienvenida = 1500 + 12000 + 20000 + 6000               = 39500
-- Combo regalo         = 1000 + 3000 + 1500                        = 5500
-- Catálogo tienda      = 0 + 39500 + 18000 + 5500                  = 63000

IF DB_ID(N'CatalogoComposite') IS NULL
    CREATE DATABASE CatalogoComposite;
GO

USE CatalogoComposite;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Componentes')
BEGIN
    CREATE TABLE Componentes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(80) NOT NULL,
        EsCaja BIT NOT NULL,
        PrecioUnitario DECIMAL(18,2) NULL,
        CostoEmpaque DECIMAL(18,2) NULL,
        PadreId INT NULL,
        CONSTRAINT FK_Componentes_Padre FOREIGN KEY (PadreId) REFERENCES Componentes(Id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM Componentes)
BEGIN
    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Catálogo tienda', 1, NULL, 0, NULL);
    DECLARE @Catalogo INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Pedido de bienvenida', 1, NULL, 1500, @Catalogo);
    DECLARE @Pedido INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Remera', 0, 12000, NULL, @Pedido);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Kit escritorio', 1, NULL, 800, @Pedido);
    DECLARE @Kit INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES
        (N'Mouse', 0, 9000, NULL, @Kit),
        (N'Mousepad', 0, 4000, NULL, @Kit);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Pack cables', 1, NULL, 200, @Kit);
    DECLARE @Cables INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES
        (N'Cable HDMI', 0, 3500, NULL, @Cables),
        (N'Cable USB-C', 0, 2500, NULL, @Cables);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Taza', 0, 6000, NULL, @Pedido);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Libro de patrones', 0, 18000, NULL, @Catalogo);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES (N'Combo regalo', 1, NULL, 1000, @Catalogo);
    DECLARE @Combo INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
    VALUES
        (N'Cuaderno', 0, 3000, NULL, @Combo),
        (N'Lapicera', 0, 1500, NULL, @Combo);
END
GO
