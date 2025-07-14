use bbddSmeall;
go


CREATE TABLE Almacen (
    IdAlmacen INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Ubicacion VARCHAR(100) NOT NULL,
    Capacidad INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,      
    Contrasena VARCHAR(255) NOT NULL,               -
    DNI VARCHAR(20) NOT NULL UNIQUE,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,             
    Genero VARCHAR(6) CHECK (Genero IN ('Hombre', 'Mujer')) NOT NULL,
    Area_Asignada VARCHAR(50) CHECK (Area_Asignada IN ('Area de Compras', 'Area de Almacén', 'Area de Ventas', 'Administración General')) NOT NULL,
    Rol VARCHAR(20) NOT NULL,                       -- aqui colocas Cual es el empleado
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Cliente (
    IdCliente INT PRIMARY KEY IDENTITY(1,1),
    Nombres VARCHAR(100) NOT NULL,
    Correo VARCHAR(100),
    Telefono VARCHAR(20),
    Direccion VARCHAR(200),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Producto (
    IdProducto INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Descripcion VARCHAR(100),
    StockTotal INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE StockAlmacen (
    IdStock INT PRIMARY KEY IDENTITY(1,1),
    IdAlmacen INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdAlmacen) REFERENCES Almacen(IdAlmacen),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);


CREATE TABLE Proveedores (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Telefono VARCHAR(15) NOT NULL,
    Direccion VARCHAR(200) NOT NULL,
    Tipo_Proveedor VARCHAR(15) CHECK (Tipo_Proveedor IN ('Mayorista', 'Minorista')) NOT NULL,
    Tipo_Persona VARCHAR(20) CHECK (Tipo_Persona IN ('Natural', 'Jurídica')) NOT NULL,
    Tipo_Documento VARCHAR(30) CHECK (Tipo_Documento IN ('RUC', 'DNI', 'Carnet Extranjería', 'Otros')) NOT NULL,
    Numero_Documento VARCHAR(20) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Compra (
    IdCompra INT PRIMARY KEY IDENTITY(1,1),
    FechaCompra DATE NOT NULL DEFAULT GETDATE(),
    IdAlmacen INT NOT NULL,
    IdUsuario INT NOT NULL,
    IdProveedor INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdAlmacen) REFERENCES Almacen(IdAlmacen),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdProveedor) REFERENCES Proveedores(Id)
);


CREATE TABLE DetalleCompra (
    IdDetalleCompra INT PRIMARY KEY IDENTITY(1,1),
    IdCompra INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdCompra) REFERENCES Compra(IdCompra),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);


CREATE TABLE Venta (
    IdVenta INT PRIMARY KEY IDENTITY(1,1),
    FechaVenta DATE NOT NULL DEFAULT GETDATE(),
    IdAlmacen INT NOT NULL,
    IdUsuario INT NOT NULL,
    IdCliente INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdAlmacen) REFERENCES Almacen(IdAlmacen),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
);


CREATE TABLE DetalleVenta (
    IdDetalleVenta INT PRIMARY KEY IDENTITY(1,1),
    IdVenta INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdVenta) REFERENCES Venta(IdVenta),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);


CREATE TABLE GuiaSalida (
    IdGuiaSalida INT PRIMARY KEY IDENTITY(1,1),
    FechaSalida DATE NOT NULL DEFAULT GETDATE(),
    Responsable VARCHAR(100) NOT NULL,
    Destino VARCHAR(100) NOT NULL,
    IdAlmacen INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdAlmacen) REFERENCES Almacen(IdAlmacen)
);


CREATE TABLE DetalleGuiaSalida (
    IdDetalleGuia INT PRIMARY KEY IDENTITY(1,1),
    IdGuiaSalida INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdGuiaSalida) REFERENCES GuiaSalida(IdGuiaSalida),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);
go

-- p´rocedimientos almacenados para que los usen... Si los adapnta por favor avisen. Gracias

--para verificar el usuario y contraseña

CREATE PROCEDURE sp_verificarlogin
    @UsuarioInput VARCHAR(100), -- puede ser username o email
    @Contrasena VARCHAR(255)
AS
BEGIN
    SELECT 
        IdUsuario,
        NombreUsuario,
        Email,
        Nombres,
        Apellidos,
        Rol,
        Area_Asignada
    FROM Usuario
    WHERE (NombreUsuario = @UsuarioInput OR Email = @UsuarioInput)
      AND Contrasena = @Contrasena;
END;

go

-- para agregar un usuario

CREATE PROCEDURE sp_InsertarUsuario
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(255),
    @DNI VARCHAR(20),
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Telefono VARCHAR(15),
    @Email VARCHAR(100),
    @Genero VARCHAR(6),
    @Area_Asignada VARCHAR(50),
    @Rol VARCHAR(20)
