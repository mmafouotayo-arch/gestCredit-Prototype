# Modèle de données - GestCredit

## Vue d'ensemble

L'application gère des demandes de crédit soumises par des clients, traitées par des utilisateurs internes
(Agents, Analystes, Administrateurs), avec des pièces jointes associées à chaque demande.

## Schéma

### Clients
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INT | PK, IDENTITY |
| Nom | NVARCHAR(100) | NOT NULL |
| Ville | NVARCHAR(100) | NOT NULL |
| Email | NVARCHAR(150) | UNIQUE |
| DateCreation | DATETIME2 | NOT NULL, DEFAULT GETDATE() |

### Roles
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INT | PK, IDENTITY |
| Nom | NVARCHAR(50) | NOT NULL, UNIQUE |

### Utilisateurs
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INT | PK, IDENTITY |
| NomUtilisateur | NVARCHAR(100) | NOT NULL, UNIQUE |
| MotDePasseHash | NVARCHAR(255) | NOT NULL |
| Email | NVARCHAR(150) | NOT NULL, UNIQUE |

### UtilisateursRoles (table de jonction — relation N-N)
| Colonne | Type | Contraintes |
|---|---|---|
| UtilisateurId | INT | PK (composite), FK -> Utilisateurs.Id |
| RoleId | INT | PK (composite), FK -> Roles.Id |

### DemandesCredit
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INT | PK, IDENTITY |
| ClientId | INT | FK -> Clients.Id, NOT NULL |
| Montant | DECIMAL(18,2) | NOT NULL, CHECK > 0 |
| TauxAnnuel | DECIMAL(5,2) | NOT NULL, CHECK > 0 |
| DureeMois | INT | NOT NULL, CHECK BETWEEN 1 AND 360 |
| Statut | NVARCHAR(20) | NOT NULL, DEFAULT 'Brouillon' |
| DateCreation | DATETIME2 | NOT NULL, DEFAULT GETDATE() |

### Documents
| Colonne | Type | Contraintes |
|---|---|---|
| Id | INT | PK, IDENTITY |
| DemandeCreditId | INT | FK -> DemandesCredit.Id, NOT NULL |
| NomFichier | NVARCHAR(255) | NOT NULL |
| CheminStockage | NVARCHAR(500) | NOT NULL |
| DateUpload | DATETIME2 | NOT NULL, DEFAULT GETDATE() |

## Relation N-N : UtilisateursRoles

Un utilisateur peut cumuler plusieurs rôles (par exemple Agent et Analyste), et un rôle peut être attribué
à plusieurs utilisateurs. Cette relation many-to-many ne peut pas être représentée par une simple colonne
`RoleId` dans `Utilisateurs` (qui ne permettrait qu'un seul rôle par utilisateur). La table `UtilisateursRoles`
résout ce problème : chaque ligne associe un `UtilisateurId` à un `RoleId`, et la clé primaire composite
empêche les doublons (un même couple utilisateur/rôle ne peut exister qu'une fois).

## Justification de la 3ème forme normale (3NF)

**1NF (valeurs atomiques) :** chaque colonne contient une seule valeur indivisible. Par exemple, un client
n'a qu'une seule valeur de `Ville` par ligne — on ne stocke jamais une liste de villes dans une cellule.

**2NF (dépendance totale à la clé) :** dans les tables à clé simple (Clients, Roles, Utilisateurs,
DemandesCredit, Documents), la 2NF est automatiquement respectée car il n'y a qu'une seule colonne dans
la clé primaire. Dans la table à clé composite `UtilisateursRoles`, il n'existe aucune colonne
supplémentaire qui ne dépendrait que d'une partie de la clé (`UtilisateurId` seul ou `RoleId` seul) :
la table ne contient que les deux colonnes de la clé elle-même.

**3NF (pas de dépendance transitive) :** aucune colonne non-clé ne dépend d'une autre colonne non-clé.
Par exemple, dans `DemandesCredit`, `Montant` dépend uniquement de `Id` (l'identifiant de la demande),
jamais de `ClientId` ou d'une autre colonne. De même, `Ville` dans `Clients` ne dépend que de `Id`, pas du
`Nom` du client. Chaque table ne décrit qu'un seul concept métier (un client, une demande, un document...),
ce qui évite les redondances et les anomalies de mise à jour.