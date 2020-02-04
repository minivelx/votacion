using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiVotacion.Entidades;

namespace WebApiVotacion.Controllers
{
    [Produces("application/json")]
    [Route("api/Candidatos")]
    public class CandidatosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidatosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Candidatos()
        {
            try
            {

                return Json(new { success = true, message = _context.Candidatos });
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error!. " + ErrorMsg });
            }
        }

        [HttpGet("crear")]
        public async Task<IActionResult> CrearCandidatos()
        {
            try
            {
                var candidat1 = new Candidato { Id = 0, Nombre = "JUAN FERNANDO RESTREPO", FotoBase64 = null };
                var candidat2 = new Candidato { Id = 0, Nombre = "JUAN FELIPE MOSQUERA", FotoBase64 = null };
                var candidat3 = new Candidato { Id = 0, Nombre = "JUAN LUIS URIBE", FotoBase64 = null };
                _context.Candidatos.Add(candidat1);
                _context.Candidatos.Add(candidat2);
                _context.Candidatos.Add(candidat3);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Registros guardados." });
            }
            catch (Exception exc)
            {
                string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
                return Json(new { success = false, message = "Error!. " + ErrorMsg });
            }
        }

    }
}