document.addEventListener("DOMContentLoaded", () => {
  // Elementos del formulario
  const nombreInput = document.getElementById("nombre");
  const paternoInput = document.getElementById("paterno");
  const maternoInput = document.getElementById("materno");
  const fechaInput = document.getElementById("fechaNacimiento");
  const sexoInput = document.getElementById("sexo");
  const entidadInput = document.getElementById("entidadNacimiento");
  const curpInput = document.getElementById("curp");

  // Escuchar cambios en cualquiera de los campos
  const inputs = [nombreInput, paternoInput, maternoInput, fechaInput, sexoInput, entidadInput];
  inputs.forEach(input => {
    if (input) {
      input.addEventListener("input", generarCURP);
      input.addEventListener("change", generarCURP);
    }
  });

  function generarCURP() {
    const nombre = limpiarTexto(nombreInput?.value || "");
    const paterno = limpiarTexto(paternoInput?.value || "");
    const materno = limpiarTexto(maternoInput?.value || "");
    const fecha = fechaInput?.value || ""; // Formato YYYY-MM-DD
    const sexo = sexoInput?.value || "X";
    const entidad = entidadInput?.value || "NE";

    // Validar datos mínimos necesarios
    if (!nombre || !paterno || !fecha) {
      return;
    }

    // 1. Inicial Apellido Paterno + Primera Vocal Interna
    let c1 = paterno.charAt(0);
    let c2 = primeraVocalInterna(paterno);

    // 2. Inicial Apellido Materno (o 'X' si no existe)
    let c3 = materno ? materno.charAt(0) : "X";

    // 3. Inicial del Primer Nombre (ignora JOSE / MARIA si tiene más nombres)
    let primerNombre = filtrarNombresComunes(nombre);
    let c4 = primerNombre.charAt(0);

    // 4. Fecha de Nacimiento (AAMMDD)
    let partesFecha = fecha.split("-"); // [YYYY, MM, DD]
    let anio = partesFecha[0] ? partesFecha[0].substring(2, 4) : "00";
    let mes = partesFecha[1] || "00";
    let dia = partesFecha[2] || "00";
    let c5_10 = `${anio}${mes}${dia}`;

    // 5. Sexo (H/M)
    let c11 = sexo;

    // 6. Entidad Federativa (2 letras)
    let c12_13 = entidad;

    // 7. Primeras consonantes internas de Paterno, Materno y Nombre
    let c14 = primeraConsonanteInterna(paterno);
    let c15 = materno ? primeraConsonanteInterna(materno) : "X";

    // Armar la CURP completa
    let curpCalculada = `${c1}${c2}${c3}${c4}${c5_10}${c11}${c12_13}${c14}${c15}`.toUpperCase();

    // Inyectar en el campo CURP
    if (curpInput) {
      curpInput.value = curpCalculada;
    }
  }

  // Helper Functions
  function limpiarTexto(txt) {
    return txt.trim().toUpperCase()
      .normalize("NFD").replace(/[\u0300-\u036f]/g, "") // Quita acentos
      .replace(/Ñ/g, "X");
  }

  function primeraVocalInterna(texto) {
    for (let i = 1; i < texto.length; i++) {
      if (/[AEIOU]/.test(texto.charAt(i))) {
        return texto.charAt(i);
      }
    }
    return "X";
  }

  function primeraConsonanteInterna(texto) {
    for (let i = 1; i < texto.length; i++) {
      let char = texto.charAt(i);
      if (/[BCDFGHJKLMNPQRSTVWXYZ]/.test(char)) {
        return char;
      }
    }
    return "X";
  }

  function filtrarNombresComunes(nombreCompleto) {
    let nombres = nombreCompleto.split(" ");
    if (nombres.length > 1 && (nombres[0] === "JOSE" || nombres[0] === "MARIA")) {
      return nombres[1];
    }
    return nombres[0];
  }

// Función auxiliar para extraer números reales de un texto con comas/formato
function obtenerValorNumerico(id) {
    const el = document.getElementById(id);
    if (!el || !el.value) return 0;
    // Remueve comas y caracteres no numéricos excepto el punto
    const valLimpio = el.value.replace(/,/g, '');
    return parseFloat(valLimpio) || 0;
}

// Formateador de moneda para montos al perder el foco (blur)
function formatearMontoPesos(e) {
    let valor = e.target.value.replace(/[^0-9.]/g, ''); // Limpiar caracteres
    if (valor !== '') {
        let numero = parseFloat(valor);
        if (!isNaN(numero)) {
            e.target.value = new Intl.NumberFormat('es-MX', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }).format(numero);
        }
    }
}

    // --- CÁLCULO DE LÍNEA OPERATIVA SEGÚN PUNTO 5 ---
    function calcularLineaOperativa() {
        const checkCompra = document.getElementById('CheckHeaderCompra')?.checked || false;
        const checkVenta = document.getElementById('CheckHeaderVenta')?.checked || false;

        // Obtener valores sin comas
        const montoEfCompra = obtenerValorNumerico('PerfilCompraEfectivo');
        const montoEfVenta = obtenerValorNumerico('PerfilVentaEfectivo');
        const montoRemCompra = obtenerValorNumerico('PerfilCompraRemesa');
        const montoRemVenta = obtenerValorNumerico('PerfilVentaRemesa');

        const tieneEfectivo = (montoEfCompra > 0 || montoEfVenta > 0);
        const tieneRemesa = (montoRemCompra > 0 || montoRemVenta > 0);

        // 1. Acciones (COMPRA / VENTA)
        let parteAccion = "";
        if (checkCompra && checkVenta) {
            parteAccion = "COMPRA Y VENTA";
        } else if (checkCompra) {
            parteAccion = "COMPRA";
        } else if (checkVenta) {
            parteAccion = "VENTA";
        }

        // 2. Instrumentos (EFECTIVO / REMESA X VOLUMEN)
        let parteInstrumento = "";
        if (tieneEfectivo && tieneRemesa) {
            parteInstrumento = "EFECTIVO Y REMESA X VOLUMEN";
        } else if (tieneEfectivo) {
            parteInstrumento = "EFECTIVO";
        } else if (tieneRemesa) {
            parteInstrumento = "REMESA X VOLUMEN";
        }

        // 3. Cadena final
        let resultadoFinal = "";
        if (parteAccion && parteInstrumento) {
            resultadoFinal = `${parteAccion} DE ${parteInstrumento}`;
        } else if (parteAccion) {
            resultadoFinal = parteAccion;
        } else if (parteInstrumento) {
            resultadoFinal = parteInstrumento;
        }

        const inputTipoOp = document.getElementById('LineaTipoOperacion');
        if (inputTipoOp) inputTipoOp.value = resultadoFinal;

        // 4. Suma total para Monto
        const sumaTotal = montoEfCompra + montoEfVenta + montoRemCompra + montoRemVenta;
        const formatoMoneda = new Intl.NumberFormat('es-MX', {
            style: 'currency',
            currency: 'MXN',
            minimumFractionDigits: 2
        }).format(sumaTotal);

        const inputMonto = document.getElementById('LineaMontoSolicitado');
        if (inputMonto) inputMonto.value = formatoMoneda;
    }

    document.addEventListener("DOMContentLoaded", () => {
        // 1. Restricción para campos solo números enteros (Columna Verde: OP Promedio)
        const camposEnteros = document.querySelectorAll(".solo-enteros");
        camposEnteros.forEach(input => {
            input.addEventListener("input", (e) => {
                e.target.value = e.target.value.replace(/[^0-9]/g, '');
            });
        });

        // 2. Formato con comas y decimales para montos (Recuadros Rojos)
        const camposMontos = document.querySelectorAll(".input-monto-pesos");
        camposMontos.forEach(input => {
            // Al escribir solo permite números y un punto
            input.addEventListener("input", (e) => {
                e.target.value = e.target.value.replace(/[^0-9.]/g, '');
                calcularLineaOperativa();
            });
            // Al salir del campo le da el formato $ 100,000.00
            input.addEventListener("blur", formatearMontoPesos);
        });

        // 3. Eventos para checkboxes del encabezado
        ['CheckHeaderCompra', 'CheckHeaderVenta'].forEach(id => {
            const el = document.getElementById(id);
            if (el) el.addEventListener('change', calcularLineaOperativa);
        });

        calcularLineaOperativa();
    });
 

  // Escuchar cambios en los inputs del punto 5 para actualizar en tiempo real
  const inputsPunto5 = document.querySelectorAll('input[name="CheckHeaderCompra"], input[name="CheckHeaderVenta"], input[name="PerfilCompraEfectivo"], input[name="PerfilVentaEfectivo"], input[name="PerfilCompraRemesa"], input[name="PerfilVentaRemesa"]');

  inputsPunto5.forEach(input => {
      input.addEventListener('input', calcularLineaOperativa);
      input.addEventListener('change', calcularLineaOperativa);
  });
});
