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


CREATE TABLE GuiaEntrada (
    IdGuiaEntrada INT PRIMARY KEY IDENTITY(1,1),
    FechaEntrada DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    Proveedor VARCHAR(150) NOT NULL,
    DocumentoReferencia VARCHAR(50) NULL, 
    IdAlmacen INT NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_GuiaEntrada_Almacen FOREIGN KEY (IdAlmacen)
        REFERENCES Almacen(IdAlmacen)
);
GO

CREATE TABLE DetalleGuiaEntrada (
    IdDetalleGuiaEntrada INT PRIMARY KEY IDENTITY(1,1),
    IdGuiaEntrada INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DetalleGuiaEntrada_GuiaEntrada FOREIGN KEY (IdGuiaEntrada)
        REFERENCES GuiaEntrada(IdGuiaEntrada)
        ON DELETE CASCADE,
    CONSTRAINT FK_DetalleGuiaEntrada_Producto FOREIGN KEY (IdProducto)
        REFERENCES Producto(IdProducto)
);
GO



--Tienda, carrito, pasarela
CREATE TABLE Categoria (
    IdCategoria INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(80) NOT NULL,
    Slug VARCHAR(100) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    Orden INT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Categoria_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE ProductoCategoria (
    IdProducto INT NOT NULL,
    IdCategoria INT NOT NULL,
    PRIMARY KEY (IdProducto, IdCategoria),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria)
);
GO

CREATE TABLE ProductoImagen (
    IdProductoImagen INT PRIMARY KEY IDENTITY(1,1),
    IdProducto INT NOT NULL,
    Url VARCHAR(300) NOT NULL,
    EsPrincipal BIT NOT NULL DEFAULT 0,
    Orden INT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);
GO


CREATE UNIQUE INDEX UX_ProductoImagen_Principal
  ON ProductoImagen(IdProducto)
  WHERE EsPrincipal = 1;
GO



CREATE TABLE DireccionCliente (
    IdDireccion INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    Receptor VARCHAR(120) NOT NULL,          
    Telefono VARCHAR(20) NULL,
    Direccion VARCHAR(200) NOT NULL,
    Referencia VARCHAR(200) NULL,
    Distrito VARCHAR(80) NULL,
    Provincia VARCHAR(80) NULL,
    Departamento VARCHAR(80) NULL,
    Principal BIT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
);
GO

CREATE TABLE OrdenEnvio (
    IdOrden INT PRIMARY KEY,                 
    IdDireccion INT NULL,                    
    MetodoEnvio VARCHAR(15) NOT NULL         
        CHECK (MetodoEnvio IN ('Recojo','Delivery')),
    Costo DECIMAL(10,2) NOT NULL DEFAULT 0,
    Courier VARCHAR(50) NULL,                
    TrackingCode VARCHAR(60) NULL,
    EstadoEnvio VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (EstadoEnvio IN ('PENDIENTE','EN_CAMINO','ENTREGADO','FALLIDO','CANCELADO')),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdOrden) REFERENCES Orden(IdOrden),
    FOREIGN KEY (IdDireccion) REFERENCES DireccionCliente(IdDireccion)
);
GO



CREATE OR ALTER VIEW v_StockProducto AS
SELECT
    p.IdProducto,
    p.Nombre,
    p.Precio,
    p.Descripcion,
    SUM(ISNULL(sa.Cantidad,0)) AS StockDisponible
FROM Producto p
LEFT JOIN StockAlmacen sa ON sa.IdProducto = p.IdProducto
GROUP BY p.IdProducto, p.Nombre, p.Precio, p.Descripcion;
GO


CREATE TABLE Carrito (
    IdCarrito INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NULL,                          
    SessionId VARCHAR(100) NULL,                 
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activo' 
        CHECK (Estado IN ('Activo','Convertido','Cancelado')),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);
GO

CREATE TABLE CarritoItem (
    IdCarritoItem INT PRIMARY KEY IDENTITY(1,1),
    IdCarrito INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitarioSnapshot DECIMAL(10,2) NOT NULL, 
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdCarrito) REFERENCES Carrito(IdCarrito),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);
GO


