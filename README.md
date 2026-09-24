# GolfCart

Tienda sencilla de bolas de golf usadas, creada con ASP.NET Core MVC (.NET 10) y SQLite.

## Primera versión

- Catálogo de lotes por marca y calidad: Mint (A), B, C y D.
- Filtro por marca y calidad.
- Carrito por sesión.
- Confirmación de pedido de prueba que descuenta stock de manera transaccional.
- Área de inventario para crear y modificar lotes, precio, visibilidad y stock.
- Acceso de administrador protegido por ASP.NET Core Identity y el rol `Admin`.

## Arranque local

```bash
dotnet run
```

La base SQLite se crea automáticamente en `Data/golfcart.db` e incluye cuatro lotes de ejemplo. Para restablecer los datos locales, elimina ese archivo mientras la aplicación está detenida.

## Crear el primer administrador

No hay registro público. Antes del primer arranque, guarda las credenciales únicamente en User Secrets (no se guardan en Git):

```bash
dotnet user-secrets set "BootstrapAdmin:Email" "tu-correo@ejemplo.com"
dotnet user-secrets set "BootstrapAdmin:Password" "UnaClaveLarga!2026"
dotnet run
```

La contraseña debe tener un mínimo de 12 caracteres, mayúscula, minúscula, número y símbolo. Tras crear la cuenta, elimina el secreto de contraseña con `dotnet user-secrets remove "BootstrapAdmin:Password"`. En Azure, las mismas claves se configurarán desde Key Vault como `BootstrapAdmin__Email` y `BootstrapAdmin__Password` durante el aprovisionamiento.

## Siguiente decisión recomendada

Antes de publicar: decidir proveedor de pago y datos de envío. Para una primera puesta en producción sencilla, Azure App Service con Azure Database for MySQL es una evolución natural; SQLite queda bien para desarrollo y un piloto de una sola instancia.
