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
                return BadRequest("Dados da reserva incompletos ou inválidos");
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
            if (!ModelState.IsValid || reservaDto.DataReserva == default || reservaDto.HoraReserva == default)
            {
                return BadRequest("Dados da reserva incompletos ou inválidos");
            }

            var reservaExistente = await _reservaAppService.ObterReservaPorId(id);
            if (reservaExistente == null)
            {
                return NotFound($"Reserva com ID {id} não encontrada.");
            }

            reservaExistente.ClienteId = reservaDto.ClienteId;
            reservaExistente.DataReserva = reservaDto.DataReserva;
            reservaExistente.HoraReserva = reservaDto.HoraReserva;
            reservaExistente.NumeroPessoas = reservaDto.NumeroPessoas;
            reservaExistente.Confirmada = reservaDto.Confirmada;

            try
            {
                await _reservaAppService.AtualizarReserva(reservaExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar reserva: {ex.Message}");
            }

            var reservaAtualizada = await _reservaAppService.ObterReservaPorId(id);
            return Ok(reservaAtualizada);
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
            return Ok("Reserva cancelada!");
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarReserva(int id)
        {
            try
            {
                var reserva = await _reservaAppService.ObterReservaPorId(id);

                if (reserva == null)
                {
                    return NotFound($"Reserva com ID {id} não encontrada.");
                }

                if (reserva.Confirmada)
                {
                    return BadRequest("Você já confirmou essa reserva!");
                }

                await _reservaAppService.ConfirmarReserva(id);
                return Ok("Reserva confirmada com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao confirmar reserva: {ex.Message}");
            }
        }

    }
}
