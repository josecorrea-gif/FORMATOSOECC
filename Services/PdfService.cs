using CartaDeclaratoriaApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace CartaDeclaratoriaApp.Services
{
    public class PdfService
    {
        public PdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerarCartaDeclaratoriaPdf(CartaDeclaratoria c)
        {
            var colorEmpresa = Color.FromHex("#292667");
            var colorBarraTitulo = Color.FromHex("#292667");
            var colorBorde = Color.FromHex("#8F8D8D");
            var colorLinea = Color.FromHex("#000000");

            const string FuenteBase = Fonts.Arial;
            const float TamanoTextoBase = 8f;

            // Helper para campos cortos de UNA sola línea
            void Campo(RowDescriptor r, string etiqueta, string? valor, float ancho = 1)
            {
                r.RelativeItem(ancho).Row(x =>
                {
                    x.AutoItem()
                     .Text(etiqueta)
                     .FontFamily(FuenteBase)
                     .FontSize(TamanoTextoBase)
                     .SemiBold()
                     .FontColor(Colors.Grey.Darken2);

                    x.RelativeItem()
                        .PaddingLeft(3)
                        .BorderBottom(0.4f)
                        .BorderColor(Colors.Black)
                        .PaddingBottom(1)
                        .Text(t =>
                        {
                            t.Span(" " + (valor ?? ""))
                             .FontFamily(FuenteBase)
                             .FontSize(TamanoTextoBase)
                             .FontColor(Colors.Black);
                        });
                });
            }

            // HELPER UNIFICADO: Calcula dinámicamente la capacidad del primer renglón
            void CampoMultiLineaEstiloCuaderno(ColumnDescriptor col, string etiqueta, string? valor, int lineasTotales = 2)
            {
                col.Item().PaddingBottom(4).Column(c =>
                {
                    string texto = (valor ?? "").Trim();

                    // Ajusta dinámicamente la capacidad del primer renglón según el largo de la etiqueta
                    int caracteresPrimeraLinea = 110;

                    int caracteresLineaCompleta = 120;

                    var lineasDeTexto = new List<string>();

                    if (texto.Length <= caracteresPrimeraLinea)
                    {
                        lineasDeTexto.Add(texto);
                        texto = "";
                    }
                    else
                    {
                        int corte = texto.LastIndexOf(' ', caracteresPrimeraLinea);
                        if (corte <= 0) corte = caracteresPrimeraLinea;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    while (texto.Length > 0)
                    {
                        if (texto.Length <= caracteresLineaCompleta)
                        {
                            lineasDeTexto.Add(texto);
                            break;
                        }

                        int corte = texto.LastIndexOf(' ', caracteresLineaCompleta);
                        if (corte <= 0) corte = caracteresLineaCompleta;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    string textoLinea1 = lineasDeTexto.Count > 0 ? lineasDeTexto[0] : "";

                    c.Item().Row(r =>
                    {
                        r.AutoItem()
                         .Text(etiqueta)
                         .FontFamily(FuenteBase)
                         .FontSize(TamanoTextoBase)
                         .SemiBold()
                         .FontColor(Colors.Grey.Darken2);

                        r.RelativeItem()
                            .PaddingLeft(3)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLinea1) ? " " : " " + textoLinea1)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    });

                    for (int i = 1; i < lineasTotales; i++)
                    {
                        string textoLineaSiguiente = i < lineasDeTexto.Count ? lineasDeTexto[i] : "";

                        c.Item()
                            .PaddingTop(2)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLineaSiguiente) ? " " : textoLineaSiguiente)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    }
                });
            }

            void CampoMultiLineaEstiloCuaderno1(ColumnDescriptor col, string etiqueta, string? valor, int lineasTotales = 2)
            {
                col.Item().PaddingBottom(4).Column(c =>
                {
                    string texto = (valor ?? "").Trim();

                    // Ajusta dinámicamente la capacidad del primer renglón según el largo de la etiqueta
                    int caracteresPrimeraLinea = 130;

                    int caracteresLineaCompleta = 120;

                    var lineasDeTexto = new List<string>();

                    if (texto.Length <= caracteresPrimeraLinea)
                    {
                        lineasDeTexto.Add(texto);
                        texto = "";
                    }
                    else
                    {
                        int corte = texto.LastIndexOf(' ', caracteresPrimeraLinea);
                        if (corte <= 0) corte = caracteresPrimeraLinea;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    while (texto.Length > 0)
                    {
                        if (texto.Length <= caracteresLineaCompleta)
                        {
                            lineasDeTexto.Add(texto);
                            break;
                        }

                        int corte = texto.LastIndexOf(' ', caracteresLineaCompleta);
                        if (corte <= 0) corte = caracteresLineaCompleta;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    string textoLinea1 = lineasDeTexto.Count > 0 ? lineasDeTexto[0] : "";

                    c.Item().Row(r =>
                    {
                        r.AutoItem()
                         .Text(etiqueta)
                         .FontFamily(FuenteBase)
                         .FontSize(TamanoTextoBase)
                         .SemiBold()
                         .FontColor(Colors.Grey.Darken2);

                        r.RelativeItem()
                            .PaddingLeft(3)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLinea1) ? " " : " " + textoLinea1)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    });

                    for (int i = 1; i < lineasTotales; i++)
                    {
                        string textoLineaSiguiente = i < lineasDeTexto.Count ? lineasDeTexto[i] : "";

                        c.Item()
                            .PaddingTop(2)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLineaSiguiente) ? " " : textoLineaSiguiente)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    }
                });
            }


            void CampoMultiLineaEstiloCuaderno2(ColumnDescriptor col, string etiqueta, string? valor, int lineasTotales = 2)
            {
                col.Item().PaddingBottom(4).Column(c =>
                {
                    string texto = (valor ?? "").Trim();

                    // Ajusta dinámicamente la capacidad del primer renglón según el largo de la etiqueta
                    int caracteresPrimeraLinea = 130;

                    int caracteresLineaCompleta = 120;

                    var lineasDeTexto = new List<string>();

                    if (texto.Length <= caracteresPrimeraLinea)
                    {
                        lineasDeTexto.Add(texto);
                        texto = "";
                    }
                    else
                    {
                        int corte = texto.LastIndexOf(' ', caracteresPrimeraLinea);
                        if (corte <= 0) corte = caracteresPrimeraLinea;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    while (texto.Length > 0)
                    {
                        if (texto.Length <= caracteresLineaCompleta)
                        {
                            lineasDeTexto.Add(texto);
                            break;
                        }

                        int corte = texto.LastIndexOf(' ', caracteresLineaCompleta);
                        if (corte <= 0) corte = caracteresLineaCompleta;

                        lineasDeTexto.Add(texto.Substring(0, corte).Trim());
                        texto = texto.Substring(corte).Trim();
                    }

                    string textoLinea1 = lineasDeTexto.Count > 0 ? lineasDeTexto[0] : "";

                    c.Item().Row(r =>
                    {
                        r.AutoItem()
                         .Text(etiqueta)
                         .FontFamily(FuenteBase)
                         .FontSize(TamanoTextoBase)
                         .SemiBold()
                         .FontColor(Colors.Grey.Darken2);

                        r.RelativeItem()
                            .PaddingLeft(3)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLinea1) ? " " : " " + textoLinea1)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    });

                    for (int i = 1; i < lineasTotales; i++)
                    {
                        string textoLineaSiguiente = i < lineasDeTexto.Count ? lineasDeTexto[i] : "";

                        c.Item()
                            .PaddingTop(2)
                            .BorderBottom(0.4f)
                            .BorderColor(Colors.Black)
                            .PaddingBottom(1)
                            .Text(t =>
                            {
                                t.Span(string.IsNullOrWhiteSpace(textoLineaSiguiente) ? " " : textoLineaSiguiente)
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .FontColor(Colors.Black);
                            });
                    }
                });
            }



            IContainer CuadroConTitulo(ColumnDescriptor col, string titulo)
            {
                IContainer? resultado = null;
                col.Item().PaddingTop(4.5f).Border(1).BorderColor(colorBorde).Column(box =>
                {
                    box.Item().Background(colorBarraTitulo).Padding(4)
                        .Text(titulo)
                        .FontFamily(FuenteBase)
                        .Bold()
                        .FontSize(9)
                        .FontColor(Colors.White)
                        .AlignCenter();

                    resultado = box.Item().Padding(8);
                });
                return resultado!;
            }

            IContainer CuadroSinTitulo(ColumnDescriptor col)
            {
                IContainer? resultado = null;
                col.Item().PaddingTop(4.5f).Border(1).BorderColor(colorBorde).Padding(8).Column(box =>
                {
                    resultado = box.Item();
                });
                return resultado!;
            }

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(40);

                    page.DefaultTextStyle(x => x.FontFamily(FuenteBase).FontSize(TamanoTextoBase).FontColor(Colors.Black));

                    // ---------- Encabezado ----------
                    page.Header().Column(col =>
                    {
                        var logoPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot", "images", "LogoCC.png"
                        );

                        if (File.Exists(logoPath))
                        {
                            col.Item().Width(140).Image(logoPath).FitWidth();
                        }

                        col.Item().PaddingTop(4).Background(colorEmpresa).Padding(5)
                            .Text("ORDER EXPRESS CASA DE CAMBIO SA DE CV ACTIVIDAD AUXILIAR DEL CRÉDITO")
                            .FontFamily(FuenteBase)
                            .FontColor(Colors.White)
                            .Bold()
                            .FontSize(10)
                            .AlignCenter();

                        col.Item().PaddingTop(6.5f).AlignCenter()
                            .Text("CARTA DECLARATORIA")
                            .FontFamily(FuenteBase)
                            .Bold()
                            .FontSize(11);

                        col.Item().AlignCenter()
                            .Text("(REMESA)")
                            .FontFamily(FuenteBase)
                            .FontSize(TamanoTextoBase)
                            .FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(30).Row(r =>
                        {
                            r.ConstantItem(180).Row(x =>
                            {
                                x.AutoItem()
                                 .Text("Fecha de Elaboración: ")
                                 .FontFamily(FuenteBase)
                                 .FontSize(TamanoTextoBase)
                                 .SemiBold()
                                 .FontColor(Colors.Grey.Darken2);

                                x.RelativeItem().PaddingLeft(3).BorderBottom(0.3f).BorderColor(Colors.Black)
                                    .PaddingBottom(1)
                                    .Text(t =>
                                    {
                                        t.Span(" " + c.FechaElaboracion.ToString("dd/MM/yyyy"))
                                         .FontFamily(FuenteBase)
                                         .FontSize(TamanoTextoBase)
                                         .FontColor(Colors.Black);
                                    });
                            });

                            r.RelativeItem();
                        });
                    });

                    // ---------- Cuerpo ----------
                    page.Content().Column(col =>
                    {
                        // ===== CUADRO 1: Datos del Beneficiario =====
                        var beneficiario = CuadroConTitulo(col, "DATOS DEL BENEFICIARIO");
                        beneficiario.Column(inner =>
                        {
                            inner.Spacing(6);

                            inner.Item().Row(r => Campo(r, "Nombre Completo: ", c.BeneficiarioNombreCompleto));

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Fecha de Nacimiento: ", c.BeneficiarioFechaNacimiento?.ToString("dd/MM/yyyy") ?? "");
                                Campo(r, "CURP: ", c.BeneficiarioCurp);
                            });

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Tipo de Identificación: ", c.BeneficiarioTipoIdentificacion);
                                Campo(r, "N° de Identificación: ", c.BeneficiarioNumIdentificacion);
                            });

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Teléfono: ", c.BeneficiarioTelefono);
                                Campo(r, "País de nacimiento: ", c.BeneficiarioPaisNacimiento);
                                Campo(r, "Entidad de nacimiento: ", c.BeneficiarioEntidadNacimiento);
                            });

                            inner.Item().Row(r => Campo(r, "Domicilio: ", c.BeneficiarioDomicilio));

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Ocupación: ", c.BeneficiarioOcupacion, 0.8f);
                            });

                            CampoMultiLineaEstiloCuaderno(inner, "Describa brevemente el punto anterior:", c.BeneficiarioDescripcionOcupacion, lineasTotales: 2);
                        });

                        // ===== CUADRO 2: Datos del Girador =====
                        var girador = CuadroConTitulo(col, "DATOS DEL GIRADOR");
                        girador.Column(inner =>
                        {
                            inner.Spacing(6);

                            inner.Item().Row(r =>
                                Campo(r, "Por medio de la presente declaro que la remesa con N° de folio: ", c.RemesaFolio));

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Por el monto: ", $"${c.Monto:N2}");
                                Campo(r, "Del banco: ", c.Banco);
                            });

                            inner.Item().Row(r =>
                                Campo(r, "Del cual soy beneficiario proviene de la cuenta N°: ", c.CuentaNumero));

                            inner.Item().Row(r => Campo(r, "A nombre de: ", c.GiradorNombre));

                            CampoMultiLineaEstiloCuaderno1(inner, "El cual se dedica a:", c.GiradorOcupacion, lineasTotales: 2);

                            inner.Item().Column(colEn =>
                            {
                                colEn.Item().Row(r =>
                                {
                                    r.AutoItem()
                                     .Text("En: ")
                                     .FontFamily(FuenteBase)
                                     .FontSize(TamanoTextoBase)
                                     .SemiBold()
                                     .FontColor(Colors.Grey.Darken2);

                                    r.RelativeItem().PaddingLeft(3).BorderBottom(0.4f).BorderColor(Colors.Black)
                                        .PaddingBottom(1)
                                        .Text(t =>
                                        {
                                            t.Span(" " + (c.GiradorLocalidadEstado ?? ""))
                                             .FontFamily(FuenteBase)
                                             .FontSize(TamanoTextoBase)
                                             .FontColor(Colors.Black);
                                        });
                                });

                                colEn.Item()
                                    .AlignCenter()
                                    .Text("(Localidad y Estado)")
                                    .FontFamily(FuenteBase)
                                    .FontSize(6.5f)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });

                        // ===== CUADRO 3: Relación / Origen de recursos =====
                        var recursos = CuadroSinTitulo(col);
                        recursos.Column(inner =>
                        {
                            inner.Spacing(6);

                            inner.Item().Row(r =>
                                Campo(r, "Mi relación o parentesco con el Girador es: ", c.RelacionConGirador));

                            CampoMultiLineaEstiloCuaderno(inner, "Dichos recursos provienen de, y será utilizado para:", c.OrigenDestinoRecursos, lineasTotales: 2);

                            inner.Item().Row(r =>
                            {
                                Campo(r, "Propietario Real del recurso: ", c.PropietarioReal);
                            });

                            CampoMultiLineaEstiloCuaderno(inner, "El cual radica en: ", c.PropietarioRealLocalidadEstado, lineasTotales: 2);
                        });

                        // ===== CUADRO 4: Declaración final + Firma =====
                        var declaracion = CuadroSinTitulo(col);
                        declaracion.Column(inner =>
                        {
                            inner.Item().PaddingTop(-3).Text(
                                "Por lo cual yo declaro, que no habrá en el futuro ningún reclamo por este cheque, como un paro del pago, reporte de robo, y/o cualquier otra circunstancia. " +
                                "Por medio del presente manifiesto que los datos y/o documentos proporcionados son auténticos y el origen, manejo o destino de los recursos no se encuentran relacionados con" +
                                " operaciones que pudieran favorecer, prestar ayuda, auxilio o cooperación de cualquier especie para la comisión del delito previsto en el artículo 139 Quáter del Código Penal Federal " +
                                "o que pudiesen ubicarse en los supuestos del artículo 400 Bis del mismo ordenamiento legal."
                            )
                            .FontFamily(FuenteBase)
                            .FontSize(TamanoTextoBase)
                            .Justify()
                            .LineHeight(1.3f);

                            inner.Item().PaddingTop(50).AlignCenter().Column(firma =>
                            {
                                firma.Item().Width(300).LineHorizontal(0.2f).LineColor(colorLinea);

                                firma.Item().AlignCenter()
                                    .Text("(Firma)")
                                    .FontFamily(FuenteBase)
                                    .FontSize(6.5f)
                                    .FontColor(Colors.Grey.Darken1);

                                firma.Item().PaddingTop(2).AlignCenter()
                                    .Text(c.BeneficiarioNombreCompleto ?? "NOMBRE DEL BENEFICIARIO")
                                    .FontFamily(FuenteBase)
                                    .FontSize(TamanoTextoBase)
                                    .Bold();
                            });
                        });
                    });

                    // ---------- Pie de página ----------
                    page.Footer().PaddingTop(15).Column(f =>
                    {
                        f.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                        f.Item().PaddingTop(4).Text(
                            "Order Express Casa de Cambio SA de CV Actividad Auxiliar del Crédito | Melchor Ocampo No. 51, Int. A, Col. Centro, C.P. 61250, Maravatío, Michoacán | Tel. (447) 478 0005 | V.1.1 | Aut. 101-00801 | Formato de Uso Interno"
                        )
                        .FontFamily(FuenteBase)
                        .FontSize(6.5f)
                        .AlignCenter()
                        .FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return documento.GeneratePdf();
        }
    }
}