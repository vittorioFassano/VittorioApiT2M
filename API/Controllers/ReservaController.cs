using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VittorioApiT2M.Application.DTOs;
using VittorioApiT2M.Application.Services;

namespace VittorioApiT2M.Api.Controllers
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
        public async Task<IActionResult> ObterTodasReservas()
        {
            var reservas = await _reservaAppService.ObterTodasReservas();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterReservaPorId(int id)
        {
            var reserva = await _reservaAppService.ObterReservaPorId(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservaDto reservaDto)
        {
            if (!ModelState.IsValid || reservaDto.DataReserva == default || reservaDto.HoraReserva == default)
            {
                return BadRequest("Dados da reserva incompletos ou inválidos.");
            }

            try
            {
                await _reservaAppService.AdicionarReserva(reservaDto);
                var reservas = await _reservaAppService.ObterTodasReservas();
                var novaReserva = reservas.Last();

                return CreatedAtAction(nameof(ObterReservaPorId), new { id = novaReserva.Id }, novaReserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar reserva: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarReserva(int id, [FromBody] ReservaDto reservaDto)
        {
            // Verifica se o modelo está válido e se os campos DataReserva e HoraReserva não são padrões
            if (!ModelState.IsValid || reservaDto.DataReserva == default || reservaDto.HoraReserva == default)
            {
                return BadRequest("Dados da reserva incompletos ou inválidos.");
            }

            // Obtém a reserva existente
            var reservaExistente = await _reservaAppService.ObterReservaPorId(id);
            if (reservaExistente == null)
            {
                return NotFound();
            }

            // Atualiza os campos da reserva existente com os novos valores
            reservaExistente.ClienteId = reservaDto.ClienteId;
            reservaExistente.DataReserva = reservaDto.DataReserva;
            reservaExistente.HoraReserva = reservaDto.HoraReserva;
            reservaExistente.NumeroPessoas = reservaDto.NumeroPessoas;
            reservaExistente.Confirmada = reservaDto.Confirmada;

            // Atualiza a reserva
            try
            {
                await _reservaAppService.AtualizarReserva(reservaExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar reserva: {ex.Message}");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverReserva(int id)
        {
            var reservaExistente = await _reservaAppService.ObterReservaPorId(id);
            if (reservaExistente == null)
            {
                return NotFound();
            }

            await _reservaAppService.RemoverReserva(id);
            return NoContent();
        }
    }
}
