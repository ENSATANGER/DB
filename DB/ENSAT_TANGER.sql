create table Filiere (
	id int NOT NULL PRIMARY KEY,
	code varchar(255) UNIQUE,
	designation varchar(255)
)

create table Eleve (
	id int NOT NULL PRIMARY KEY,
	code varchar(255) UNIQUE,
	nom varchar(255),
	prenom varchar(255),
	niveau varchar(255),
	code_fil varchar(255) FOREIGN KEY REFERENCES Filiere(code)
)

create table Module
(
	id int NOT NULL PRIMARY KEY,
	code varchar(255) UNIQUE,
	designation varchar(255),
	niveau varchar(255),
	semestre varchar(255),
	code_fil varchar(255) FOREIGN KEY REFERENCES Filiere(code)
)

create table Matiere (
	id int NOT NULL PRIMARY KEY,
	code varchar(255) UNIQUE,
	designation varchar(255),
	VH float,
	code_module varchar(255) FOREIGN KEY REFERENCES Module(code)
)

create table Notes (
	id int NOT NULL PRIMARY KEY,
	code_eleve varchar(255)  FOREIGN KEY REFERENCES Eleve(code),
	code_mat varchar(255)  FOREIGN KEY REFERENCES Matiere(code),
	note float
)
create table Moyennes (
	id int NOT NULL PRIMARY KEY,
	code_eleve varchar(255) FOREIGN KEY REFERENCES Eleve(code),
	code_fil varchar(255) FOREIGN KEY REFERENCES Filiere(code),
	niveau varchar(255),
	moyenne float
)