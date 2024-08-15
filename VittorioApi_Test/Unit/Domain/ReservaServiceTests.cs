using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;
using VittorioApiT2M.Domain.Services;

namespace VittorioApiT2M.Tests.Unit.Domain
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
        public async Task ObterReservasPorClienteIdESemana_ReservaExistente_RetornaReservas()
        {
            // Arrange
            var reservas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    ClienteId = 1,
                    DataReserva = DateTime.SpecifyKind(new DateTime(2024, 08, 15), DateTimeKind.Utc)
                }
            };
            _mockReservaRepository.Setup(repo => repo.ObterReservasPorClienteIdESemana(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(reservas);

            // Act
            var result = await _reservaService.ObterReservasPorClienteIdESemana(1, new DateTime(2024, 08, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 08, 20, 0, 0, 0, DateTimeKind.Utc));

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }
    }
}
