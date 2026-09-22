-- 03_requetes.sql
-- 12 requêtes d'exploration sur la base GestCredit

USE GestCredit;
GO

-- 1. Liste de toutes les demandes avec le nom du client (jointure simple)
SELECT d.Id, c.Nom AS Client, d.Montant, d.Statut
FROM DemandesCredit d
INNER JOIN Clients c ON d.ClientId = c.Id
ORDER BY d.Id;
GO

-- 2. Nombre de demandes par client (LEFT JOIN pour ne pas perdre les clients sans demande)
SELECT c.Nom, COUNT(d.Id) AS NombreDemandes
FROM Clients c
LEFT JOIN DemandesCredit d ON c.Id = d.ClientId
GROUP BY c.Nom
ORDER BY NombreDemandes DESC;
GO

-- 3. Clients sans aucune demande (LEFT JOIN + IS NULL)
SELECT c.Nom, c.Ville
FROM Clients c
LEFT JOIN DemandesCredit d ON c.Id = d.ClientId
WHERE d.Id IS NULL;
GO

-- 4. Encours total (somme des montants) par statut
SELECT Statut, COUNT(*) AS Nombre, SUM(Montant) AS EncoursTotal
FROM DemandesCredit
GROUP BY Statut
ORDER BY EncoursTotal DESC;
GO

-- 5. Les 5 plus gros dossiers (montant le plus élevé)
SELECT TOP 5 d.Id, c.Nom AS Client, d.Montant, d.Statut
FROM DemandesCredit d
INNER JOIN Clients c ON d.ClientId = c.Id
ORDER BY d.Montant DESC;
GO

-- 6. Taux d'approbation global (pourcentage de demandes Approuvee sur le total)
SELECT
    COUNT(CASE WHEN Statut = 'Approuvee' THEN 1 END) AS NombreApprouvees,
    COUNT(*) AS NombreTotal,
    CAST(COUNT(CASE WHEN Statut = 'Approuvee' THEN 1 END) AS DECIMAL(5,2)) * 100.0 / COUNT(*) AS TauxApprobationPourcent
FROM DemandesCredit;
GO

-- 7. Dossiers en analyse depuis plus de 30 jours (fonction de date DATEDIFF)
SELECT d.Id, c.Nom AS Client, d.DateCreation, DATEDIFF(DAY, d.DateCreation, GETDATE()) AS JoursEcoules
FROM DemandesCredit d
INNER JOIN Clients c ON d.ClientId = c.Id
WHERE d.Statut = 'EnAnalyse'
  AND DATEDIFF(DAY, d.DateCreation, GETDATE()) > 30;
GO

-- 8. Montant moyen des demandes par ville (LEFT JOIN pour garder les villes sans demande active)
SELECT c.Ville, AVG(d.Montant) AS MontantMoyen
FROM Clients c
LEFT JOIN DemandesCredit d ON c.Id = d.ClientId
GROUP BY c.Ville
ORDER BY MontantMoyen DESC;
GO

-- 9. Clients ayant plus d'une demande (GROUP BY + HAVING)
SELECT c.Nom, COUNT(d.Id) AS NombreDemandes
FROM Clients c
INNER JOIN DemandesCredit d ON c.Id = d.ClientId
GROUP BY c.Nom
HAVING COUNT(d.Id) > 1
ORDER BY NombreDemandes DESC;
GO

-- 10. Demandes triées par durée décroissante, avec mensualité approximative (calcul en SQL)
SELECT
    Id,
    Montant,
    TauxAnnuel,
    DureeMois,
    Montant * (TauxAnnuel / 1200.0) / (1 - POWER(1 + TauxAnnuel / 1200.0, -DureeMois)) AS MensualiteEstimee
FROM DemandesCredit
ORDER BY DureeMois DESC;
GO

-- 11. Toutes les demandes avec statut connu, y compris les clients dont l'email est NULL (COALESCE)
SELECT c.Nom, COALESCE(c.Email, 'Email non renseigné') AS Email, d.Statut
FROM Clients c
INNER JOIN DemandesCredit d ON c.Id = d.ClientId
ORDER BY c.Nom;
GO

-- 12. Nombre total de documents par demande (LEFT JOIN car certaines demandes n'ont aucun document)
SELECT d.Id AS DemandeId, c.Nom AS Client, COUNT(doc.Id) AS NombreDocuments
FROM DemandesCredit d
INNER JOIN Clients c ON d.ClientId = c.Id
LEFT JOIN Documents doc ON doc.DemandeCreditId = d.Id
GROUP BY d.Id, c.Nom
ORDER BY NombreDocuments DESC;
GO