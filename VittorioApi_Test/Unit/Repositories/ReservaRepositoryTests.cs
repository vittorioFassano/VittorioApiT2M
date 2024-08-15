using Moq;
using Xunit;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Infrastructure.Repositories;
using VittorioApiT2M.Application.Data;

namespace VittorioApiT2M.Tests.Unit.Repositories
{
    public class ReservaRepositoryTests
    {
        private readonly Mock<IDbConnection> _dbConnectionMock;
        private readonly Mock<IDapperWrapper> _dapperWrapperMock;
        private readonly ReservaRepository _reservaRepository;

        public ReservaRepositoryTests()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _dapperWrapperMock = new Mock<IDapperWrapper>();
            _reservaRepository = new ReservaRepository(_dbConnectionMock.Object, _dapperWrapperMock.Object);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarReserva_QuandoIdExistente()
        {
            // Arrange
            var reservaId = 1;
            var reservaEsperada = new Reservas { Id = reservaId, ClienteId = 1, NumeroPessoas = 4 };

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(reservaEsperada);

            // Act
            var resultado = await _reservaRepository.ObterPorId(reservaId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(reservaId, resultado.Id);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNull_QuandoIdNaoExistente()
        {
            // Arrange
            var reservaId = 99;

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync((Reservas)null);

            // Act
            var resultado = await _reservaRepository.ObterPorId(reservaId);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterPorId_DeveLancarExcecao_QuandoErroOcorre()
        {
            // Arrange
            var reservaId = 1;

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado no banco de dados"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterPorId(reservaId));
        }
    }
}
