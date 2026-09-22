-- 02_seed.sql
-- Jeu de données de test : 10 clients et 25 demandes de crédit

USE GestCredit;
GO

-- Nettoyage des données existantes (pour pouvoir relancer le seed proprement)
DELETE FROM Documents;
DELETE FROM DemandesCredit;
DELETE FROM UtilisateursRoles;
DELETE FROM Utilisateurs;
DELETE FROM Roles;
DELETE FROM Clients;
GO

-- Réinitialiser les compteurs IDENTITY
DBCC CHECKIDENT ('Clients', RESEED, 0);
DBCC CHECKIDENT ('DemandesCredit', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Utilisateurs', RESEED, 0);
GO

-- 10 clients
INSERT INTO Clients (Nom, Ville, Email) VALUES
('Ahmadou Bello', 'Yaoundé', 'ahmadou.bello@example.com'),
('Fatima Njoya', 'Douala', 'fatima.njoya@example.com'),
('Jean Mballa', 'Yaoundé', 'jean.mballa@example.com'),
('Aicha Souley', 'Garoua', 'aicha.souley@example.com'),
('Paul Etoundi', 'Douala', 'paul.etoundi@example.com'),
('Marie Ngo', 'Yaoundé', 'marie.ngo@example.com'),
('Ibrahim Sali', 'Garoua', 'ibrahim.sali@example.com'),
('Christelle Abena', 'Douala', 'christelle.abena@example.com'),
('Samuel Fouda', 'Yaoundé', 'samuel.fouda@example.com'),
('Ngozi Bassong', 'Garoua', 'ngozi.bassong@example.com');
GO

-- Rôles
INSERT INTO Roles (Nom) VALUES ('Agent'), ('Analyste'), ('Administrateur');
GO

-- 25 demandes de crédit, réparties sur les 10 clients, avec des statuts variés
INSERT INTO DemandesCredit (ClientId, Montant, TauxAnnuel, DureeMois, Statut) VALUES
(1, 5000000, 12, 24, 'Approuvee'),
(1, 2000000, 10, 12, 'Brouillon'),
(2, 3500000, 14, 36, 'Soumise'),
(2, 1200000, 9, 18, 'Approuvee'),
(3, 8000000, 15, 48, 'EnAnalyse'),
(3, 750000, 8, 6, 'Rejetee'),
(4, 4500000, 11, 24, 'Approuvee'),
(4, 6000000, 13, 30, 'Soumise'),
(5, 2500000, 10, 12, 'Brouillon'),
(5, 9000000, 16, 60, 'EnAnalyse'),
(6, 1800000, 9, 18, 'Approuvee'),
(6, 3200000, 12, 24, 'Rejetee'),
(7, 5500000, 14, 36, 'Soumise'),
(7, 700000, 8, 6, 'Approuvee'),
(8, 4000000, 11, 24, 'Brouillon'),
(8, 6500000, 15, 42, 'EnAnalyse'),
(9, 2200000, 10, 12, 'Approuvee'),
(9, 3800000, 13, 30, 'Soumise'),
(10, 5200000, 12, 24, 'Rejetee'),
(10, 1500000, 9, 18, 'Approuvee'),
(1, 7200000, 15, 48, 'EnAnalyse'),
(3, 2800000, 11, 24, 'Brouillon'),
(5, 4300000, 12, 30, 'Approuvee'),
(7, 6100000, 14, 36, 'Soumise'),
(9, 1900000, 10, 12, 'Approuvee');
GO

PRINT 'Jeu de données inséré : 10 clients, 3 rôles, 25 demandes.';