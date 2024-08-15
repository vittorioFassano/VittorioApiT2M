using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Infrastructure.Repositories;
using VittorioApiT2M.Domain.Repositories;
using System.Data;
using VittorioApiT2M.Application.Data;

namespace VittorioApiT2M.Tests.Unit.Repositories
{
    public class ReservaRepositoryTests
    {
        private readonly Mock<IDbConnection> _dbConnectionMock;
        private readonly Mock<IDapperWrapper> _dapperWrapperMock;
        private readonly IReservaRepository _reservaRepository;

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
            var reservaEsperada = new Reservas { Id = reservaId, NomeCliente = "Cliente Teste", EmailCliente = "cliente@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true };

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
                new Reservas { Id = 1, NomeCliente = "Cliente 1", EmailCliente = "cliente1@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true },
                new Reservas { Id = 2, NomeCliente = "Cliente 2", EmailCliente = "cliente2@test.com", DataReserva = new DateTime(2024, 8, 2), HoraReserva = new TimeSpan(20, 0, 0), NumeroPessoas = 2, Confirmada = false }
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
            var reserva = new Reservas { Id = 1, NomeCliente = "Cliente Teste", EmailCliente = "cliente@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true };

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
            var reserva = new Reservas { Id = 1, NomeCliente = "Cliente Teste", EmailCliente = "cliente@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao adicionar reserva"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Adicionar(reserva));
        }

        // Testes para Atualizar
        [Fact]
        public async Task Atualizar_DeveAtualizarReservaComSucesso_QuandoNaoHouverErros()
        {
            var reserva = new Reservas { Id = 1, NomeCliente = "Cliente Teste", EmailCliente = "cliente@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true };

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
            var reserva = new Reservas { Id = 1, NomeCliente = "Cliente Teste", EmailCliente = "cliente@test.com", DataReserva = new DateTime(2024, 8, 1), HoraReserva = new TimeSpan(18, 0, 0), NumeroPessoas = 4, Confirmada = true };
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), reserva))
                              .ThrowsAsync(new Exception("Erro simulado ao atualizar reserva"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.Atualizar(reserva));
        }

        // Testes para ObterReservasPorEmailClienteEData
        [Fact]
        public async Task ObterReservasPorEmailClienteEData_DeveRetornarReservas_QuandoParametrosSaoValidos()
        {
            var emailCliente = "cliente@test.com";
            var dataInicial = new DateTime(2024, 8, 1);
            var dataFinal = new DateTime(2024, 8, 7);

            var reservasEsperadas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    NomeCliente = "Cliente 1",
                    EmailCliente = emailCliente,
                    DataReserva = new DateTime(2024, 8, 2),
                    HoraReserva = new TimeSpan(18, 0, 0),
                    NumeroPessoas = 4,
                    Confirmada = true
                }
            };

            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(reservasEsperadas);

            var resultado = await _reservaRepository.ObterReservasPorEmailClienteEData(emailCliente, dataInicial, dataFinal);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(reservasEsperadas.First().Id, resultado.First().Id);
        }

        [Fact]
        public async Task ObterReservasPorEmailClienteEData_DeveLancarExcecao_QuandoErroOcorre()
        {
            var emailCliente = "cliente@test.com";
            var dataInicial = new DateTime(2024, 8, 1);
            var dataFinal = new DateTime(2024, 8, 7);

            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado ao obter reservas"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterReservasPorEmailClienteEData(emailCliente, dataInicial, dataFinal));
        }

        // Testes para ObterReservasPorNomeEmailESemana
        [Fact]
        public async Task ObterReservasPorNomeEmailESemana_DeveRetornarReservas_QuandoParametrosSaoValidos()
        {
            var nomeCliente = "Cliente Teste";
            var emailCliente = "cliente@test.com";
            var inicioSemana = new DateTime(2024, 8, 1);
            var fimSemana = new DateTime(2024, 8, 7);

            var reservasEsperadas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    NomeCliente = nomeCliente,
                    EmailCliente = emailCliente,
                    DataReserva = new DateTime(2024, 8, 2),
                    HoraReserva = new TimeSpan(18, 0, 0),
                    NumeroPessoas = 4,
                    Confirmada = true
                }
            };

            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(reservasEsperadas);

            var resultado = await _reservaRepository.ObterReservasPorNomeEmailESemana(nomeCliente, emailCliente, inicioSemana, fimSemana);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(reservasEsperadas.First().Id, resultado.First().Id);
        }

        [Fact]
        public async Task ObterReservasPorNomeEmailESemana_DeveLancarExcecao_QuandoErroOcorre()
        {
            var nomeCliente = "Cliente Teste";
            var emailCliente = "cliente@test.com";
            var inicioSemana = new DateTime(2024, 8, 1);
            var fimSemana = new DateTime(2024, 8, 7);

            _dapperWrapperMock.Setup(d => d.QueryAsync<Reservas>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ThrowsAsync(new Exception("Erro simulado ao obter reservas"));

            await Assert.ThrowsAsync<Exception>(() => _reservaRepository.ObterReservasPorNomeEmailESemana(nomeCliente, emailCliente, inicioSemana, fimSemana));
        }
    }
}
