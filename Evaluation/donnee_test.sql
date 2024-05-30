    ALTER SEQUENCE seq_admin RESTART WITH 1;
    INSERT INTO administrateur VALUES (default,'fitahiana','f@gmail.com','1234'); --ADMIN001
    INSERT INTO administrateur VALUES (default,'maurino','m@gmail.com','1234'); --ADMIN002

    -------------------------------------------------------------------------------------------
    INSERT INTO client(contact) VALUES ('0325002518');
    -------------------------------------------------------------------------------------------
    ALTER SEQUENCE seq_unite RESTART WITH 1;
    INSERT INTO unite(nom) VALUES ('m2'); --U001
    INSERT INTO unite(nom) VALUES ('m3'); --U002

    ALTER SEQUENCE seq_lieu RESTART WITH 1;

    INSERT INTO lieu(nom) VALUES ('Andavamamba'); --U002
    INSERT INTO lieu(nom) VALUES ('Mahamasina'); --U002

    -------------------------------------------------------------------------------------------
    ALTER SEQUENCE seq_travaux RESTART WITH 1;
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U002','100','Decapage sur terrain','550000'); 
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U002','101','Dressage du plateforme','250000'); 
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U001','102','Fouille douvrage terrain ferme','360000'); 
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U001','103','Remblai douvrage','1500000'); 
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U001','104','Travaux dimplantation','120000');

    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U002','105','Chappe de 2 cm','250000'); 
    INSERT INTO travaux(id_unite,code_travaux,designation,prix_unitaire) VALUES ('U002','106','Mur de soutenement','20000'); 

    -------------------------------------------------------------------------------------------
    ALTER SEQUENCE seq_type_maison RESTART WITH 1;
    INSERT INTO type_maison(nom,duree_travaux,surface,description) VALUES ('contemporaine','50','200','10 chambre , 3 douche');
    INSERT INTO type_maison(nom,duree_travaux,surface,description) VALUES ('traditionnelle','60','160','5WC , 9 chambre');
    INSERT INTO type_maison(nom,duree_travaux,surface,description) VALUES ('moderne','120','300','6 etage , 2 douche ');
    -------------------------------------------------------------------------------------------

    ALTER SEQUENCE seq_travaux_type_maison RESTART WITH 1;
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T001','TM001','20.56'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T002','TM001','30'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T003','TM001','25'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T004','TM001','15.6'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T005','TM001','22.5'); 

    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T001','TM002','10.5'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T002','TM002','50.5'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T006','TM002','25.5'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T007','TM002','25.5'); 

    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T001','TM003','12.5'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T003','TM003','45.5'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T004','TM003','22'); 
    INSERT INTO travaux_type_maison (id_travaux, id_type_maison, quantite) VALUES ('T005','TM003','25.5'); 
    
    -------------------------------------------------------------------------------------------

    ALTER SEQUENCE seq_type_finition RESTART WITH 1;
    INSERT INTO type_finition(nom,pourcentage) VALUES('standard',0); --TF001
    INSERT INTO type_finition(nom,pourcentage) VALUES('gold',10); --TF002
    INSERT INTO type_finition(nom,pourcentage) VALUES('prenium',20); --TF003
    INSERT INTO type_finition(nom,pourcentage) VALUES('vip',30); --TF003

    -------------------------------------------------------------------------------------------

    -- UPDATE devis d
    -- SET montant_total = m.montant_total
    -- FROM montant_total_devis m
    -- WHERE d.id = 'D003';

    -- UPDATE devis d
    -- SET montant_total = (SELECT m.montant_total FROM montant_total_devis m WHERE id = 'D003')
    -- WHERE d.id = 'D003' 

    -- -- -- 
    -- INSERT INTO historique_devis_travaux (id_devis, id_travaux, prix_unitaire, quantite)
    -- SELECT  vdms.id_devis, vdms.id_travaux, vdms.prix_unitaire, vdms.quantite
    -- FROM view_devis_maison_travaux as vdms WHERE vdms.id_devis = 'D001';

    -- -- -- 
    -- -- INSERT INTO historique_devis_finition (id_devis, id_finition, pourcentage)
    -- SELECT d.id, tf.id, tf.pourcentage
    -- FROM devis d
    -- JOIN type_finition tf ON d.id_type_finition = tf.id WHERE d.id = 'D001';






