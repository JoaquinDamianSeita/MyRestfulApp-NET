using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace MyRestfulApp_NET.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ApiController]
    public class PaisesController : BaseController
    {
        private readonly IPaisesService _paisesService;

        public PaisesController(IPaisesService paisesService)
        {
            _paisesService = paisesService;
        }

        [HttpGet("{pais}")]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPais(string pais)
        {
            switch (pais.ToUpper())
            {
                case "AR":
                    var informacionPais = await _paisesService.ObtenerInformacionPais(pais.ToUpper());
                    if (informacionPais == null)
                    {
                        return NotFound($"País {pais} no encontrado");
                    }

                    return Ok(informacionPais);
                case "BR":
                case "CO":
                    // Retorna un error 401 Unauthorized para los países BR y CO
                    return StatusCode((int)HttpStatusCode.Unauthorized);
                default:
                    // Retorna un error 404 Not Found si el país no está soportado
                    return NotFound($"País {pais} no encontrado");
            }
        }
    }
}
