using System.ComponentModel.DataAnnotations;

namespace CartaDeclaratoriaApp.Models
{
    public class CartaDeclaratoria
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de elaboración")]
        [DataType(DataType.Date)]
        public DateTime FechaElaboracion { get; set; } = DateTime.Today;

        // ---------- Datos del Beneficiario ----------
        [Display(Name = "Nombre completo")]
        public string BeneficiarioNombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? BeneficiarioFechaNacimiento { get; set; }

        [Display(Name = "País de nacimiento")]
        public string? BeneficiarioPaisNacimiento { get; set; }

        [Display(Name = "Entidad de nacimiento")]
        public string? BeneficiarioEntidadNacimiento { get; set; }

        [Display(Name = "Domicilio particular")]
        public string? BeneficiarioDomicilio { get; set; }

        [Display(Name = "N° de identificación")]
        public string? BeneficiarioNumIdentificacion { get; set; }

        [Display(Name = "Tipo de identificación")]
        public string? BeneficiarioTipoIdentificacion { get; set; }

        [Display(Name = "Teléfono")]
        public string? BeneficiarioTelefono { get; set; }

        [Display(Name = "CURP")][StringLength(18)]
        public string? BeneficiarioCurp { get; set; }

        [Display(Name = "Ocupación")]
        public string? BeneficiarioOcupacion { get; set; }

        [Display(Name = "Describa brevemente el punto anterior")]
        public string? BeneficiarioDescripcionOcupacion { get; set; }

        // ---------- Datos del Girador ----------
        [Display(Name = "N° de folio de la remesa")]
        public string RemesaFolio { get; set; } = string.Empty;

        [Display(Name = "Monto")]   [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [Display(Name = "Cuenta N°")]
        public string? CuentaNumero { get; set; }

        [Display(Name = "Nombre completo del banco")]
        public string? Banco { get; set; }

        [Display(Name = "A nombre de (Girador)")]
        public string? GiradorNombre { get; set; }

        [Display(Name = "El cual se dedica a")]
        public string? GiradorOcupacion { get; set; }
		
		[Display(Name = "Localidad y estado (Girador)")]
        public string? GiradorLocalidadEstado { get; set; }


        [Display(Name = "Relación o parentesco con el Girador")]
        public string? RelacionConGirador { get; set; }

        [Display(Name = "Origen y destino de los recursos")]
        public string? OrigenDestinoRecursos { get; set; }

        [Display(Name = "Propietario real del recurso")]
        public string? PropietarioReal { get; set; }

        [Display(Name = "Localidad y estado (Propietario real)")]
        public string? PropietarioRealLocalidadEstado { get; set; }

        // ---------- Control interno ----------
        [Display(Name = "Capturado por")]
        public string? CapturadoPorUsuarioId { get; set; }

        [Display(Name = "Fecha de captura")]
        public DateTime FechaCaptura { get; set; } = DateTime.Now;

        [Display(Name = "Nombre y firma (texto/registro de conformidad)")]
        public string? NombreFirma { get; set; }
    }
}
