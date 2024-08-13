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
            if (reservas == null || !reservas.Any())
            {
                return NotFound("Nenhuma reserva encontrada.");
            }
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reserva = await _reservaAppService.ObterReservaPorId(id);
            if (reserva == null)
            {
                return NotFound("Reserva não encontrada.");
            }
            return Ok(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservaDto reservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _reservaAppService.AdicionarReserva(reservaDto);
                var reservas = await _reservaAppService.ObterTodasReservas();
                var novaReserva = reservas.Last();

                return CreatedAtAction(nameof(GetById), new { id = novaReserva.Id }, novaReserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar reserva: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _reservaAppService.ObterReservaPorId(id);
            if (reserva == null)
            {
                return NotFound("Reserva não encontrada para exclusão");
            }

            try
            {
                await _reservaAppService.RemoverReserva(id);
                return Ok("Reserva cancelada com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir reserva: {ex.Message}");
            }
        }
    }
}
