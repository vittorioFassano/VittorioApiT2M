-- Criação da tabela Cliente
CREATE TABLE Cliente (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL
);

-- Criação da tabela Reservas
CREATE TABLE Reservas (
    Id SERIAL PRIMARY KEY,
    ClienteId INT NOT NULL REFERENCES Cliente(Id) ON DELETE CASCADE,
    DataReserva DATE NOT NULL,
    HoraReserva TIME NOT NULL,
    NumeroPessoas INT NOT NULL,
    Confirmada BOOLEAN NOT NULL
);

-- Inserção de registros na tabela Cliente
INSERT INTO Cliente (Nome, Email, Senha) VALUES
('Paulo Hernique', 'paulo.hernique@example.com', '123'),
('Thiago Silva', 'thiago.silva@example.com', '123');

-- Inserção de registros na tabela Reservas
-- Reservas para João da Silva (ClienteId = 1)
INSERT INTO Reservas (ClienteId, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES
(1, '2024-08-15', '19:00:00', 4, TRUE),
(1, '2024-08-25', '20:30:00', 2, FALSE);

-- Reservas para Maria Oliveira (ClienteId = 2)
INSERT INTO Reservas (ClienteId, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES
(2, '2024-08-10', '18:00:00', 3, TRUE),
(2, '2024-08-20', '21:00:00', 5, TRUE);
