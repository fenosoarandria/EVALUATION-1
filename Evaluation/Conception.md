Administrateur:
    -id,
    -nom,
    -email,
    -motDePasse
Client:
    -id,
    -telephone...
Unite:
    -id
    -nom
    
TypeTravaux:
    -id,
    -nom

Travaux:
    -id,
    -idUnite
    -idTypeTravaux
    -(foreign key)parent,
    -designation,
    -prixUnitaire

TypeMaison:
    -id,
    -nom
    -duree

TravauxTypeMaison:
    -id
    -idTypeMaison
    -iTravaux
    -quantite

TypeFinition:
    -id,
    -nom,
    -pourcentage(x,y,z)


Devis:
    -id,
    -idclient,
    -idTypeMaison,
    -idFinitionClient,
    -debutDevis
    -debutTravaux
    -montantTotal

HistoriqueDevisTravaux:
    -id
    -idDevis
    -idTravaux
    -prixUnitaire
    -quantite

HistoriqueDevisFinition:
    -id
    -idDevis
    -idFinition
    -pourcentage

Payement:
    -id,
    -idClient,
    -idDevis
    -montant(montantTotal-input),
    -datePayement