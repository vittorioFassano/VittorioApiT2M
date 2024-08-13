using Microsoft.AspNetCore.Mvc;
using VittorioApiT2M.Application.DTOs;
using VittorioApiT2M.Application.Services;

namespace VittorioApiT2M.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly ReservaAppService _reservaAppService;

        public ReservaController(ReservaAppService reservaAppService)
        {
            _reservaAppService = reservaAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservas = await _reservaAppService.ObterTodasReservas();
            return Ok(reservas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservaDto reservaDto)
        {
            await _reservaAppService.AdicionarReserva(reservaDto);

            // Obter a lista completa e retornar o recém-criado
            var reservas = await _reservaAppService.ObterTodasReservas();
            var novaReserva = reservas.Last(); // Assumindo que o Id é gerado automaticamente

            return CreatedAtAction(nameof(GetAll), new { id = novaReserva.Id }, novaReserva);
        }
    }
}
