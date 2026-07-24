using CartaDeclaratoriaApp.Data;
using CartaDeclaratoriaApp.Models;
using CartaDeclaratoriaApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartaDeclaratoriaApp.Controllers
{
    public class CartaDeclaratoriaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PdfService _pdfService;

        public CartaDeclaratoriaController(ApplicationDbContext db, PdfService pdfService)
        {
            _db = db;
            _pdfService = pdfService;
        }

        // ===========================
        // LISTADO
        // ===========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var registros = await _db.CartasDeclaratorias
                .OrderByDescending(x => x.FechaCaptura)
                .ToListAsync();

            return View(registros);
        }


        // ===========================
        // FORMULARIO
        // ===========================
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CartaDeclaratoria());
        }


       // ===========================
// GUARDAR
// ===========================
[HttpPost]
//[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(CartaDeclaratoria modelo)
{
    Console.WriteLine("=================================");
    Console.WriteLine("FORMULARIO RECIBIDO");
    Console.WriteLine("=================================");

    foreach (var item in Request.Form)
    {
        Console.WriteLine($"{item.Key} = {item.Value}");
    }

    Console.WriteLine("====================================");
    Console.WriteLine("ENTRÓ AL MÉTODO CREATE");
    Console.WriteLine("====================================");

    if (!ModelState.IsValid)
    {
        Console.WriteLine("EL MODELO NO ES VÁLIDO");

        foreach (var item in ModelState)
        {
            foreach (var error in item.Value.Errors)
            {
                Console.WriteLine($"{item.Key} -> {error.ErrorMessage}");
            }
        }

        return View(modelo);
    }

    Console.WriteLine("MODELO CORRECTO");

    Console.WriteLine("=================================");
    Console.WriteLine("VALORES DEL MODELO");
    Console.WriteLine("=================================");
    Console.WriteLine($"Nombre: {modelo.BeneficiarioNombreCompleto}");
    Console.WriteLine($"CURP: {modelo.BeneficiarioCurp}");
    Console.WriteLine($"Folio: {modelo.RemesaFolio}");
    Console.WriteLine($"Monto: {modelo.Monto}");

    modelo.CapturadoPorUsuarioId = User.Identity?.Name;
    modelo.FechaCaptura = DateTime.Now;

    Console.WriteLine("INSERTANDO EN SQL...");

    _db.CartasDeclaratorias.Add(modelo);

    await _db.SaveChangesAsync();

    Console.WriteLine($"REGISTRO GUARDADO. ID = {modelo.Id}");

    return RedirectToAction(nameof(DescargarPdf), new { id = modelo.Id });
}


        // ===========================
        // DETALLE
        // ===========================
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var registro = await _db.CartasDeclaratorias
                .FindAsync(id);

            if (registro == null)
                return NotFound();

            return View(registro);
        }


        // ===========================
        // PDF
        // ===========================
        [HttpGet]
        public async Task<IActionResult> DescargarPdf(int id)
        {
            Console.WriteLine($"GENERANDO PDF DEL ID {id}");

            var registro = await _db.CartasDeclaratorias
                .FindAsync(id);

            if (registro == null)
                return NotFound();


            byte[] pdfBytes = _pdfService.GenerarCartaDeclaratoriaPdf(registro);


            string nombreArchivo = 
                $"CartaDeclaratoria_{registro.RemesaFolio}_{registro.Id}.pdf";


            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
    }
}