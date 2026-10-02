# Écarts entre le schéma SQL Server (Semaine 3) et le modèle EF Core (Jour 16)

## Base de données distincte

La base générée par EF Core s'appelle `GestCreditApi`, différente de `GestCredit` créée manuellement
en Semaine 3. C'est volontaire : `GestCreditApi` est générée par les migrations EF Core à partir du
code C#, tandis que `GestCredit` reste la base de référence créée en SQL pur (Jours 12-14), conservée
pour les besoins de la Semaine 3.

## Tables non encore mappées en C#

Le modèle relationnel complet (Jour 11) comprend 6 tables : `Clients`, `Roles`, `Utilisateurs`,
`UtilisateursRoles`, `DemandesCredit`, `Documents`.

À ce stade (Jour 16), seules `Clients` et `DemandesCredit` ont des entités C# correspondantes
(classes `Client` et `DemandeCredit` dans `GestCredit.Api/Models`). Les tables suivantes seront
ajoutées plus tard dans le programme :

- **`Utilisateurs`, `Roles`, `UtilisateursRoles`** : seront introduites au Jour 20 (authentification JWT),
  quand la gestion des comptes et des rôles (Agent/Analyste/Administrateur) devient nécessaire.
- **`Documents`** : sera introduite en Semaine 7 (gestion documentaire des demandes de crédit).

## Particularité technique : constructeurs et EF Core

Les classes `Client` et `DemandeCredit` utilisent des propriétés en lecture seule (`{ get; }`) pour
garantir l'immuabilité après création, conformément à l'encapsulation vue au Jour 6. EF Core a besoin
d'un setter accessible (même privé) pour pouvoir hydrater ces propriétés lors de la lecture depuis la
base de données. Toutes les propriétés persistées ont donc été changées en `{ get; private set; }`,
et un constructeur privé sans paramètre a été ajouté à chaque classe, dédié exclusivement à EF Core.
Ces changements ne modifient pas le comportement métier : seules les méthodes de la classe elle-même
(constructeurs publics, `ChangerStatut`, `Renommer`) peuvent encore modifier l'état d'un objet.