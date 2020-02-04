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

    }
}