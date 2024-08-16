using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;
using VittorioApiT2M.Domain.Services;
using VittorioApiT2M.Application.DTOs;

namespace VittorioApi_Test.Unit.Domain
{
    public class ReservaServiceTests
    {
        private readonly Mock<IReservaRepository> _mockReservaRepository;
        private readonly ReservaService _reservaService;

        public ReservaServiceTests()
        {
            _mockReservaRepository = new Mock<IReservaRepository>();
            _reservaService = new ReservaService(_mockReservaRepository.Object);
        }

        [Fact]
        public async Task ConfirmarReserva_ReservaExiste_ConfirmaReserva()
        {
            var reserva = new Reservas { Id = 1, Confirmada = false };
            _mockReservaRepository.Setup(repo => repo.ObterPorId(1)).ReturnsAsync(reserva);

            await _reservaService.ConfirmarReserva(1);

            Assert.True(reserva.Confirmada);
            _mockReservaRepository.Verify(repo => repo.Atualizar(reserva), Times.Once);
        }

        [Fact]
        public async Task ConfirmarReserva_ReservaNaoExiste_LancaException()
        {
            _mockReservaRepository.Setup(repo => repo.ObterPorId(It.IsAny<int>())).ReturnsAsync((Reservas?)null);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _reservaService.ConfirmarReserva(1));
            Assert.Equal("Ocorreu um erro ao confirmar a reserva.", exception.Message);
        }

        [Fact]
        public async Task ObterReservasPorNomeEmailESemana_ReservasExistentes_RetornaReservas()
        {
            var reservas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    NomeCliente = "João Silva",
                    EmailCliente = "joao.silva@example.com",
                    DataReserva = new DateTime(2024, 08, 15, 0, 0, 0, DateTimeKind.Utc),
                    HoraReserva = new TimeSpan(10, 0, 0),
                    NumeroPessoas = 4,
                    Confirmada = false
                }
            };

            _mockReservaRepository.Setup(repo => repo.ObterReservasPorNomeEmailESemana("João Silva", "joao.silva@example.com", new DateTime(2024, 08, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 08, 20, 0, 0, 0, DateTimeKind.Utc)))
                .ReturnsAsync(reservas);

            var result = await _reservaService.ObterReservasPorNomeEmailESemana("João Silva", "joao.silva@example.com", new DateTime(2024, 08, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 08, 20, 0, 0, 0, DateTimeKind.Utc));

            Assert.Single(result);
            var reservaDto = result.First();
            Assert.Equal(1, reservaDto.Id);
            Assert.Equal("João Silva", reservaDto.NomeCliente);
            Assert.Equal("joao.silva@example.com", reservaDto.EmailCliente);
            Assert.Equal(new DateTime(2024, 08, 15, 0, 0, 0, DateTimeKind.Utc), reservaDto.DataReserva);
            Assert.Equal(new TimeSpan(10, 0, 0), reservaDto.HoraReserva);
            Assert.Equal(4, reservaDto.NumeroPessoas);
            Assert.False(reservaDto.Confirmada);
        }
    }
}
