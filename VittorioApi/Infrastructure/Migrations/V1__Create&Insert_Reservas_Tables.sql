-- Criação da tabela Reservas
CREATE TABLE Reservas (
    Id SERIAL PRIMARY KEY,
    NomeCliente VARCHAR(255) NOT NULL,
    EmailCliente VARCHAR(255) NOT NULL,
    DataReserva DATE NOT NULL,
    HoraReserva TIME NOT NULL,
    NumeroPessoas INT NOT NULL,
    Confirmada BOOLEAN NOT NULL
);

-- Inserção de registros na tabela Reservas
-- Registros para Paulo Henrique
INSERT INTO Reservas (NomeCliente, EmailCliente, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES
('Paulo Henrique', 'paulo.henrique@example.com', '2024-08-15', '19:00:00', 4, TRUE),
('Paulo Henrique', 'paulo.henrique@example.com', '2024-08-30', '18:00:00', 3, FALSE);

-- Registros para Thiago Silva
INSERT INTO Reservas (NomeCliente, EmailCliente, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES
('Thiago Silva', 'thiago.silva@example.com', '2024-08-20', '21:00:00', 5, TRUE),
('Thiago Silva', 'thiago.silva@example.com', '2024-08-28', '20:00:00', 2, TRUE);

-- Registros para Fred Guedes
INSERT INTO Reservas (NomeCliente, EmailCliente, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES
('Fred Guedes', 'fred.guedes@example.com', '2024-08-22', '19:30:00', 4, TRUE),
('Fred Guedes', 'fred.guedes@example.com', '2024-08-29', '17:00:00', 6, FALSE);
