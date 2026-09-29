# Base de données GestCredit

## Prérequis

- SQL Server 2019 ou supérieur (Developer, Express ou Standard)
- SSMS (SQL Server Management Studio) ou Azure Data Studio

## Ordre d'exécution des scripts

Exécutez les scripts SQL **dans cet ordre exact**, chacun dans une fenêtre de requête SSMS connectée
au serveur local (`localhost`, authentification Windows) :

1. **`01_schema.sql`**
   Crée la base de données `GestCredit` et les 6 tables (`Clients`, `Roles`, `Utilisateurs`,
   `UtilisateursRoles`, `DemandesCredit`, `Documents`) avec leurs clés et contraintes.
   Script idempotent : peut être exécuté plusieurs fois sans erreur.

2. **`02_seed.sql`**
   Insère un jeu de données de test : 10 clients, 3 rôles, 25 demandes de crédit.
   Réinitialise les données existantes avant insertion (peut être relancé sans erreur).

3. **`03_requetes.sql`**
   12 requêtes d'exploration commentées (jointures, agrégations, filtres). Ne modifie aucune donnée,
   peut être exécuté à tout moment après le seed pour explorer la base.

4. **`04_objets.sql`**
   Crée la vue `vw_DemandesDetaillees`, la procédure stockée `sp_SoumettreDemande` (transactionnelle),
   et deux index de performance. Idempotent.

## Vérification rapide

Après avoir exécuté les 4 scripts dans l'ordre, vérifiez que tout est en place :

```sql
USE GestCredit;
SELECT COUNT(*) AS NombreClients FROM Clients;        -- doit retourner 10
SELECT COUNT(*) AS NombreDemandes FROM DemandesCredit; -- doit retourner 25 (avant tout test manuel)
SELECT * FROM vw_DemandesDetaillees;                   -- doit afficher les demandes avec le nom du client
```

## Modèle de données

Voir `modele.md` pour le schéma complet des tables, la justification de la relation N-N
(`UtilisateursRoles`) et de la 3ème forme normale.

## Structure du dossier

```
database/
README.md         - Ce fichier
modele.md          - Modèle relationnel et justification 3NF (Jour 11)
01_schema.sql       - Création de la base et des tables (Jour 12)
02_seed.sql         - Jeu de données de test (Jour 12)
03_requetes.sql     - Requêtes DML d'exploration (Jour 13)
04_objets.sql       - Vue, procédure, index (Jour 14)
```