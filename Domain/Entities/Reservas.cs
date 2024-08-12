using System;

namespace VittorioApiT2M.Domain.Entities
{
    public class Reservas
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public required Cliente Cliente { get; set; }
        public required DateTime DataReserva { get; set; }
        public required TimeSpan HoraReserva { get; set; }
        public required int NumeroPessoas { get; set; }
        public required bool Confirmada { get; set; }
    }
}