CREATE TABLE Orden (
    IdOrden INT PRIMARY KEY IDENTITY(1,1),
    CodigoOrden VARCHAR(30) NOT NULL UNIQUE,      
    IdCarrito INT NOT NULL,                       
    IdCliente INT NULL,                           
    IdAlmacen INT NOT NULL,                       
    Subtotal DECIMAL(10,2) NOT NULL,
    Descuento DECIMAL(10,2) NOT NULL DEFAULT 0,
    Envio DECIMAL(10,2) NOT NULL DEFAULT 0,
    Total DECIMAL(10,2) NOT NULL,
    Moneda VARCHAR(3) NOT NULL DEFAULT 'PEN',
    Estado VARCHAR(20) NOT NULL DEFAULT 'CREADA'  
        CHECK (Estado IN ('CREADA','PENDIENTE_PAGO','PAGADA','FALLIDA','ANULADA','EXPIRADA')),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdCarrito) REFERENCES Carrito(IdCarrito),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente),
    FOREIGN KEY (IdAlmacen) REFERENCES Almacen(IdAlmacen)
);
GO

CREATE TABLE OrdenItem (
    IdOrdenItem INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdOrden) REFERENCES Orden(IdOrden),
    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);
GO


CREATE TABLE Pago (
    IdPago INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    Metodo VARCHAR(20) NOT NULL                  
        CHECK (Metodo IN ('EFECTIVO','TARJETA','YAPE','PLIN','PAGOEFECTIVO')),
    Monto DECIMAL(10,2) NOT NULL,
    Moneda VARCHAR(3) NOT NULL DEFAULT 'PEN',
    Estado VARCHAR(20) NOT NULL DEFAULT 'CREADO' 
        CHECK (Estado IN ('CREADO','PENDIENTE','APROBADO','RECHAZADO','ANULADO','EXPIRADO','DEVUELTO')),

   
    TarjetaMarca VARCHAR(12) NULL CHECK (TarjetaMarca IN ('VISA','MASTERCARD')),
    AuthorizationCode VARCHAR(50) NULL,
    CardLast4 CHAR(4) NULL,
    ExternalId VARCHAR(100) NULL,                

   -- Aquí van los yape y plines. no los toquen sin autorizacio´n
    WalletTelefono VARCHAR(20) NULL,            
    WalletRef VARCHAR(100) NULL,                 
    QrData NVARCHAR(MAX) NULL,                  
    QrExpiry DATETIME NULL,

    -- Aquí va el pagoefectivo
    CipCode VARCHAR(30) NULL,
    CipExpiry DATETIME NULL,

    -- General
    ReturnUrl VARCHAR(300) NULL,
    Observaciones NVARCHAR(500) NULL,

    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdOrden) REFERENCES Orden(IdOrden)
);
GO


CREATE TABLE WebhookPago (
    IdWebhook INT PRIMARY KEY IDENTITY(1,1),
    IdPago INT NULL,
    Origen VARCHAR(20) NOT NULL                 
        CHECK (Origen IN ('TARJETA','YAPE','PLIN','PAGOEFECTIVO')),
    EventType VARCHAR(100) NOT NULL,            
    Payload NVARCHAR(MAX) NOT NULL,            
    Procesado BIT NOT NULL DEFAULT 0,
    FechaRecepcion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdPago) REFERENCES Pago(IdPago)
);
GO

-- procedimientos almacenados para que los usen... Si los adapnta por favor avisen. Gracias

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
    FROM StockAlmacen S
    INNER JOIN Almacen A ON SA.IdAlmacen = A.IdAlmacen
    INNER JOIN Producto P ON SA.IdProducto = P.IdProducto
    WHERE SA.IdAlmacen = @IdAlmacen;
END;
go

