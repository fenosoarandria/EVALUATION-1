    \c postgres 
    DROP DATABASE construction;
    CREATE DATABASE construction;
    \c construction;

    CREATE SEQUENCE seq_admin;
    CREATE TABLE administrateur(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('ADMIN', LPAD(nextval('seq_admin')::TEXT, 3, '0')),
        nom VARCHAR NOT NULL ,
        email VARCHAR UNIQUE NOT NULL ,
        mot_de_passe VARCHAR NOT NULL
    ); 

    CREATE SEQUENCE seq_client;
    CREATE TABLE client(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('CLIENT', LPAD(nextval('seq_client')::TEXT, 3, '0')),
        contact VARCHAR UNIQUE NOT NULL 
    );

    CREATE SEQUENCE seq_unite;
    CREATE TABLE unite(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('U', LPAD(nextval('seq_unite')::TEXT, 3, '0')),
        nom VARCHAR UNIQUE NOT NULL 
    );

    CREATE SEQUENCE seq_lieu;
    CREATE TABLE lieu(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('LIEU', LPAD(nextval('seq_lieu')::TEXT, 3, '0')),
        nom VARCHAR UNIQUE NOT NULL 
    );


    CREATE SEQUENCE seq_travaux;
    CREATE TABLE travaux(
        id VARCHAR PRIMARY KEY DEFAULT CONCAT('T', LPAD(nextval('seq_travaux')::TEXT, 3, '0')),
        id_unite VARCHAR REFERENCES unite(id),
        code_travaux INTEGER,
        designation VARCHAR,
        prix_unitaire NUMERIC(10, 2)
    );  
    

    CREATE SEQUENCE seq_type_maison;
    CREATE TABLE type_maison(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('TM', LPAD(nextval('seq_type_maison')::TEXT, 3, '0')),
        nom VARCHAR NOT NULL,
        duree_travaux DOUBLE PRECISION NOT NULL,
        surface NUMERIC(10,2),
        description TEXT 
    ); 


    CREATE SEQUENCE seq_travaux_type_maison;
    CREATE TABLE travaux_type_maison(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('TTM', LPAD(nextval('seq_travaux_type_maison')::TEXT, 3, '0')),
        id_travaux VARCHAR REFERENCES travaux(id),
        id_type_maison VARCHAR REFERENCES type_maison(id),
        quantite NUMERIC(10, 2) NOT NULL
    ); 


    CREATE SEQUENCE seq_type_finition;
    CREATE TABLE type_finition(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('TF', LPAD(nextval('seq_type_finition')::TEXT, 3, '0')),
        nom VARCHAR UNIQUE NOT NULL,
        pourcentage NUMERIC(10, 2) NOT NULL
    ); 

    CREATE SEQUENCE seq_devis;
    CREATE TABLE devis(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('D', LPAD(nextval('seq_devis')::TEXT, 3, '0')),
        ref_devis VARCHAR,
        id_client VARCHAR NOT NULL REFERENCES client(id),
        id_type_maison VARCHAR NOT NULL REFERENCES type_maison(id),
        id_type_finition VARCHAR NOT NULL REFERENCES type_finition(id),
        date_creation DATE NOT NULL ,
        date_debut_travaux DATE NOT NULL ,
        montant_total DOUBLE PRECISION,
        id_lieu VARCHAR REFERENCES lieu(id)
    ); 


    CREATE SEQUENCE seq_historique_devis_travaux;
    CREATE TABLE historique_devis_travaux(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('HDT', LPAD(nextval('seq_historique_devis_travaux')::TEXT, 3, '0')),
        id_devis VARCHAR NOT NULL REFERENCES devis(id),
        id_travaux VARCHAR NOT NULL REFERENCES travaux(id),
        prix_unitaire NUMERIC(10, 2) NOT NULL,
        quantite NUMERIC(10, 2) NOT NULL,
        id_unite VARCHAR REFERENCES unite(id)
    ); 


    CREATE SEQUENCE seq_historique_devis_finition;
    CREATE TABLE historique_devis_finition(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('HDF', LPAD(nextval('seq_historique_devis_finition')::TEXT, 3, '0')),
        id_devis VARCHAR NOT NULL REFERENCES devis(id),
        id_finition VARCHAR NOT NULL REFERENCES type_finition(id),
        pourcentage NUMERIC(10, 2) NOT NULL
    ); 

    CREATE SEQUENCE seq_payement;
    CREATE TABLE payement(
        id VARCHAR NOT NULL PRIMARY KEY DEFAULT CONCAT('P', LPAD(nextval('seq_payement')::TEXT, 3, '0')),
        id_devis VARCHAR NOT NULL REFERENCES devis(id),
        id_client VARCHAR NOT NULL REFERENCES client(id),
        montant NUMERIC NOT NULL,
        date_payement DATE NOT NULL
    );





    CREATE VIEW view_devis_maison_travaux as 
        SELECT 
        d.id as id_devis, d.date_creation, d.date_debut_travaux,
        c.id as id_client,
        tm.id as id_type_maison, tm.nom as maison, tm.duree_travaux,
        ttm.id_travaux, ttm.quantite, 
        t.designation, t.prix_unitaire, t.id_unite, u.nom as unite,tm.surface as surface
        FROM devis d
        JOIN client c ON d.id_client = c.id
        JOIN type_maison tm ON d.id_type_maison = tm.id
        JOIN travaux_type_maison ttm ON tm.id = ttm.id_type_maison
        JOIN travaux t ON ttm.id_travaux = t.id
        JOIN unite u ON t.id_unite = u.id;



    CREATE OR REPLACE VIEW total_travaux AS
        SELECT 
            id_devis, SUM(prix_unitaire * quantite) AS montant_total_travaux
        FROM historique_devis_travaux
        GROUP BY id_devis;
        -- 
        CREATE VIEW total_finitions AS
        SELECT 
            t.id_devis, 
            (t.montant_total_travaux + (t.montant_total_travaux * f.pourcentage / 100)) AS montant_total_finitions
        FROM total_travaux t
        JOIN historique_devis_finition f ON t.id_devis = f.id_devis;
        -- 
    CREATE OR REPLACE  VIEW montant_total_devis AS
        SELECT 
            d.id,
            d.ref_devis, 
            d.id_client, 
            d.id_type_maison, 
            d.id_type_finition, 
            d.date_creation, 
            d.date_debut_travaux, 
            f.montant_total_finitions AS montant_total
        FROM 
            devis d
        LEFT JOIN 
            total_travaux t ON d.id = t.id_devis
        LEFT JOIN 
            total_finitions f ON d.id = f.id_devis;


    -- INSERT INTO devis (ref_devis, id_client, id_type_maison, id_type_finition, date_creation, date_debut_travaux, montant_total, id_lieu) 
    -- VALUES ('D001', 'CLIENT001', 'TM001', 'TF001', '29/05/2024 10:14:25', '24/05/2024 21:00:00', 0, 'LIEU002');
    --Table temporaire
    CREATE  TABLE temp_maison_travaux (
        type_maison TEXT,
        description TEXT,
        surface NUMERIC,
        code_travaux INTEGER,
        type_travaux TEXT,
        unite TEXT,
        prix_unitaire NUMERIC,
        quantite NUMERIC(10,2),
        duree_travaux NUMERIC
    );

    CREATE  TABLE temp_devis (
        client TEXT,
        ref_devis TEXT,
        type_maison VARCHAR ,
        finition VARCHAR,
        taux_finition NUMERIC,
        date_devis DATE,
        date_debut DATE,
        lieu VARCHAR
    );
    CREATE  TABLE temp_payement (
        ref_devis VARCHAR,
        ref_payement VARCHAR,
        date_payement DATE,
        montant NUMERIC(10,2)
    );

