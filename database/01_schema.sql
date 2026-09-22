-- 01_schema.sql
-- Script idempotent : peut être exécuté plusieurs fois sans erreur

-- 1. Création de la base si elle n'existe pas déjà
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GestCredit')
BEGIN
    CREATE DATABASE GestCredit;
END
GO

USE GestCredit;
GO

-- 2. Table Clients
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clients')
BEGIN
    CREATE TABLE Clients (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nom NVARCHAR(100) NOT NULL,
        Ville NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NULL UNIQUE,
        DateCreation DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 3. Table Roles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE Roles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nom NVARCHAR(50) NOT NULL UNIQUE
    );
END
GO

-- 4. Table Utilisateurs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Utilisateurs')
BEGIN
    CREATE TABLE Utilisateurs (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NomUtilisateur NVARCHAR(100) NOT NULL UNIQUE,
        MotDePasseHash NVARCHAR(255) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE
    );
END
GO

-- 5. Table UtilisateursRoles (table de jonction, relation N-N)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UtilisateursRoles')
BEGIN
    CREATE TABLE UtilisateursRoles (
        UtilisateurId INT NOT NULL,
        RoleId INT NOT NULL,
        PRIMARY KEY (UtilisateurId, RoleId),
        FOREIGN KEY (UtilisateurId) REFERENCES Utilisateurs(Id),
        FOREIGN KEY (RoleId) REFERENCES Roles(Id)
    );
END
GO

-- 6. Table DemandesCredit
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DemandesCredit')
BEGIN
    CREATE TABLE DemandesCredit (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClientId INT NOT NULL,
        Montant DECIMAL(18,2) NOT NULL,
        TauxAnnuel DECIMAL(5,2) NOT NULL,
        DureeMois INT NOT NULL,
        Statut NVARCHAR(20) NOT NULL DEFAULT 'Brouillon',
        DateCreation DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (ClientId) REFERENCES Clients(Id),
        CONSTRAINT CK_DemandesCredit_Montant CHECK (Montant > 0),
        CONSTRAINT CK_DemandesCredit_Taux CHECK (TauxAnnuel > 0),
        CONSTRAINT CK_DemandesCredit_Duree CHECK (DureeMois BETWEEN 1 AND 360)
    );
END
GO

-- 7. Table Documents
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Documents')
BEGIN
    CREATE TABLE Documents (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DemandeCreditId INT NOT NULL,
        NomFichier NVARCHAR(255) NOT NULL,
        CheminStockage NVARCHAR(500) NOT NULL,
        DateUpload DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (DemandeCreditId) REFERENCES DemandesCredit(Id)
    );
END
GO

PRINT 'Schéma GestCredit créé (ou déjà existant).';