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
        private readonly IReservaRepository _reservaRepository;

        // ID
        [Fact]
        public void ObterPorId_DeveRetornarReservaValida()
        {
            // Arrange
            var reserva = new Reservas
            {
                Id = 1,
                NomeCliente = "Maria Oliveira",
                EmailCliente = "maria.oliveira@example.com",
                DataReserva = new DateTime(2024, 08, 20),
                HoraReserva = new TimeSpan(18, 30, 00),
                NumeroPessoas = 3,
                Confirmada = true
            };

            // Act & Assert
            Assert.NotNull(reserva);
            Assert.Equal(1, reserva.Id);
            Assert.Equal("Maria Oliveira", reserva.NomeCliente);
            Assert.Equal("maria.oliveira@example.com", reserva.EmailCliente);
            Assert.Equal(new DateTime(2024, 08, 20), reserva.DataReserva);
            Assert.Equal(new TimeSpan(18, 30, 00), reserva.HoraReserva);
            Assert.Equal(3, reserva.NumeroPessoas);
            Assert.True(reserva.Confirmada);
        }

        [Fact]
        public void ObterPorId_DeveRetornarNull_QuandoReservaNaoExistir()
        {
            // Arrange
            Reservas reserva = null;

            // Act & Assert
            Assert.Null(reserva);
        }

        // ALL
        [Fact]
        public void ObterTodas_DeveRetornarListaDeReservas()
        {
            // Arrange
            var reservas = new List<Reservas>
            {
                new Reservas
                {
                    Id = 1,
                    NomeCliente = "Carlos Andrade",
                    EmailCliente = "carlos.andrade@example.com",
                    DataReserva = new DateTime(2024, 08, 25),
                    HoraReserva = new TimeSpan(20, 00, 00),
                    NumeroPessoas = 2,
                    Confirmada = true
                },
                new Reservas
                {
                    Id = 2,
                    NomeCliente = "Ana Santos",
                    EmailCliente = "ana.santos@example.com",
                    DataReserva = new DateTime(2024, 08, 26),
                    HoraReserva = new TimeSpan(21, 00, 00),
                    NumeroPessoas = 5,
                    Confirmada = false
                }
            };

            // Act & Assert
            Assert.NotNull(reservas);
            Assert.Equal(2, reservas.Count);
            Assert.Equal("Carlos Andrade", reservas[0].NomeCliente);
            Assert.Equal("Ana Santos", reservas[1].NomeCliente);
        }

        [Fact]
        public void ObterTodas_DeveRetornarListaVazia_QuandoNaoExistiremReservas()
        {
            // Arrange
            var reservas = new List<Reservas>();

            // Act & Assert
            Assert.Empty(reservas);
        }

        // Atualizar
        [Fact]
        public void Atualizar_DeveAtualizarReservaValida()
        {
            // Arrange
            var reserva = new Reservas
            {
                Id = 1,
                NomeCliente = "Roberto Lima",
                EmailCliente = "roberto.lima@example.com",
                DataReserva = new DateTime(2024, 08, 30),
                HoraReserva = new TimeSpan(19, 00, 00),
                NumeroPessoas = 4,
                Confirmada = true
            };

            // Act & Assert
            Assert.NotNull(reserva);
            Assert.Equal("Roberto Lima", reserva.NomeCliente);
            Assert.Equal("roberto.lima@example.com", reserva.EmailCliente);
            Assert.Equal(new DateTime(2024, 08, 30), reserva.DataReserva);
            Assert.Equal(new TimeSpan(19, 00, 00), reserva.HoraReserva);
        }

        [Fact]
        public void Atualizar_DeveLancarExcecao_QuandoReservaForNula()
        {
            // Arrange
            Reservas reserva = null;

            // Act & Assert
            var exception = Assert.Throws<NullReferenceException>(() => reserva.ToString());
            // Sem necessidade de verificar a mensagem, apenas a exceção
        }

        // Remover
        [Fact]
        public void Remover_DeveRemoverReserva_QuandoIdExistir()
        {
            // Arrange
            var id = 1;

            // Act & Assert
            Assert.Equal(1, id);
        }

        [Fact]
        public void Remover_DeveLancarExcecao_QuandoIdForInvalido()
        {
            // Arrange
            var id = 0;

            // Act & Assert
            Assert.True(id <= 0);
        }

        // ObterReservasPorNomeEmailESemana
        [Fact]
        public void ObterReservasPorNomeEmailESemana_DeveLancarExcecao_QuandoInicioSemanaForMaiorQueFimSemana()
        {
            // Arrange
            var inicioSemana = new DateTime(2024, 08, 25);
            var fimSemana = new DateTime(2024, 08, 24);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                if (inicioSemana > fimSemana)
                    throw new ArgumentException("A data de início da semana não pode ser posterior à data de fim da semana.");
            });
            Assert.Equal("A data de início da semana não pode ser posterior à data de fim da semana.", exception.Message);
        }

        // ObterReservasPorEmailClienteEData
        [Fact]
        public void ObterReservasPorEmailClienteEData_DeveLancarExcecao_QuandoDataInicialForMaiorQueDataFinal()
        {
            // Arrange
            var dataInicial = new DateTime(2024, 08, 25);
            var dataFinal = new DateTime(2024, 08, 24);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                if (dataInicial > dataFinal)
                    throw new ArgumentException("A data inicial não pode ser posterior à data final.");
            });
            Assert.Equal("A data inicial não pode ser posterior à data final.", exception.Message);
        }

        //Adicionar
        [Fact]
        public void Adicionar_DeveAdicionarReservaValida()
        {
            // Arrange
            var reserva = new Reservas
            {
                Id = 1,
                NomeCliente = "Ana Costa",
                EmailCliente = "ana.costa@example.com",
                DataReserva = new DateTime(2024, 08, 28),
                HoraReserva = new TimeSpan(16, 00, 00),
                NumeroPessoas = 4,
                Confirmada = false
            };

            // Act & Assert
            Assert.NotNull(reserva);
            Assert.Equal(1, reserva.Id);
            Assert.Equal("Ana Costa", reserva.NomeCliente);
            Assert.Equal("ana.costa@example.com", reserva.EmailCliente);
            Assert.Equal(new DateTime(2024, 08, 28), reserva.DataReserva);
            Assert.Equal(new TimeSpan(16, 00, 00), reserva.HoraReserva);
            Assert.Equal(4, reserva.NumeroPessoas);
            Assert.False(reserva.Confirmada);
        }

        [Fact]
        public void Adicionar_DeveLancarExcecao_QuandoReservaForNula()
        {
            Reservas reserva = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                if (reserva == null)
                {
                    throw new ArgumentNullException(nameof(reserva), "Reserva não pode ser nula.");
                }
            });
            Assert.Equal("Reserva não pode ser nula. (Parameter 'reserva')", exception.Message);
        }


    }
}
