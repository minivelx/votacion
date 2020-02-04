using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiVotacion.Entidades
{
    public class Votacion
    {
        [Key, Required, ForeignKey("Candidato")]
        public int IdCandidato { get; set; }
        [Key, Required, StringLength(450), ForeignKey("Usuario")]
        public string IdUsuario { get; set; }


        public ApplicationUser Usuario { get; set; }
        public Candidato Candidato { get; set; }
    }
}
