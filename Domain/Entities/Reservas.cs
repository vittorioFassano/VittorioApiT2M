using System;

namespace VittorioApiT2M.Domain.Entities
{
    public class Reservas
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Clientes? Cliente { get; set; }
        public DateTime DataReserva { get; set; }
        public TimeSpan HoraReserva { get; set; }
        public int NumeroPessoas { get; set; }
        public bool Confirmada { get; set; }

        public Reservas(int clienteId, DateTime dataReserva, TimeSpan horaReserva, int numeroPessoas, bool confirmada, Clientes cliente)
        {
            ClienteId = clienteId;
            DataReserva = dataReserva;
            HoraReserva = horaReserva;
            NumeroPessoas = numeroPessoas;
            Confirmada = confirmada;
            Cliente = cliente;
        }

        public Reservas()
        {

        }
    }

}
