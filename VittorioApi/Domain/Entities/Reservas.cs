using System;

namespace VittorioApiT2M.Domain.Entities
{
    public class Reservas
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public DateTime DataReserva { get; set; }
        public TimeSpan HoraReserva { get; set; }
        public int NumeroPessoas { get; set; }
        public bool Confirmada { get; set; }

        public Reservas(string nomeCliente, string emailCliente, DateTime dataReserva, TimeSpan horaReserva, int numeroPessoas, bool confirmada)
        {
            NomeCliente = nomeCliente;
            EmailCliente = emailCliente;
            DataReserva = dataReserva;
            HoraReserva = horaReserva;
            NumeroPessoas = numeroPessoas;
            Confirmada = confirmada;
        }
        public Reservas()
        {
        }
    }
}
