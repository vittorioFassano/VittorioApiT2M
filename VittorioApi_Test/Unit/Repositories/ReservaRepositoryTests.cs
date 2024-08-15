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
            var reservaId = 1;
            var reservaEsperada = new Reservas { Id = reservaId, ClienteId = 1, NumeroPessoas = 4 };

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(reservaEsperada);

            var resultado = await _reservaRepository.ObterPorId(reservaId);

            Assert.NotNull(resultado);
            Assert.Equal(reservaId, resultado.Id);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNull_QuandoIdNaoExistente()
        {
            var reservaId = 99;

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync((Reservas)null);

            var resultado = await _reservaRepository.ObterPorId(reservaId);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterPorId_DeveLancarExcecao_QuandoErroOcorre()
        {
            var reservaId = 1;

            _dapperWrapperMock.Setup(d => d.QuerySingleOrDefaultAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado no banco de dados"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterPorId(reservaId));
        }

        // Testes para ObterTodas
        [Fact]
        public async Task ObterTodas_DeveRetornarReservas_QuandoNaoHouverErros()
        {
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

            var resultado = await _reservaRepository.ObterTodas();

            Assert.NotNull(resultado);
            Assert.Equal(reservasEsperadas.Count, resultado.Count());
        }


        [Fact]
        public async Task ObterTodas_DeveLancarExcecao_QuandoErroOcorre()
        {
            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado no banco de dados"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterTodas());
        }

        // Testes para Adicionar
        [Fact]
        public async Task Adicionar_DeveAdicionarReservaComSucesso_QuandoNaoHouverErros()
        {
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };

            await _reservaRepository.Adicionar(reserva);

            _dapperWrapperMock.Verify(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva), Times.Once);
        }

        [Fact]
        public async Task Adicionar_DeveLancarExcecao_QuandoReservaEhNula()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _reservaRepository.Adicionar(null));
        }

        [Fact]
        public async Task Adicionar_DeveLancarExcecao_QuandoErroOcorre()
        {
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao adicionar reserva"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Adicionar(reserva));
        }

        // Testes para Atualizar
        [Fact]
        public async Task Atualizar_DeveAtualizarReservaComSucesso_QuandoNaoHouverErros()
        {
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };

            await _reservaRepository.Atualizar(reserva);

            _dapperWrapperMock.Verify(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveLancarExcecao_QuandoReservaEhNula()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _reservaRepository.Atualizar(null));
        }

        [Fact]
        public async Task Atualizar_DeveLancarExcecao_QuandoErroOcorre()
        {
            var reserva = new Reservas { Id = 1, ClienteId = 1, NumeroPessoas = 4 };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao atualizar reserva"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Atualizar(reserva));
        }

        // Testes para ObterReservasPorClienteIdESemana
        [Fact]
        public async Task ObterReservasPorClienteIdESemana_DeveLancarExcecao_QuandoDataInicioEhPosteriorADataFim()
        {
            var clienteId = 1;
            var inicioSemana = new DateTime(2024, 8, 8);
            var fimSemana = new DateTime(2024, 8, 7);

            await Assert.ThrowsAsync<ArgumentException>(() => _reservaRepository.ObterReservasPorClienteIdESemana(clienteId, inicioSemana, fimSemana));
        }

        [Fact]
        public async Task ObterReservasPorClienteIdESemana_DeveRetornarReservas_QuandoParametrosSaoValidos()
        {
            var clienteId = 1;
            var inicioSemana = new DateTime(2024, 8, 1);
            var fimSemana = new DateTime(2024, 8, 7);

            var reservasEsperadas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    ClienteId = clienteId,
                    DataReserva = new DateTime(2024, 8, 2),
                    HoraReserva = new TimeSpan(18, 0, 0),
                    NumeroPessoas = 4,
                    Confirmada = true
                },
                new Reservas
                {
                    Id = 2,
                    ClienteId = clienteId,
                    DataReserva = new DateTime(2024, 8, 5),
                    HoraReserva = new TimeSpan(20, 0, 0),
                    NumeroPessoas = 2,
                    Confirmada = false
                }
            };

            _dapperWrapperMock
               .Setup(d => d.QueryAsync<Reservas>(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()
                ))
                .ReturnsAsync(reservasEsperadas);

            var reservasObtidas = await _reservaRepository.ObterReservasPorClienteIdESemana(clienteId, inicioSemana, fimSemana);

            Assert.NotNull(reservasObtidas);
            Assert.Equal(2, reservasObtidas.Count());
            Assert.Contains(reservasEsperadas, r => r.Id == reservasObtidas.First().Id);
        }

        [Fact]
        public async Task Remover_DeveLancarExcecao_QuandoErroNoBancoDeDados()
        {

            var id = 1;
            var exceptionMessage = "Erro no banco de dados";

            _dapperWrapperMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()
                ))
                .ThrowsAsync(new Exception(exceptionMessage));

            var exception = await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Remover(id));
            Assert.Equal(exceptionMessage, exception.Message);
        }

        /*
        Remover_DeveRemoverReserva_QuandoIdExistente()
        */

    }
}