CREATE OR REPLACE FUNCTION update_mes_donnees()
    RETURNS TRIGGER AS $$
    BEGIN

        INSERT INTO historique_devis_travaux (id_devis, id_travaux, prix_unitaire, quantite, id_unite)
            SELECT NEW.id, vdms.id_travaux, vdms.prix_unitaire, vdms.quantite, vdms.id_unite
            FROM view_devis_maison_travaux AS vdms 
            WHERE vdms.id_devis = NEW.id;

        INSERT INTO historique_devis_finition (id_devis, id_finition, pourcentage)
            SELECT NEW.id, tf.id, tf.pourcentage
            FROM type_finition tf
            JOIN devis d ON d.id_type_finition = tf.id
            WHERE d.id = NEW.id;
        UPDATE devis d
        SET montant_total = (
            SELECT m.montant_total
            FROM montant_total_devis m
            WHERE m.ref_devis = d.ref_devis
            ORDER BY m.id DESC
            LIMIT 1
        ),
        ref_devis =  NEW.id
        WHERE d.id = NEW.id;

        RETURN NEW;
    END;
    $$ LANGUAGE plpgsql;


    -- Créer une déclencheur pour appeler la fonction lors d'une insertion dans mes_donnees_test
    CREATE TRIGGER update_mes_donnees_trigger
    AFTER INSERT ON devis
    FOR EACH ROW
    EXECUTE FUNCTION update_mes_donnees();


        ALTER SEQUENCE seq_admin RESTART WITH 1;
        INSERT INTO administrateur VALUES (default,'fitahiana','f@gmail.com','1234'); --ADMIN001
        INSERT INTO administrateur VALUES (default,'maurino','m@gmail.com','1234'); --ADMIN002


    -- CREATE OR REPLACE FUNCTION calculer_montant_total()
    -- RETURNS TRIGGER AS $$
    -- DECLARE
    --     somme_prix_unitaire NUMERIC(10, 2);
    --     pourcentage_finition NUMERIC(10, 2);
    --     montant_total_devis NUMERIC(10, 2);
    -- BEGIN
    --     -- Calcul de la somme des prix unitaires des travaux pour le devis actuel
    --     SELECT SUM(dpt.prix_unitaire * dpt.quantite)
    --     INTO somme_prix_unitaire
    --     FROM devis_prix_travaux AS dpt
    --     WHERE dpt.id_type_maison = NEW.id_type_maison AND dpt.id = NEW.id;
        
    --     -- Récupération du pourcentage de la finition pour le devis actuel
    --     SELECT pourcentage
    --     INTO pourcentage_finition
    --     FROM devis_taux_finition AS dtf
    --     WHERE dtf.id_type_finition = NEW.id_type_finition AND dtf.id = NEW.id;
        
    --     -- Calcul du montant total pour le devis actuel
    --     montant_total_devis := somme_prix_unitaire + ((somme_prix_unitaire * pourcentage_finition) / 100);

    --     UPDATE devis SET montant_total = montant_total_devis WHERE id = NEW.id;
        
    --     RETURN NEW;
    -- END;
    -- $$ LANGUAGE plpgsql;

    -- -- Création d'une déclencheur pour exécuter la fonction avant chaque insertion dans la table "devis"
    -- CREATE TRIGGER calcul_montant_total_trigger
    -- AFTER INSERT ON devis
    -- FOR EACH ROW
    -- EXECUTE FUNCTION calculer_montant_total();