CREATE OR ALTER PROCEDURE dbo.sp_CrearOrdenDesdeCarrito
  @IdCarrito INT,
  @IdCliente INT = NULL,
  @IdAlmacen INT,
  @Envio DECIMAL(10,2) = 0,
  @Descuento DECIMAL(10,2) = 0,
  @Moneda VARCHAR(3) = 'PEN',
  @IdOrden INT OUTPUT,
  @CodigoOrden VARCHAR(30) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRY
    BEGIN TRAN;

    DECLARE @Subtotal DECIMAL(10,2);
    SELECT @Subtotal = SUM(ci.Cantidad * ci.PrecioUnitarioSnapshot)
    FROM CarritoItem ci
    WHERE ci.IdCarrito = @IdCarrito;

    IF @Subtotal IS NULL SET @Subtotal = 0;

    DECLARE @Total DECIMAL(10,2) = @Subtotal - @Descuento + @Envio;

    SET @CodigoOrden = 'ORD' + RIGHT(CONVERT(VARCHAR(8), ABS(CHECKSUM(NEWID()))), 8);

    INSERT INTO Orden(CodigoOrden, IdCarrito, IdCliente, IdAlmacen, Subtotal, Descuento, Envio, Total, Moneda, Estado, FechaRegistro, FechaActualizacion)
    VALUES(@CodigoOrden, @IdCarrito, @IdCliente, @IdAlmacen, @Subtotal, @Descuento, @Envio, @Total, @Moneda, 'PENDIENTE_PAGO', GETDATE(), GETDATE());

    SET @IdOrden = SCOPE_IDENTITY();

    INSERT INTO OrdenItem(IdOrden, IdProducto, Cantidad, PrecioUnitario, Subtotal, FechaRegistro, FechaActualizacion)
    SELECT @IdOrden, ci.IdProducto, ci.Cantidad, ci.PrecioUnitarioSnapshot,
           (ci.Cantidad * ci.PrecioUnitarioSnapshot), GETDATE(), GETDATE()
    FROM CarritoItem ci
    WHERE ci.IdCarrito = @IdCarrito;

    UPDATE Carrito SET Estado = 'Convertido', FechaActualizacion = GETDATE()
    WHERE IdCarrito = @IdCarrito;

    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
  END CATCH
END
go




CREATE OR ALTER PROCEDURE dbo.sp_CrearORecuperarCarrito
  @IdUsuario INT = NULL,
  @SessionId VARCHAR(100) = NULL,
  @IdCarrito INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT TOP 1 @IdCarrito = IdCarrito
  FROM Carrito
  WHERE Estado = 'Activo'
    AND ((@IdUsuario IS NOT NULL AND IdUsuario = @IdUsuario)
      OR (@IdUsuario IS NULL AND @SessionId IS NOT NULL AND SessionId = @SessionId));

  IF @IdCarrito IS NULL
  BEGIN
    INSERT INTO Carrito (IdUsuario, SessionId, Estado, FechaRegistro, FechaActualizacion)
    VALUES (@IdUsuario, @SessionId, 'Activo', GETDATE(), GETDATE());
    SET @IdCarrito = SCOPE_IDENTITY();
  END
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_CrearORecuperarCarrito
  @IdUsuario INT = NULL,
  @SessionId VARCHAR(100) = NULL,
  @IdCarrito INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT TOP 1 @IdCarrito = IdCarrito
  FROM Carrito
  WHERE Estado = 'Activo'
    AND ((@IdUsuario IS NOT NULL AND IdUsuario = @IdUsuario)
      OR (@IdUsuario IS NULL AND @SessionId IS NOT NULL AND SessionId = @SessionId));

  IF @IdCarrito IS NULL
  BEGIN
    INSERT INTO Carrito (IdUsuario, SessionId, Estado, FechaRegistro, FechaActualizacion)
    VALUES (@IdUsuario, @SessionId, 'Activo', GETDATE(), GETDATE());
    SET @IdCarrito = SCOPE_IDENTITY();
  END
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_AgregarOActualizarItemCarrito
  @IdCarrito INT,
  @IdProducto INT,
  @Cantidad INT,
  @PrecioUnitarioSnapshot DECIMAL(10,2),
  @IdCarritoItem INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @Existe INT;
  SELECT @Existe = IdCarritoItem
  FROM CarritoItem
  WHERE IdCarrito = @IdCarrito AND IdProducto = @IdProducto;

  IF @Existe IS NULL
  BEGIN
    INSERT INTO CarritoItem (IdCarrito, IdProducto, Cantidad, PrecioUnitarioSnapshot, FechaRegistro, FechaActualizacion)
    VALUES (@IdCarrito, @IdProducto, @Cantidad, @PrecioUnitarioSnapshot, GETDATE(), GETDATE());
    SET @IdCarritoItem = SCOPE_IDENTITY();
  END
  ELSE
  BEGIN
    UPDATE CarritoItem
      SET Cantidad = Cantidad + @Cantidad,
          FechaActualizacion = GETDATE()
    WHERE IdCarritoItem = @Existe;

    SET @IdCarritoItem = @Existe;
  END
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_CambiarCantidadItemCarrito
  @IdCarritoItem INT,
  @NuevaCantidad INT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE CarritoItem
    SET Cantidad = @NuevaCantidad,
        FechaActualizacion = GETDATE()
  WHERE IdCarritoItem = @IdCarritoItem;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_EliminarItemCarrito
  @IdCarritoItem INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM CarritoItem WHERE IdCarritoItem = @IdCarritoItem;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_VaciarCarrito
  @IdCarrito INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM CarritoItem WHERE IdCarrito = @IdCarrito;
END
GO



CREATE OR ALTER PROCEDURE dbo.sp_AgregarOActualizarItemCarrito
  @IdCarrito INT,
  @IdProducto INT,
  @Cantidad INT,
  @PrecioUnitarioSnapshot DECIMAL(10,2),
  @IdCarritoItem INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @Existe INT;
  SELECT @Existe = IdCarritoItem
  FROM CarritoItem
  WHERE IdCarrito = @IdCarrito AND IdProducto = @IdProducto;

  IF @Existe IS NULL
  BEGIN
    INSERT INTO CarritoItem (IdCarrito, IdProducto, Cantidad, PrecioUnitarioSnapshot, FechaRegistro, FechaActualizacion)
    VALUES (@IdCarrito, @IdProducto, @Cantidad, @PrecioUnitarioSnapshot, GETDATE(), GETDATE());
    SET @IdCarritoItem = SCOPE_IDENTITY();
  END
  ELSE
  BEGIN
    UPDATE CarritoItem
      SET Cantidad = Cantidad + @Cantidad,
          FechaActualizacion = GETDATE()
    WHERE IdCarritoItem = @Existe;

    SET @IdCarritoItem = @Existe;
  END
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_CambiarCantidadItemCarrito
  @IdCarritoItem INT,
  @NuevaCantidad INT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE CarritoItem
    SET Cantidad = @NuevaCantidad,
        FechaActualizacion = GETDATE()
  WHERE IdCarritoItem = @IdCarritoItem;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_EliminarItemCarrito
  @IdCarritoItem INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM CarritoItem WHERE IdCarritoItem = @IdCarritoItem;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_VaciarCarrito
  @IdCarrito INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM CarritoItem WHERE IdCarrito = @IdCarrito;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_CrearOrdenDesdeCarrito
  @IdCarrito INT,
  @IdCliente INT = NULL,
  @IdAlmacen INT,
  @Envio DECIMAL(10,2) = 0,
  @Descuento DECIMAL(10,2) = 0,
  @Moneda VARCHAR(3) = 'PEN',
  @IdOrden INT OUTPUT,
  @CodigoOrden VARCHAR(30) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  BEGIN TRY
    BEGIN TRAN;

    DECLARE @Subtotal DECIMAL(10,2);
    SELECT @Subtotal = ISNULL(SUM(ci.Cantidad * ci.PrecioUnitarioSnapshot),0)
    FROM CarritoItem ci
    WHERE ci.IdCarrito = @IdCarrito;

    DECLARE @Total DECIMAL(10,2) = @Subtotal - ISNULL(@Descuento,0) + ISNULL(@Envio,0);

    
    SET @CodigoOrden = 'ORD' + RIGHT(CONVERT(VARCHAR(8), ABS(CHECKSUM(NEWID()))), 8);

    INSERT INTO Orden (CodigoOrden, IdCarrito, IdCliente, IdAlmacen, Subtotal, Descuento, Envio, Total, Moneda, Estado, FechaRegistro, FechaActualizacion)
    VALUES(@CodigoOrden, @IdCarrito, @IdCliente, @IdAlmacen, @Subtotal, @Descuento, @Envio, @Total, @Moneda, 'PENDIENTE_PAGO', GETDATE(), GETDATE());

    SET @IdOrden = SCOPE_IDENTITY();

    INSERT INTO OrdenItem (IdOrden, IdProducto, Cantidad, PrecioUnitario, Subtotal, FechaRegistro, FechaActualizacion)
    SELECT @IdOrden, ci.IdProducto, ci.Cantidad, ci.PrecioUnitarioSnapshot,
           (ci.Cantidad * ci.PrecioUnitarioSnapshot), GETDATE(), GETDATE()
    FROM CarritoItem ci
    WHERE ci.IdCarrito = @IdCarrito;

    UPDATE Carrito
      SET Estado = 'Convertido',
          FechaActualizacion = GETDATE()
    WHERE IdCarrito = @IdCarrito;

    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
  END CATCH
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_ObtenerOrdenCompleta
  @IdOrden INT
