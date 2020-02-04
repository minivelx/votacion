using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiVotacion.Entidades;

namespace WebApiVotacion.Controllers
{
    [Produces("application/json")]
    [Route("api/Votacion")]
    public class VotacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VotacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Votacion([FromBody] int idCandidato)
        {
            try
            {
                var userId = User.getUserId();
                var voto = new Votacion { IdCandidato = idCandidato, IdUsuario = userId };
                _context.Votacion.Add(voto);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "voto asignado correctamente." });
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error!. " + ErrorMsg });
            }
        }

    }
}