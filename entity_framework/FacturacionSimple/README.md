# Facturación simple — ejemplo Entity Framework (3 capas)

Ejemplo que sigue la guía `../guia-solucion-tres-capas-ef.md`.

## Estructura

| Proyecto | Rol |
|----------|-----|
| `FacturacionSimple.Modelo` | Entidades (`Producto`, `Factura`, `DetalleFactura`), `FacturacionDbContext`, acceso a datos |
| `FacturacionSimple.Controladora` | Validaciones y orquestación de negocio |
| `FacturacionSimple.Vista` | Windows Forms (ABM productos, emitir y consultar facturas) |

## Tecnologías

- .NET 8
- EF Core **9.0.0**
- LocalDB: `Server=(localdb)\MSSQLLocalDB;Database=FacturacionSimpleDb;...`

## Cómo ejecutar

1. Abrir `FacturacionSimple.sln` en Visual Studio (o usar CLI).
2. Proyecto de inicio: `FacturacionSimple.Vista`.
3. Si aún no aplicaste la migración:

```powershell
dotnet ef database update --project FacturacionSimple.Modelo --startup-project FacturacionSimple.Vista
```

4. Ejecutar (F5) o:

```powershell
dotnet run --project FacturacionSimple.Vista
```

## Funcionalidad

- **Productos**: alta, modificación, baja lógica, eliminación física (solo si no está en facturas), búsqueda.
- **Emitir factura**: cabecera (cliente) + líneas; el precio se copia al detalle; grabación en transacción.
- **Consultar facturas**: filtro por fechas/cliente; detalle en solo lectura.

## Dominio

- `Producto`: Codigo (único), Nombre, Precio, Activo
- `Factura`: Numero, Fecha, ClienteNombre, ClienteDocumento, Total
- `DetalleFactura`: ProductoId, Cantidad, PrecioUnitario, Subtotal