AS
BEGIN
  SET NOCOUNT ON;


  SELECT o.*
  FROM Orden o
  WHERE o.IdOrden = @IdOrden;

 
  SELECT oi.*
  FROM OrdenItem oi
  WHERE oi.IdOrden = @IdOrden;
END
GO




CREATE OR ALTER PROCEDURE dbo.sp_CrearPago
  @IdOrden INT,
  @Metodo VARCHAR(20),             
  @Monto DECIMAL(10,2),
  @Moneda VARCHAR(3) = 'PEN',

  -- TARJETA
  @TarjetaMarca VARCHAR(12) = NULL, 
  @ExternalId VARCHAR(100) = NULL,
  @AuthorizationCode VARCHAR(50) = NULL,
  @CardLast4 CHAR(4) = NULL,

  -- YAPE / PLIN
  @WalletTelefono VARCHAR(20) = NULL,
  @WalletRef VARCHAR(100) = NULL,
  @QrData NVARCHAR(MAX) = NULL,
  @QrExpiry DATETIME = NULL,

  -- PagoEfectivo
  @CipCode VARCHAR(30) = NULL,
  @CipExpiry DATETIME = NULL,

  -- General
  @ReturnUrl VARCHAR(300) = NULL,
  @Observaciones NVARCHAR(500) = NULL,

  @IdPago INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  
  IF @ExternalId IS NULL SET @ExternalId = CONVERT(VARCHAR(36), NEWID());
  IF (@Metodo IN ('YAPE','PLIN') AND @QrData IS NULL) SET @QrData = 'QR-' + CONVERT(VARCHAR(36), NEWID());
  IF (@Metodo = 'PAGOEFECTIVO' AND @CipCode IS NULL) SET @CipCode = 'CIP' + RIGHT(CONVERT(VARCHAR(16), ABS(CHECKSUM(NEWID()))), 8);

  INSERT INTO Pago (
    IdOrden, Metodo, Monto, Moneda, Estado,
    TarjetaMarca, AuthorizationCode, CardLast4, ExternalId,
    WalletTelefono, WalletRef, QrData, QrExpiry,
    CipCode, CipExpiry, ReturnUrl, Observaciones,
    FechaRegistro, FechaActualizacion
  )
  VALUES (
    @IdOrden, @Metodo, @Monto, @Moneda, 'PENDIENTE',
    @TarjetaMarca, @AuthorizationCode, @CardLast4, @ExternalId,
    @WalletTelefono, @WalletRef, @QrData, @QrExpiry,
    @CipCode, @CipExpiry, @ReturnUrl, @Observaciones,
    GETDATE(), GETDATE()
  );

  SET @IdPago = SCOPE_IDENTITY();
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_ActualizarEstadoPago
  @IdPago INT,
  @NuevoEstado VARCHAR(20),           
  @AuthorizationCode VARCHAR(50) = NULL,
  @ExternalId VARCHAR(100) = NULL,
  @CardLast4 CHAR(4) = NULL,
  @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  DECLARE @IdOrden INT;

  BEGIN TRY
    BEGIN TRAN;

    UPDATE Pago
      SET Estado = @NuevoEstado,
          AuthorizationCode = COALESCE(@AuthorizationCode, AuthorizationCode),
          ExternalId = COALESCE(@ExternalId, ExternalId),
          CardLast4 = COALESCE(@CardLast4, CardLast4),
          Observaciones = COALESCE(@Observaciones, Observaciones),
          FechaActualizacion = GETDATE()
    WHERE IdPago = @IdPago;

    SELECT @IdOrden = IdOrden FROM Pago WHERE IdPago = @IdPago;

    IF @NuevoEstado = 'APROBADO'
    BEGIN
      UPDATE Orden
        SET Estado = 'PAGADA',
            FechaActualizacion = GETDATE()
      WHERE IdOrden = @IdOrden;
    END

    IF @NuevoEstado IN ('RECHAZADO','ANULADO','EXPIRADO','DEVUELTO')
    BEGIN
      
      UPDATE Orden
        SET FechaActualizacion = GETDATE()
      WHERE IdOrden = @IdOrden AND Estado = 'PENDIENTE_PAGO';
    END

    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
  END CATCH
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_ConfirmarPagoEfectivo
  @IdPago INT
