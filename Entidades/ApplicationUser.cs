using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiVotacion.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(20)]
        public string Cedula { get; set; }

        public bool Activo { get; set; }
    }

    public class ApplicationRole : IdentityRole<string>
    {

    }

    public class IdentityUserClaim : IdentityUserClaim<string>
    {

    }

    public class IdentityUserLogin : IdentityUserLogin<string>
    {

    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {

    }
}
