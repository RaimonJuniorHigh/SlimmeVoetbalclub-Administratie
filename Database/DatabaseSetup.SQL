-- CREATE DATABASE VoetbalClubDB;
-- GO
-- USE VoetbalClubDB;

-- 2. Tabellen verwijderen als ze al bestaan (voor een schone start)
DROP TABLE IF EXISTS Wedstrijd;
DROP TABLE IF EXISTS Betaling;
DROP TABLE IF EXISTS Lidmaatschap;
DROP TABLE IF EXISTS Team;

-- 3. Tabel: Team
CREATE TABLE Team (
    TeamID INT PRIMARY KEY IDENTITY(1,1),
    TeamNaam VARCHAR(50) NOT NULL,
    Categorie VARCHAR(20) -- bijv. O19, Senioren, Vrouwen
);

-- 4. Tabel: Lidmaatschap (Ledenregistratie)
CREATE TABLE Lidmaatschap (
    LidID INT PRIMARY KEY IDENTITY(1,1),
    Voornaam VARCHAR(50) NOT NULL,
    Achternaam VARCHAR(50) NOT NULL,
    Geboortedatum DATE NOT NULL,
    Email VARCHAR(100),
    TeamID INT FOREIGN KEY REFERENCES Team(TeamID),
    IsActief BIT DEFAULT 1 -- 1 = Actief, 0 = Gestopt
);

-- 5. Tabel: Betaling (Contributie-inning)
CREATE TABLE Betaling (
    BetalingID INT PRIMARY KEY IDENTITY(1,1),
    LidID INT FOREIGN KEY REFERENCES Lidmaatschap(LidID),
    Bedrag DECIMAL(10, 2) NOT NULL,
    Betaaldatum DATE NULL, -- NULL betekent nog niet betaald
    Vervaldatum DATE NOT NULL,
    Status AS (CASE WHEN Betaaldatum IS NULL AND GETDATE() > Vervaldatum THEN 'Achterstallig' 
                    WHEN Betaaldatum IS NULL THEN 'Openstaand' 
                    ELSE 'Betaald' END)
);

-- 6. Tabel: Wedstrijd (Wedstrijdplanning & Veldbeheer)
CREATE TABLE Wedstrijd (
    WedstrijdID INT PRIMARY KEY IDENTITY(1,1),
    ThuisTeamID INT FOREIGN KEY REFERENCES Team(TeamID),
    UitTeamNaam VARCHAR(100) NOT NULL,
    DatumTijd DATETIME NOT NULL,
    VeldNummer INT NOT NULL,
    Status VARCHAR(20) DEFAULT 'Gepland' -- bijv. Gepland, Afgelast, Gespeeld
);

-- 7. Dummy Data toevoegen
INSERT INTO Team (TeamNaam, Categorie) VALUES ('Raimon 1', 'Senioren'), ('Raimon O19', 'Jeugd');

INSERT INTO Lidmaatschap (Voornaam, Achternaam, Geboortedatum, Email, TeamID) 
VALUES ('Mark', 'Evans', '2005-05-10', 'mark@raimon.nl', 1),
       ('Axel', 'Blaze', '2006-08-15', 'axel@raimon.nl', 1);

INSERT INTO Betaling (LidID, Bedrag, Vervaldatum) 
VALUES (1, 150.00, '2026-04-01'), (2, 150.00, '2026-03-01');

INSERT INTO Wedstrijd (ThuisTeamID, UitTeamNaam, DatumTijd, VeldNummer)
VALUES (1, 'Royal Academy', '2026-04-10 14:30:00', 1);