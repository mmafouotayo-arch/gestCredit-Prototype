-- 04_objets.sql
-- Vue, procédure stockée transactionnelle, et index

USE GestCredit;
GO

-- VUE : vw_DemandesDetaillees
-- Combine chaque demande avec les infos du client, pour éviter de refaire
-- la jointure à chaque fois côté application ou reporting.

CREATE OR ALTER VIEW vw_DemandesDetaillees AS
SELECT
    d.Id AS DemandeId,
    c.Nom AS ClientNom,
    c.Ville AS ClientVille,
    d.Montant,
    d.TauxAnnuel,
    d.DureeMois,
    d.Statut,
    d.DateCreation
FROM DemandesCredit d
INNER JOIN Clients c ON d.ClientId = c.Id;
GO

-- Test de la vue
-- SELECT * FROM vw_DemandesDetaillees ORDER BY DateCreation DESC;

-- PROCÉDURE : sp_SoumettreDemande
-- Insère une nouvelle demande de crédit de façon transactionnelle.
-- Si le client n'existe pas, la transaction est annulée (ROLLBACK) :
-- aucune ligne n'est insérée.

CREATE OR ALTER PROCEDURE sp_SoumettreDemande
    @ClientId INT,
    @Montant DECIMAL(18,2),
    @TauxAnnuel DECIMAL(5,2),
    @DureeMois INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Clients WHERE Id = @ClientId)
        BEGIN
            RAISERROR('Client introuvable : Id %d n''existe pas.', 16, 1, @ClientId);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO DemandesCredit (ClientId, Montant, TauxAnnuel, DureeMois, Statut)
        VALUES (@ClientId, @Montant, @TauxAnnuel, @DureeMois, 'Brouillon');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO


-- INDEX


-- Index 1 : les requêtes filtrent très souvent par Statut (rapports, tableau de bord).
-- Sans index, chaque filtre par statut force un scan complet de la table DemandesCredit.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DemandesCredit_Statut')
BEGIN
    CREATE INDEX IX_DemandesCredit_Statut ON DemandesCredit(Statut);
END
GO

-- Index 2 : les jointures Client -> Demandes (via ClientId) sont très fréquentes.
-- Un index sur la clé étrangère accélère ces jointures, même si SQL Server
-- ne le crée pas automatiquement contrairement à la clé primaire.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DemandesCredit_ClientId')
BEGIN
    CREATE INDEX IX_DemandesCredit_ClientId ON DemandesCredit(ClientId);
END
GO
PRINT 'Vue, procedure et index crees avec succes.';

EXEC sp_SoumettreDemande @ClientId = 1, @Montant = 3000000, @TauxAnnuel = 11, @DureeMois = 18;
SELECT COUNT(*) FROM DemandesCredit;
EXEC sp_SoumettreDemande @ClientId = 9999, @Montant = 3000000, @TauxAnnuel = 11, @DureeMois = 18;
SELECT COUNT(*) FROM DemandesCredit;