AS
BEGIN
    INSERT INTO Usuario (
        NombreUsuario, Contrasena, DNI, Nombres, Apellidos,
        Telefono, Email, Genero, Area_Asignada, Rol, FechaRegistro, FechaActualizacion
    )
    VALUES (
        @NombreUsuario, @Contrasena, @DNI, @Nombres, @Apellidos,
        @Telefono, @Email, @Genero, @Area_Asignada, @Rol, GETDATE(), GETDATE()
    );
END;

go
-- para agregar productos


CREATE PROCEDURE sp_InsertarProducto
    @Nombre VARCHAR(100),
    @Precio DECIMAL(10,2),
    @Descripcion VARCHAR(100),
    @StockTotal INT
AS
BEGIN
    INSERT INTO Producto (Nombre, Precio, Descripcion, StockTotal, FechaRegistro, FechaActualizacion)
    VALUES (@Nombre, @Precio, @Descripcion, @StockTotal, GETDATE(), GETDATE());
END;


-- para registar las compra

CREATE PROCEDURE sp_RegistrarCompra
    @IdAlmacen INT,
    @IdUsuario INT,
    @IdProveedor INT
AS
BEGIN
    INSERT INTO Compra (FechaCompra, IdAlmacen, IdUsuario, IdProveedor, FechaRegistro, FechaActualizacion)
    VALUES (GETDATE(), @IdAlmacen, @IdUsuario, @IdProveedor, GETDATE(), GETDATE());

    SELECT SCOPE_IDENTITY() AS IdCompra;
END;


CREATE PROCEDURE sp_DetalleCompra
    @IdCompra INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO DetalleCompra (IdCompra, IdProducto, Cantidad, PrecioUnitario, FechaRegistro, FechaActualizacion)
    VALUES (@IdCompra, @IdProducto, @Cantidad, @PrecioUnitario, GETDATE(), GETDATE());

    
    UPDATE Producto
    SET StockTotal = StockTotal + @Cantidad,
        FechaActualizacion = GETDATE()
    WHERE IdProducto = @IdProducto;
END;

-- para registra una venta 


CREATE PROCEDURE sp_RegistrarVenta
    @IdAlmacen INT,
    @IdUsuario INT,
    @IdCliente INT
AS
BEGIN
    INSERT INTO Venta (FechaVenta, IdAlmacen, IdUsuario, IdCliente, FechaRegistro, FechaActualizacion)
    VALUES (GETDATE(), @IdAlmacen, @IdUsuario, @IdCliente, GETDATE(), GETDATE());

    SELECT SCOPE_IDENTITY() AS IdVenta;
END;


CREATE PROCEDURE sp_DetalleVenta
    @IdVenta INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario, FechaRegistro, FechaActualizacion)
    VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, GETDATE(), GETDATE());

    
    UPDATE Producto
    SET StockTotal = StockTotal - @Cantidad,
        FechaActualizacion = GETDATE()
    WHERE IdProducto = @IdProducto;
END;


--Busqueda de productos

CREATE PROCEDURE sp_BuscarProductoPorNombre
    @NombreProducto VARCHAR(100)
AS
BEGIN
    SELECT 
        IdProducto,
        Nombre,
        Descripcion,
        Precio,
        StockTotal,
        FechaRegistro,
        FechaActualizacion
    FROM Producto
    WHERE Nombre LIKE '%' + @NombreProducto + '%';
END;

-- compras por mes 

CREATE PROCEDURE sp_RepComprasPorMes
AS
BEGIN
    SELECT 
        YEAR(FechaCompra) AS Año,
        MONTH(FechaCompra) AS Mes,
        COUNT(*) AS TotalCompras,
        SUM(DC.Cantidad * DC.PrecioUnitario) AS TotalGastado
    FROM Compra C
    INNER JOIN DetalleCompra DC ON C.IdCompra = DC.IdCompra
    GROUP BY YEAR(FechaCompra), MONTH(FechaCompra)
    ORDER BY Año DESC, Mes DESC;
END;


-- si quieres ver el stock por cada almacen que tengas.. eje: bolsos, no se otro mochilas, 


CREATE PROCEDURE sp_StockPorAlmacenescreados
    @IdAlmacen INT
AS
BEGIN
    SELECT 
        A.Nombre AS Almacen,
        P.Nombre AS Producto,
        SA.Cantidad,
        P.StockTotal
    FROM StockAlmacen SA
    INNER JOIN Almacen A ON SA.IdAlmacen = A.IdAlmacen
    INNER JOIN Producto P ON SA.IdProducto = P.IdProducto
    WHERE SA.IdAlmacen = @IdAlmacen;
END;







