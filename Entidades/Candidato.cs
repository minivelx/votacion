using System.ComponentModel.DataAnnotations;

namespace WebApiVotacion.Entidades
{
    public class Candidato
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
        
        public string FotoBase64 { get; set; }
    }
}
