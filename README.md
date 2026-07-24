# Carta Declaratoria - Order Express (App Web)

Aplicación ASP.NET Core para capturar la "Carta Declaratoria (Remesa)" en línea,
guardarla en SQL Server, y generar/descargar el PDF desde cualquier dispositivo
(solo empleados con login).

## Requisitos previos en tu servidor
1. **.NET 8 SDK** (para compilar) y **.NET 8 Hosting Bundle** (para que IIS pueda correr apps ASP.NET Core).
   Descárgalos de https://dotnet.microsoft.com/download/dotnet/8.0
2. **IIS** con el módulo **ASP.NET Core Module v2** instalado (viene con el Hosting Bundle).
3. **SQL Server 2019** accesible desde el servidor (puede ser el mismo servidor).
4. Visual Studio 2022 (Community está bien) en tu máquina de desarrollo, con la carga de trabajo
   "Desarrollo web y ASP.NET".

## Primeros pasos (en tu máquina de desarrollo)

1. Abre este proyecto en Visual Studio (archivo `CartaDeclaratoriaApp.csproj`).
2. Edita `appsettings.json` y pon tu cadena de conexión real, por ejemplo:
   ```
   Server=NOMBRE_SERVIDOR\\INSTANCIA;Database=CartaDeclaratoriaDB;Trusted_Connection=True;TrustServerCertificate=True;
   ```
   o si usas usuario/contraseña de SQL:
   ```
   Server=NOMBRE_SERVIDOR;Database=CartaDeclaratoriaDB;User Id=usuario;Password=tu_password;TrustServerCertificate=True;
   ```
3. Abre la "Consola del Administrador de Paquetes" (Package Manager Console) y corre:
   ```
   Add-Migration InicialCartaDeclaratoria
   Update-Database
   ```
   Esto crea la base de datos y todas las tablas automáticamente (incluidas las de login/Identity).
   *(El archivo `sql/schema.sql` queda solo como referencia si prefieres crearla a mano.)*
4. Da de alta al primer usuario/empleado: corre el proyecto (F5), ve a `/Identity/Account/Register`,
   crea la cuenta del primer empleado. Luego puedes desactivar el registro público
   (ver sección "Seguridad" abajo) y dar de alta al resto de empleados tú mismo, o dejarlo abierto
   solo en tu red interna.

## Publicar en tu IIS (Windows Server)

1. En Visual Studio: clic derecho sobre el proyecto → **Publicar** → **Carpeta**.
2. Copia la carpeta publicada a tu servidor, por ejemplo a `C:\inetpub\wwwroot\CartaDeclaratoria`.
3. En **Administrador de IIS**:
   - Crea un nuevo **Sitio** (o Aplicación dentro de un sitio existente) apuntando a esa carpeta.
   - Asigna el **Pool de Aplicaciones** con "No Managed Code" (.NET Core no usa el CLR de IIS clásico).
   - Si quieres que sea accesible por HTTPS desde internet, agrega un binding 443 con tu certificado SSL
     (recomendado usar Let's Encrypt o un certificado comprado, nunca dejarlo solo en HTTP).
4. Abre el puerto correspondiente en el Firewall de Windows si aún no está abierto.
5. Entra desde tu celular/otra red a `https://tu-dominio-o-ip/` para confirmar que carga.

## Seguridad recomendada (importante, dado que maneja datos sensibles tipo CURP/domicilio)

- **Desactiva el registro público** de usuarios una vez creadas las cuentas de tus empleados
  (edita `Program.cs` o usa `[Authorize(Roles = "Admin")]` para restringir quién da de alta usuarios).
- Usa **HTTPS obligatorio** (ya está `UseHsts()` y `UseHttpsRedirection()` en el `Program.cs`).
- Considera cifrar en la base de datos los campos sensibles (CURP, domicilio) si tu área de
  cumplimiento lo requiere.
- Haz respaldos periódicos de `CartaDeclaratoriaDB` (esto es responsabilidad de tu DBA/SQL Server Agent).
- Guarda un registro (log) de quién capturó y quién descargó cada carta — el modelo ya guarda
  `CapturadoPorUsuarioId`; puedes ampliar esto con una tabla de auditoría si lo necesitas.

## Notas sobre el PDF generado

- El PDF se genera "al vuelo" con la librería **QuestPDF**, replicando el layout de tu formato
  original (encabezado azul, secciones "Datos del Beneficiario" / "Datos del Girador", y el texto
  legal de los artículos 139 Quáter y 400 Bis).
- Puedes agregar tu logo real reemplazando el bloque de encabezado en `Services/PdfService.cs`
  con una imagen (`.Image("wwwroot/images/logo.png")`), en vez de solo texto.
- QuestPDF tiene licencia **Community gratuita** si Order Express factura menos de cierto umbral anual;
  revisa https://www.questpdf.com/license/ para confirmar que aplica a tu empresa.

## Estructura del proyecto

```
CartaDeclaratoriaApp/
├── Controllers/CartaDeclaratoriaController.cs   -> lógica de guardar/listar/descargar
├── Models/CartaDeclaratoria.cs                  -> campos del formulario
├── Data/ApplicationDbContext.cs                 -> conexión EF Core + Identity
├── Services/PdfService.cs                       -> generación del PDF
├── Views/CartaDeclaratoria/                     -> formulario y listado (HTML)
├── Views/Shared/_Layout.cshtml                  -> plantilla visual responsive
├── sql/schema.sql                               -> tabla de referencia (opcional)
└── appsettings.json                             -> cadena de conexión a SQL Server
```

## Lo que falta agregar (según cómo lo uses)

Este es un **proyecto base funcional**, no un producto terminado. Si el volumen de uso crece,
considera:
- Roles de usuario (ej. "Captura" vs "Supervisor" que autoriza).
- Búsqueda/filtro en el listado por folio, fecha o beneficiario.
- Firma digital real (capturada con el dedo/mouse) en vez de solo el nombre en texto.
- Exportar reportes en Excel para tus auditorías de PLD (Prevención de Lavado de Dinero).
