using System;
using System.ComponentModel.DataAnnotations;

namespace VittorioApiT2M.Application.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail do cliente é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail do cliente deve ser válido.")]
        public string EmailCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data da reserva é obrigatória.")]
        public DateTime DataReserva { get; set; }

        [Required(ErrorMessage = "A hora da reserva é obrigatória.")]
        public TimeSpan HoraReserva { get; set; }

        [Required(ErrorMessage = "O número de pessoas é obrigatório.")]
        public int NumeroPessoas { get; set; }

        [Required(ErrorMessage = "A confirmação da reserva é obrigatória.")]
        public bool Confirmada { get; set; }
    }
}