AS
BEGIN
  SET NOCOUNT ON;

  EXEC dbo.sp_ActualizarEstadoPago
    @IdPago = @IdPago,
    @NuevoEstado = 'APROBADO';
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_RegistrarWebhookPago
  @Origen VARCHAR(20),            
  @EventType VARCHAR(100),
  @Payload NVARCHAR(MAX),
  @IdPago INT = NULL,
  @IdWebhook INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO WebhookPago (IdPago, Origen, EventType, Payload, Procesado, FechaRecepcion)
  VALUES (@IdPago, @Origen, @EventType, @Payload, 0, GETDATE());

  SET @IdWebhook = SCOPE_IDENTITY();
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Tienda_ListarProductos
  @IdCategoria INT = NULL,
  @Texto VARCHAR(100) = NULL,     -- busca en nombre/descr
  @Page INT = 1,
  @PageSize INT = 20
AS
BEGIN
  SET NOCOUNT ON;

  WITH Base AS (
    SELECT
      p.IdProducto, p.Nombre, p.Precio, p.Descripcion,
      s.StockDisponible,
      (SELECT TOP 1 Url
         FROM ProductoImagen i
         WHERE i.IdProducto = p.IdProducto AND i.EsPrincipal = 1
         ORDER BY i.Orden ASC, i.IdProductoImagen ASC) AS ImagenPrincipal,
      ROW_NUMBER() OVER (ORDER BY p.Nombre ASC) AS rn
    FROM v_StockProducto s
    JOIN Producto p ON p.IdProducto = s.IdProducto
    WHERE (@Texto IS NULL OR (p.Nombre LIKE '%' + @Texto + '%' OR p.Descripcion LIKE '%' + @Texto + '%'))
      AND (
        @IdCategoria IS NULL
        OR EXISTS (SELECT 1 FROM ProductoCategoria pc WHERE pc.IdProducto = p.IdProducto AND pc.IdCategoria = @IdCategoria)
      )
  )
  SELECT IdProducto, Nombre, Precio, Descripcion, StockDisponible, ImagenPrincipal
  FROM Base
  WHERE rn BETWEEN ((@Page-1)*@PageSize + 1) AND (@Page*@PageSize);


  SELECT COUNT(1) AS Total
  FROM v_StockProducto s
  JOIN Producto p ON p.IdProducto = s.IdProducto
  WHERE (@Texto IS NULL OR (p.Nombre LIKE '%' + @Texto + '%' OR p.Descripcion LIKE '%' + @Texto + '%'))
    AND (
      @IdCategoria IS NULL
      OR EXISTS (SELECT 1 FROM ProductoCategoria pc WHERE pc.IdProducto = p.IdProducto AND pc.IdCategoria = @IdCategoria)
    );
