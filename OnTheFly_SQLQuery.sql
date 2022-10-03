create database OnTheFly;

use OnTheFly;


Create Table Passageiro(
CPF varchar(11) not null,
Situacao varchar(7) not null, --Ativa, Inativa
Sexo varchar (15) not null,
Nome varchar(50) not null,
Data_Cadastro DateTime not null,
Data_Nascimento Date not null,
Data_Ultima_Compra DateTime not null,

Constraint CPF Primary Key (CPF)
);



Create Table CompanhiaAerea(
CNPJ varchar(14) not null,
Razao_Social varchar(50) not null,
Data_Abertura Date not null,
Situacao varchar(7) not null, --Ativa, Inativa
Data_Cadastro DateTime not null,
Ultimo_Voo DateTime not null,

Constraint CNPJ Primary Key (CNPJ)
);



Create Table Aeronave(
Inscricao varchar(6) not null,
CNPJ varchar(14) not null,
Capacidade varchar(3) not null,
Ultima_Venda DateTime not null,
Data_Cadastro DateTime not null,
Situacao varchar(7) not null, --Ativa, Inativa
Velocidade_Media decimal(6,2) not null,
Distancia_Voo_Max decimal(8,2) not null,

Constraint Inscricao Primary Key(Inscricao),
FOREIGN KEY (CNPJ) References CompanhiaAerea (CNPJ)
);



Create Table Destino(
IATA varchar(3) not null,
Continente varchar(20) not null,
Pais varchar(50) not null,
Cidade varchar (50) not null,
Nome_Aeroporto varchar(100) not null,
Distancia_Rota decimal(8,2) not null,

Constraint IATA Primary Key (IATA) 
);




Create Table Voo(
ID_Voo varchar(5) not null,
Inscricao varchar(6) not null,
IATA varchar(3) not null,
Assentos_Ocupados smallint not null,
Data_Cadastro DateTime not null,
Data_Hora_Voo DateTime not null,
Situacao varchar(10) not null, -- Agendado, Cancelado, Em Rota, Finalizado
Tempo_Voo varchar (15) not null,
Data_Chegada DateTime not null

Constraint ID_Voo Primary Key(ID_Voo),
FOREIGN KEY (Inscricao) References Aeronave (Inscricao),
FOREIGN KEY (IATA) References Destino (IATA)
);



Create Table Venda(
ID_Venda int IDENTITY not null,
CPF varchar (11) not null,
Data_Venda DateTime not null,
Valor_Total decimal(10,2) not null,

constraint chkUnsigned check(Valor_Total >= 0),
Constraint ID_Venda Primary Key(ID_Venda),
FOREIGN KEY (CPF) References Passageiro (CPF)
);




Create Table Passagem(
ID_Passagem varchar(6) not null,
ID_Voo varchar(5) not null,
ID_Venda int,
Valor decimal(10,2) not null,
Data_Ultima_Operacao DateTime not null,
Situacao varchar(9) not null, -- Vendida, Reservada, Livre, Cancelada
Assento varchar(3) not null,
CPF varchar(11),

constraint cheekUnsigned check(Valor >= 0),
constraint chekUnsigned check(Assento >= 0),
Constraint PK_Passagem Primary Key(ID_Passagem, ID_Voo),
FOREIGN KEY (ID_Voo) References Voo (ID_Voo),
FOREIGN KEY (ID_Venda) References Venda (ID_Venda),
FOREIGN KEY (CPF) References Passageiro (CPF)
);



Create Table ItemVenda(
ID_Item_Venda int IDENTITY not null,
ID_Venda int not null,
Valor_Unitario decimal(9,2) not null,

Constraint ID_Item_Venda Primary Key (ID_Item_Venda),
FOREIGN KEY (ID_Venda) References Venda (ID_Venda)
);





Create Table Restritos(
CPF varchar(11) Primary Key not null
);




Create Table Bloqueados(
CNPJ varchar(14) Primary Key not null
);

