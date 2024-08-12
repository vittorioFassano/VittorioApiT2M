using System;

namespace VittorioApiT2M.Application.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataReserva { get; set; }
        public TimeSpan HoraReserva { get; set; }
        public int NumeroPessoas { get; set; }
        public bool Confirmada { get; set; }
    }
}