END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Tienda_ObtenerProducto
  @IdProducto INT
AS
BEGIN
  SET NOCOUNT ON;

  
  SELECT p.*, s.StockDisponible
  FROM Producto p
  LEFT JOIN v_StockProducto s ON s.IdProducto = p.IdProducto
  WHERE p.IdProducto = @IdProducto;

 
  SELECT i.*
  FROM ProductoImagen i
  WHERE i.IdProducto = @IdProducto
  ORDER BY i.EsPrincipal DESC, i.Orden ASC, i.IdProductoImagen ASC;

  
  SELECT c.*
  FROM Categoria c
  JOIN ProductoCategoria pc ON pc.IdCategoria = c.IdCategoria
  WHERE pc.IdProducto = @IdProducto
  ORDER BY c.Orden, c.Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tienda_GuardarDireccionCliente
  @IdCliente INT,
  @Receptor VARCHAR(120),
  @Telefono VARCHAR(20) = NULL,
  @Direccion VARCHAR(200),
  @Referencia VARCHAR(200) = NULL,
  @Distrito VARCHAR(80) = NULL,
  @Provincia VARCHAR(80) = NULL,
  @Departamento VARCHAR(80) = NULL,
  @Principal BIT = 0,
  @IdDireccion INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  BEGIN TRY
    BEGIN TRAN;

    IF @Principal = 1
    BEGIN
      UPDATE DireccionCliente
        SET Principal = 0, FechaActualizacion = GETDATE()
      WHERE IdCliente = @IdCliente AND Principal = 1;
    END

    INSERT INTO DireccionCliente(
      IdCliente, Receptor, Telefono, Direccion, Referencia,
      Distrito, Provincia, Departamento, Principal,
      FechaRegistro, FechaActualizacion
    )
    VALUES(
      @IdCliente, @Receptor, @Telefono, @Direccion, @Referencia,
      @Distrito, @Provincia, @Departamento, @Principal,
      GETDATE(), GETDATE()
    );

    SET @IdDireccion = SCOPE_IDENTITY();

    COMMIT;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
  END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tienda_AsignarEnvioAOrden
  @IdOrden INT,
  @IdDireccion INT = NULL,         
  @MetodoEnvio VARCHAR(15),        
  @Costo DECIMAL(10,2) = 0,
  @Courier VARCHAR(50) = NULL,
  @TrackingCode VARCHAR(60) = NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM OrdenEnvio WHERE IdOrden = @IdOrden)
  BEGIN
    UPDATE OrdenEnvio
      SET IdDireccion = @IdDireccion,
          MetodoEnvio = @MetodoEnvio,
          Costo = @Costo,
          Courier = @Courier,
          TrackingCode = @TrackingCode,
          FechaActualizacion = GETDATE()
    WHERE IdOrden = @IdOrden;
  END
  ELSE
  BEGIN
    INSERT INTO OrdenEnvio (IdOrden, IdDireccion, MetodoEnvio, Costo, Courier, TrackingCode, EstadoEnvio, FechaRegistro, FechaActualizacion)
    VALUES (@IdOrden, @IdDireccion, @MetodoEnvio, @Costo, @Courier, @TrackingCode, 'PENDIENTE', GETDATE(), GETDATE());
  END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tienda_ActualizarEstadoEnvio
  @IdOrden INT,
  @EstadoEnvio VARCHAR(20)         
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE OrdenEnvio
    SET EstadoEnvio = @EstadoEnvio,
        FechaActualizacion = GETDATE()
  WHERE IdOrden = @IdOrden;
END
GO

