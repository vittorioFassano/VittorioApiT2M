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

        ///////////////

        // Teste para ObterTodas
        [Fact]
        public async Task ObterTodas_DeveRetornarReservas_QuandoNaoHouverErros()
        {
            // Arrange
            var reservasEsperadas = new List<Reservas>
    {
        new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 },
        new Reservas { Id = 2, ClienteId = 2, NumeroPessoas = 2 }
    };

            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(reservasEsperadas);

            // Act
            var resultado = await _reservaRepository.ObterTodas();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(reservasEsperadas.Count, resultado.Count());
        }


        [Fact]
        public async Task ObterTodas_DeveLancarExcecao_QuandoErroOcorre()
        {
            // Arrange
            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado no banco de dados"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterTodas());
        }

        // Teste para Adicionar

        [Fact]
        public async Task Adicionar_DeveAdicionarReservaComSucesso_QuandoNaoHouverErros()
        {
            // Arrange
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };

            // Act
            await _reservaRepository.Adicionar(reserva);

            // Assert
            _dapperWrapperMock.Verify(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva), Times.Once);
        }

        [Fact]
        public async Task Adicionar_DeveLancarExcecao_QuandoReservaEhNula()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _reservaRepository.Adicionar(null));
        }

        [Fact]
        public async Task Adicionar_DeveLancarExcecao_QuandoErroOcorre()
        {
            // Arrange
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao adicionar reserva"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Adicionar(reserva));
        }


        // Teste para Atualizar
        [Fact]
        public async Task Atualizar_DeveAtualizarReservaComSucesso_QuandoNaoHouverErros()
        {
            // Arrange
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };

            // Act
            await _reservaRepository.Atualizar(reserva);

            // Assert
            _dapperWrapperMock.Verify(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveLancarExcecao_QuandoReservaEhNula()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _reservaRepository.Atualizar(null));
        }

        [Fact]
        public async Task Atualizar_DeveLancarExcecao_QuandoErroOcorre()
        {
            // Arrange
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao atualizar reserva"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Atualizar(reserva));
        }


        // Teste para ObterReservasPorClienteIdESemana


        [Fact]
        public async Task ObterReservasPorClienteIdESemana_DeveLancarExcecao_QuandoDataInicioEhPosteriorADataFim()
        {
            // Arrange
            var clienteId = 1;
            var inicioSemana = new DateTime(2024, 8, 8);
            var fimSemana = new DateTime(2024, 8, 7);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _reservaRepository.ObterReservasPorClienteIdESemana(clienteId, inicioSemana, fimSemana));
        }

        //////////////////////////////////////////////////////////////////////////////////



    }
}
