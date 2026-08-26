# Transacciones en Bases de Datos con .NET 8.0 y C#

## Índice
1. [Conceptos Fundamentales](#conceptos-fundamentales)
2. [ACID en Transacciones](#acid-en-transacciones)
3. [ADO.NET](#ado.net)
4. [Niveles de Aislamiento](#niveles-de-aislamiento)
5. [Patrones y Buenas Prácticas](#patrones-y-buenas-prácticas)
6. [Manejo de Errores en ADO.NET](#manejo-de-errores-en-adonet)
7. [Ejemplos Prácticos con ADO.NET](#ejemplos-prácticos-con-adonet)
8. [Configuración](#configuración-en-appsettingsjson)

---

## Conceptos Fundamentales

### ¿Qué es una Transacción?

Una **transacción** es una unidad de trabajo que agrupa múltiples operaciones en la base de datos. Estas operaciones se ejecutan como una unidad atómica: **todas se ejecutan o ninguna se ejecuta**.

### Características Clave

- **Atomicidad**: Todo o nada
- **Coherencia**: Estado válido antes y después
- **Aislamiento**: Las transacciones no interfieren entre sí
- **Durabilidad**: Los datos confirmados persisten

---

## ACID en Transacciones

### A - Atomicidad (Atomicity)

Garantiza que una transacción se completa totalmente o se revierte completamente.

```csharp
using (var transaction = context.Database.BeginTransaction())
{
    try
    {
        context.Users.Add(new User { Name = "Juan" });
        context.SaveChanges();
        
        context.Accounts.Add(new Account { UserId = 1, Balance = 1000 });
        context.SaveChanges();
        
        transaction.Commit(); // Todo se confirma
    }
    catch
    {
        transaction.Rollback(); // Todo se revierte
        throw;
    }
}
```

### C - Coherencia (Consistency)

Asegura que la base de datos pase de un estado válido a otro estado válido.

```csharp
// Definir restricciones en el modelo
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; } // Validación
}
```

### I - Aislamiento (Isolation)

Las transacciones simultáneas no se interfieren mutuamente. Ver [Niveles de Aislamiento](#niveles-de-aislamiento).

### D - Durabilidad (Durability)

Los cambios confirmados son permanentes incluso ante fallos del sistema.

---

## ADO.NET

En todos los ejemplos se usa **una sola cadena de conexión**:

```csharp
private const string CadenaConexion =
    @"Server=(localdb)\mssqllocaldb;Database=BancoDAS;Integrated Security=true;TrustServerCertificate=true;";
```

### Transacciones Básicas con SqlConnection

La forma más simple de trabajar con transacciones en ADO.NET:

```csharp
public void TransferenciaBasica(int desde, int hacia, decimal monto)
{
    using (SqlConnection connection = new SqlConnection(CadenaConexion))
    {
        connection.Open();
        
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                // Obtener saldo de la cuenta origen
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "SELECT Balance FROM Cuentas WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", desde);
                    
                    decimal saldoOrigen = (decimal)command.ExecuteScalar();
                    
                    if (saldoOrigen < monto)
                        throw new InvalidOperationException("Fondos insuficientes");
                }
                
                // Restar de cuenta origen
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @id";
                    command.Parameters.AddWithValue("@monto", monto);
                    command.Parameters.AddWithValue("@id", desde);
                    
                    command.ExecuteNonQuery();
                }
                
                // Sumar a cuenta destino
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "UPDATE Cuentas SET Balance = Balance + @monto WHERE Id = @id";
                    command.Parameters.AddWithValue("@monto", monto);
                    command.Parameters.AddWithValue("@id", hacia);
                    
                    command.ExecuteNonQuery();
                }
                
                // Confirmar transacción
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Revertir en caso de error
                transaction.Rollback();
                Console.WriteLine($"Error en transferencia: {ex.Message}");
                throw;
            }
        }
    }
}
```

### Transacciones con Parámetros SQL

Mejor práctica para evitar inyección SQL:

```csharp
public void TransferenciaConParametros(int desde, int hacia, decimal monto)
{
    using (SqlConnection connection = new SqlConnection(CadenaConexion))
    {
        connection.Open();
        
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                // Comando 1: Actualizar cuenta origen
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @desde", 
                    connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@monto", monto);
                    cmd.Parameters.AddWithValue("@desde", desde);
                    cmd.ExecuteNonQuery();
                }
                
                // Comando 2: Actualizar cuenta destino
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Cuentas SET Balance = Balance + @monto WHERE Id = @hacia", 
                    connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@monto", monto);
                    cmd.Parameters.AddWithValue("@hacia", hacia);
                    cmd.ExecuteNonQuery();
                }
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
```

### SavePoints (Puntos de Guardado) en ADO.NET

Permite revertir parcialmente una transacción:

```csharp
public void ProcesarConSavepoints()
{
    using (SqlConnection connection = new SqlConnection(CadenaConexion))
    {
        connection.Open();
        
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                // Operación 1: Insertar usuario
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (Name) VALUES (@name)", 
                    connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@name", "Juan");
                    cmd.ExecuteNonQuery();
                }
                
                // Crear punto de guardado
                transaction.Save("SP1");
                
                // Operación 2: Insertar orden (puede fallar)
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Orders (UserId, Total) VALUES (@userId, @total)", 
                    connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@userId", 1);
                    cmd.Parameters.AddWithValue("@total", 100);
                    cmd.ExecuteNonQuery();
                }
                
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Volver al punto de guardado
                try
                {
                    transaction.Rollback("SP1");
                    Console.WriteLine("Revertido a SP1");
                }
                catch
                {
                    transaction.Rollback();
                    Console.WriteLine("Transacción completamente revertida");
                }
                throw;
            }
        }
    }
}
```

---

## Niveles de Aislamiento

Controlados por `IsolationLevel`:

### 1. ReadUncommitted (Más Bajo)
- Permite lecturas sucias (Dirty Reads)
- Máximo rendimiento, mínima consistencia

```csharp
using (var transaction = context.Database.BeginTransaction(
    System.Data.IsolationLevel.ReadUncommitted))
{
    // Operaciones...
}
```

### 2. ReadCommitted (Por Defecto)
- No permite lecturas sucias
- Permite lecturas fantasma

```csharp
using (var transaction = context.Database.BeginTransaction(
    System.Data.IsolationLevel.ReadCommitted))
{
    // Operaciones...
}
```

### 3. RepeatableRead
- No permite lecturas sucias ni lecturas no repetibles
- Permite lecturas fantasma

```csharp
using (var transaction = context.Database.BeginTransaction(
    System.Data.IsolationLevel.RepeatableRead))
{
    // Operaciones...
}
```

### 4. Serializable (Más Alto)
- Previene todas las anomalías
- Máxima consistencia, mínimo rendimiento

```csharp
using (var transaction = context.Database.BeginTransaction(
    System.Data.IsolationLevel.Serializable))
{
    // Operaciones...
}
```

### Tabla Comparativa

| Nivel | Dirty Reads | Non-Repeatable Reads | Phantom Reads |
|-------|------------|---------------------|---------------|
| ReadUncommitted | ✓ | ✓ | ✓ |
| ReadCommitted | ✗ | ✓ | ✓ |
| RepeatableRead | ✗ | ✗ | ✓ |
| Serializable | ✗ | ✗ | ✗ |

---

## Patrones y Buenas Prácticas en ADO.NET

### Pattern 1: Connection Factory

Centralizar la creación de conexiones:

```csharp
public interface IConnectionFactory
{
    SqlConnection CreateConnection();
}

public class SqlConnectionFactory : IConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(CadenaConexion);
    }
}
```

### Pattern 2: Repository Pattern con ADO.NET

```csharp
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class AccountRepository : IRepository<Account>
{
    private readonly IConnectionFactory _connectionFactory;
    
    public AccountRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public Account GetById(int id)
    {
        using (SqlConnection connection = _connectionFactory.CreateConnection())
        {
            connection.Open();
            
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Id, Balance, AccountNumber FROM Cuentas WHERE Id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Account
                        {
                            Id = (int)reader["Id"],
                            Balance = (decimal)reader["Balance"],
                            AccountNumber = reader["AccountNumber"].ToString()
                        };
                    }
                }
            }
        }
        return null;
    }
    
    public void Transferir(int from, int to, decimal amount, SqlTransaction transaction = null)
    {
        using (SqlConnection connection = _connectionFactory.CreateConnection())
        {
            connection.Open();
            
            SqlTransaction localTransaction = null;
            
            try
            {
                localTransaction = transaction ?? connection.BeginTransaction();
                
                // Validar fondos
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT Balance FROM Cuentas WHERE Id = @id", connection, localTransaction))
                {
                    cmd.Parameters.AddWithValue("@id", from);
                    decimal balance = (decimal)cmd.ExecuteScalar();
                    
                    if (balance < amount)
                        throw new InvalidOperationException("Saldo insuficiente");
                }
                
                // Realizar transferencia
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Cuentas SET Balance = Balance - @amount WHERE Id = @id", 
                    connection, localTransaction))
                {
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@id", from);
                    cmd.ExecuteNonQuery();
                }
                
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Cuentas SET Balance = Balance + @amount WHERE Id = @id", 
                    connection, localTransaction))
                {
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@id", to);
                    cmd.ExecuteNonQuery();
                }
                
                if (transaction == null)
                {
                    localTransaction.Commit();
                }
            }
            catch
            {
                if (transaction == null && localTransaction != null)
                {
                    localTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    localTransaction?.Dispose();
                }
            }
        }
    }
}
```

---

## Manejo de Errores en ADO.NET

### Detectar Errores de SQL Server

```csharp
public void ManejarErroresSQLServer(int desde, int hacia, decimal monto)
{
    using (SqlConnection connection = new SqlConnection(CadenaConexion))
    {
        try
        {
            connection.Open();
            
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @desde", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@desde", desde);
                        cmd.ExecuteNonQuery();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Cuentas SET Balance = Balance + @monto WHERE Id = @hacia", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@hacia", hacia);
                        cmd.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    
                    // Analizar número de error SQL Server
                    foreach (SqlError error in ex.Errors)
                    {
                        Console.WriteLine($"Error SQL: {error.Number} - {error.Message}");
                        
                        // 1205 = Deadlock
                        if (error.Number == 1205)
                        {
                            Console.WriteLine("Ocurrió un deadlock, reintentar operación");
                        }
                        // 2627 = Violación de restricción única
                        else if (error.Number == 2627)
                        {
                            Console.WriteLine("Restricción única violada");
                        }
                        // 547 = Violación de restricción de clave foránea
                        else if (error.Number == 547)
                        {
                            Console.WriteLine("Violación de clave foránea");
                        }
                    }
                    throw;
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            throw;
        }
    }
}
```

### Reintentos con Backoff Exponencial

```csharp
public T EjecutarConReintentos<T>(
    Func<T> operacion, 
    int maxReintentos = 3,
    int delayMs = 100)
{
    for (int intento = 0; intento < maxReintentos; intento++)
    {
        try
        {
            return operacion();
        }
        catch (SqlException ex) when (
            (ex.Number == 1205 || // Deadlock
             ex.Number == -2 ||    // Timeout
             ex.Number == 64) &&   // Communication link failure
            intento < maxReintentos - 1)
        {
            int espera = (int)Math.Pow(2, intento) * delayMs;
            Console.WriteLine($"Intento {intento + 1} falló. Esperando {espera}ms...");
            Thread.Sleep(espera);
        }
    }
    
    throw new InvalidOperationException($"Operación falló después de {maxReintentos} intentos");
}

// Uso
EjecutarConReintentos(() =>
{
    using (SqlConnection connection = new SqlConnection(CadenaConexion))
    {
        connection.Open();
        
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            // Operaciones...
            transaction.Commit();
            return true;
        }
    }
});
```

### Manejo de Deadlocks Específico

```csharp
public void ManejarDeadlock(Action operacion, int maxReintentos = 3)
{
    for (int intento = 0; intento < maxReintentos; intento++)
    {
        try
        {
            operacion();
            return;
        }
        catch (SqlException ex) when (ex.Number == 1205 && intento < maxReintentos - 1)
        {
            // Esperar progresivamente más tiempo
            int tiempoEspera = (intento + 1) * 1000;
            Console.WriteLine($"Deadlock detectado. Reintentando en {tiempoEspera}ms (Intento {intento + 1}/{maxReintentos})");
            Thread.Sleep(tiempoEspera);
        }
    }
}
```

### Logging de Errores de Transacciones

```csharp
public class TransactionLogger
{
    public void LogTransactionError(string operacion, Exception ex)
    {
        using (SqlConnection connection = new SqlConnection(CadenaConexion))
        {
            try
            {
                connection.Open();
                
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO ErrorLog (Operacion, Mensaje, StackTrace, Timestamp) VALUES (@operacion, @mensaje, @stackTrace, @timestamp)", 
                    connection))
                {
                    cmd.Parameters.AddWithValue("@operacion", operacion);
                    cmd.Parameters.AddWithValue("@mensaje", ex.Message);
                    cmd.Parameters.AddWithValue("@stackTrace", ex.StackTrace ?? "");
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error al registrar error: {logEx.Message}");
            }
        }
    }
}
```

---

## Ejemplos Prácticos con ADO.NET

### Ejemplo 1: Transferencia Bancaria Completa

```csharp
public class BankTransactionService
{
    public void TransferirSaldo(int cuentaOrigenId, int cuentaDestinoId, decimal monto)
    {
        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0");
        
        using (SqlConnection connection = new SqlConnection(CadenaConexion))
        {
            connection.Open();
            
            using (SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    // Validar fondos y obtener saldo
                    decimal saldoOrigen;
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT Balance FROM Cuentas WHERE Id = @id", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", cuentaOrigenId);
                        saldoOrigen = (decimal)cmd.ExecuteScalar() ?? 0;
                    }
                    
                    if (saldoOrigen < monto)
                        throw new InvalidOperationException("Saldo insuficiente");
                    
                    // Restar de cuenta origen
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @id", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@id", cuentaOrigenId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Sumar a cuenta destino
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Cuentas SET Balance = Balance + @monto WHERE Id = @id", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@id", cuentaDestinoId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Registrar movimiento origen
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Movimientos (CuentaId, Tipo, Monto, Fecha, Descripcion) VALUES (@cuentaId, @tipo, @monto, @fecha, @desc)", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuentaId", cuentaOrigenId);
                        cmd.Parameters.AddWithValue("@tipo", "Débito");
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@desc", $"Transferencia a cuenta {cuentaDestinoId}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Registrar movimiento destino
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Movimientos (CuentaId, Tipo, Monto, Fecha, Descripcion) VALUES (@cuentaId, @tipo, @monto, @fecha, @desc)", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@cuentaId", cuentaDestinoId);
                        cmd.Parameters.AddWithValue("@tipo", "Crédito");
                        cmd.Parameters.AddWithValue("@monto", monto);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@desc", $"Transferencia desde cuenta {cuentaOrigenId}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error en transferencia: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
```

### Ejemplo 2: Procesamiento de Órdenes con Validación de Stock

```csharp
public class OrderProcessingService
{
    public void ProcessOrder(int orderId, List<(int ProductoId, int Cantidad)> items)
    {
        using (SqlConnection connection = new SqlConnection(CadenaConexion))
        {
            connection.Open();
            
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Validar inventario para todos los productos
                    foreach (var (productoId, cantidad) in items)
                    {
                        using (SqlCommand cmd = new SqlCommand(
                            "SELECT Stock FROM Productos WHERE Id = @id", 
                            connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", productoId);
                            int stock = (int)cmd.ExecuteScalar();
                            
                            if (stock < cantidad)
                                throw new InvalidOperationException($"Stock insuficiente para producto {productoId}");
                        }
                    }
                    
                    // 2. Reducir stock
                    foreach (var (productoId, cantidad) in items)
                    {
                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Productos SET Stock = Stock - @cantidad WHERE Id = @id", 
                            connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            cmd.Parameters.AddWithValue("@id", productoId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    // 3. Actualizar estado de orden
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Orders SET Estado = @estado, FechaProcesamiento = @fecha WHERE Id = @id", 
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@estado", "Procesada");
                        cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@id", orderId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // 4. Registrar movimientos de inventario
                    foreach (var (productoId, cantidad) in items)
                    {
                        using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO MovimientosInventario (ProductoId, OrderId, Cantidad, Tipo, Fecha) VALUES (@prodId, @orderId, @cant, @tipo, @fecha)", 
                            connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@prodId", productoId);
                            cmd.Parameters.AddWithValue("@orderId", orderId);
                            cmd.Parameters.AddWithValue("@cant", cantidad);
                            cmd.Parameters.AddWithValue("@tipo", "Salida");
                            cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
```

### Ejemplo 3: Batch Processing con Lotes

```csharp
public class BatchProcessingService
{
    public void ProcessUserBatch(IEnumerable<int> userIds, decimal incrementoBalance)
    {
        const int batchSize = 100;
        var userIdList = userIds.ToList();
        
        for (int i = 0; i < userIdList.Count; i += batchSize)
        {
            var batch = userIdList.Skip(i).Take(batchSize).ToList();
            
            using (SqlConnection connection = new SqlConnection(CadenaConexion))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var userId in batch)
                        {
                            // Obtener usuario
                            using (SqlCommand cmd = new SqlCommand(
                                "SELECT Balance FROM Users WHERE Id = @id", 
                                connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@id", userId);
                                var result = cmd.ExecuteScalar();
                                
                                if (result != null)
                                {
                                    decimal currentBalance = (decimal)result;
                                    decimal newBalance = currentBalance + incrementoBalance;
                                    
                                    // Actualizar balance
                                    using (SqlCommand updateCmd = new SqlCommand(
                                        "UPDATE Users SET Balance = @newBalance, UltimaActualizacion = @fecha WHERE Id = @id", 
                                        connection, transaction))
                                    {
                                        updateCmd.Parameters.AddWithValue("@newBalance", newBalance);
                                        updateCmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                                        updateCmd.Parameters.AddWithValue("@id", userId);
                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        
                        transaction.Commit();
                        Console.WriteLine($"Lote procesado: {batch.Count} registros");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error en lote: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
```

---

## Configuración en appsettings.json

La cadena es la misma `CadenaConexion` de los ejemplos:

```json
{
  "ConnectionStrings": {
    "CadenaConexion": "Server=(localdb)\\mssqllocaldb;Database=BancoDAS;Integrated Security=true;TrustServerCertificate=true;"
  },
  "SqlConfiguration": {
    "CommandTimeout": 30,
    "ConnectionTimeout": 15,
    "MaxRetries": 3,
    "RetryDelayMs": 100
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## Inyección de Dependencias

```csharp
// Program.cs
var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Services.AddScoped<IConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();
builder.Services.AddScoped<BankTransactionService>();
builder.Services.AddScoped<OrderProcessingService>();
builder.Services.AddScoped<BatchProcessingService>();

var app = builder.Build();

app.MapPost("/api/transfer", (TransferRequest req, BankTransactionService service) =>
{
    service.TransferirSaldo(req.From, req.To, req.Amount);
    return Results.Ok();
});

app.Run();
```

### Clase de Configuración

```csharp
public class DatabaseConfiguration
{
    public string CadenaConexion { get; set; }
    public int CommandTimeout { get; set; }
    public int ConnectionTimeout { get; set; }
    public int MaxRetries { get; set; }
    public int RetryDelayMs { get; set; }
}
```

## Patrón de Inicialización de BD

```csharp
public class DatabaseInitializer
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<DatabaseInitializer> _logger;
    
    public DatabaseInitializer(IConnectionFactory factory, ILogger<DatabaseInitializer> logger)
    {
        _connectionFactory = factory;
        _logger = logger;
    }
    
    public void InitializeDatabase()
    {
        using (SqlConnection connection = _connectionFactory.CreateConnection())
        {
            try
            {
                connection.Open();
                _logger.LogInformation("Conexión a BD establecida correctamente");
                
                CreateTablesIfNotExist(connection);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error al conectar a la BD: {ex.Message}");
                throw;
            }
        }
    }
    
    private void CreateTablesIfNotExist(SqlConnection connection)
    {
        string createTablesScript = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cuentas')
            BEGIN
                CREATE TABLE Cuentas (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    AccountNumber NVARCHAR(50) NOT NULL,
                    Balance DECIMAL(18,2) NOT NULL,
                    CreatedDate DATETIME DEFAULT GETUTCDATE()
                )
            END
            
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movimientos')
            BEGIN
                CREATE TABLE Movimientos (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    CuentaId INT NOT NULL,
                    Tipo NVARCHAR(50) NOT NULL,
                    Monto DECIMAL(18,2) NOT NULL,
                    Fecha DATETIME DEFAULT GETUTCDATE(),
                    Descripcion NVARCHAR(500),
                    FOREIGN KEY (CuentaId) REFERENCES Cuentas(Id)
                )
            END
        ";
        
        using (SqlCommand cmd = new SqlCommand(createTablesScript, connection))
        {
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

## Referencias

- [ADO.NET SqlClient Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient)
- [SqlTransaction Class](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqltransaction)
- [SqlConnection Class](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection)
- [SQL Server Isolation Levels](https://learn.microsoft.com/en-us/sql/t-sql/statements/set-transaction-isolation-level-transact-sql)
- [SQL Server Error Numbers](https://learn.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors)