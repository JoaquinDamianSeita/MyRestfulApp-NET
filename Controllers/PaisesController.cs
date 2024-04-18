using Microsoft.AspNetCore.Mvc;

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
            var paisUpper = pais.ToUpper();

            if (paisUpper.Equals("BR") || paisUpper.Equals("CO"))
                return Unauthorized(new { Message = "País inválido para la consulta." });     

            var informacionPais = await _paisesService.ObtenerInformacionPais(pais.ToUpper());

            return Ok(informacionPais);
        }
    }
}
