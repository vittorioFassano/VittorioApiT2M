namespace VittorioApiT2M.Domain.Entities
{
    public class Cliente
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }

        public ICollection<Reservas> Reserva { get; set; } = [];
    }
}
