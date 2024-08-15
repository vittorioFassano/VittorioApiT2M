using Moq;
using VittorioApiT2M.Application.Services;
using VittorioApiT2M.Api.Controllers;
using Xunit;
using VittorioApiT2M.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace VittorioApi_Test
{
    public class ReservaControllerTests
    {
        private readonly Mock<IReservaAppService> _mockReservaAppService;
        private readonly ReservaController _controller;

        public ReservaControllerTests()
        {
            _mockReservaAppService = new Mock<IReservaAppService>();
            _controller = new ReservaController(_mockReservaAppService.Object);
        }

        [Fact]
        public async Task ObterTodasReservas_DeveRetornarOk_ComListaDeReservas()
        {
            // Arrange
            var mockReservas = new List<ReservaDto>
            {
                new ReservaDto { Id = 1, ClienteId = 1, DataReserva = DateTime.Now, HoraReserva = TimeSpan.FromHours(18), NumeroPessoas = 2, Confirmada = false },
                new ReservaDto { Id = 2, ClienteId = 2, DataReserva = DateTime.Now, HoraReserva = TimeSpan.FromHours(20), NumeroPessoas = 4, Confirmada = true }
            };

            _mockReservaAppService.Setup(service => service.ObterTodasReservas())
                                  .ReturnsAsync(mockReservas);

            // Act
            var result = await _controller.ObterTodasReservas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReservaDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
