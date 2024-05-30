using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Controllers;
[ServiceFilter(typeof(SessionVerificationFilter))]
public class BackOfficeController : Controller
{
    private readonly ILogger<BackOfficeController> _logger;
    private readonly Import _csv;
    private readonly DevisRepository _devis;
    private readonly PayementRepository _payement;
    private readonly TravauxRepository _travaux;
    private readonly TypeFinitionRepository _finition;
    private readonly ApplicationDbContext _context;
    private readonly AdministrateurRepository _admin;
    private readonly HistoriqueDevisTravauxRepository _h_devis;




    public BackOfficeController(ILogger<BackOfficeController> logger,HistoriqueDevisTravauxRepository hd,AdministrateurRepository ad,TypeFinitionRepository tf,ApplicationDbContext a,Import c,DevisRepository d,PayementRepository p,TravauxRepository t)
    {
        _logger = logger;
        _h_devis = hd;
        _admin = ad;
        _finition = tf;
        _context = a;
        _csv = c;
        _devis = d;
        _payement = p;
        _travaux = t;
    }

    public IActionResult Index()
    {
        ViewBag.Total = _devis.TotalDevis();
        ViewBag.Payement = _payement.TotalPayement();
        return View();
    }

    public IActionResult Restore()
    {
        // Redémarrer toutes les séquences
        _context.Database.ExecuteSqlRaw(@"
            DO
            $$
            DECLARE
                seq_name text;
            BEGIN
                FOR seq_name IN SELECT sequence_name FROM information_schema.sequences WHERE sequence_schema = 'public' LOOP
                    EXECUTE 'ALTER SEQUENCE ' || seq_name || ' RESTART WITH 1';
                END LOOP;
            END;
            $$;
        ");

        // TRUNCATE des tables
        _context.Database.ExecuteSqlRaw(@"TRUNCATE TABLE client,unite, type_finition,
            type_maison, lieu, travaux, travaux_type_maison,
            devis, historique_devis_travaux, historique_devis_finition, payement,
            temp_maison_travaux, temp_devis, temp_payement CASCADE;");

        return RedirectToAction("Index", "BackOffice");
    }

    public IActionResult DevisEnCours()
    {
        ViewBag.Devis = _devis.FindAll();
        return View();
    }
    public IActionResult DetailDevisEnCours(string id_devis)
    {
        ViewBag.Devis = _h_devis.FindAllByIdDevis(id_devis);
        return View();
    }
    public IActionResult TableauDeBord()
    {
        var histogrammeData = _context._histogramme.OrderBy(h => h.Year).ThenBy(h => h.Month).ToList();
        ViewBag.Histogramme = histogrammeData;
        return View();
    }

    public IActionResult ImportDonnee()
    {
        return View();
    }
    public IActionResult ImportMaisonTravaux(IFormFile file)
    {
        if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Le fichier est vide ou n'existe pas.";
                return Ok(TempData["Error"]);
            }
            try
            {
                _csv.ImportCsvToDatabase("temp_maison_travaux",file, TemporaireMaisonTravaux.MapTemporaireMaisonTravaux);
                _csv.InsertDataFromTempMaisonTravaux();
                return RedirectToAction("Travaux","BackOffice");
            }
         
        catch (Exception ex)
        {
            // Gérez les erreurs, par exemple en affichant un message d'erreur
            string message = ex.Message;
            return View("Exception",message);  
        }          
    }

    public IActionResult ImportDevis(IFormFile file)
    {
    if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Le fichier est vide ou n'existe pas.";
                return Ok(TempData["Error"]);
            }
            try
            {
                _csv.ImportCsvToDatabase("temp_devis",file, TemporaireDevis.MapTemporaireDevis);
                _csv.InsertDataFromDevis();
                return RedirectToAction("Index","BackOffice");
            }
         
        catch (Exception ex)
        {
            // Gérez les erreurs, par exemple en affichant un message d'erreur
            string message = ex.Message;
            return View("Exception",message);  
        }
    }
    public IActionResult Travaux()
    {
        ViewBag.Travaux = _travaux.FindAll();
        return View();
    }

    public IActionResult ModifierTravaux(string id_travaux) 
    {
        ViewBag.Travaux = _travaux.FindById(id_travaux);
        return View();
    }
   public IActionResult UpdateConfirmer(double prix, string designation, string id_travaux) 
    {
        try
        {
            // Utiliser des paramètres nommés pour éviter l'injection SQL
            var sql = $"UPDATE travaux SET designation = '{designation}', prix_unitaire = '{prix}'   WHERE id = '{id_travaux}'";
            Console.WriteLine(sql);
            _context.Database.ExecuteSqlRaw(sql);
            
            // Retourner une vue ou rediriger après la mise à jour
            return RedirectToAction("Travaux", "BackOffice");
        }
        catch (Exception)
        {
            // Logger l'exception et afficher un message d'erreur approprié
            // Par exemple: _logger.LogError(ex, "Erreur lors de la mise à jour du travaux");
            
            return Ok("Error");
        }
    }
    public IActionResult Finitions() 
    {
        ViewBag.Finition = _finition.FindAll();
        return View();
    }
    public IActionResult ModifierFinition(string id_finition) 
    {
        ViewBag.Finition = _finition.FindById(id_finition);
        return View();
    }

    public IActionResult ModifierPourcentage(string id_finition,double pourcentage) 
    {
    try
    {
                // Utiliser des paramètres nommés pour éviter l'injection SQL
                var sql = $"UPDATE type_finition SET pourcentage = '{pourcentage}'  WHERE id = '{id_finition}'";
                Console.WriteLine(sql);
                _context.Database.ExecuteSqlRaw(sql);
                
                // Retourner une vue ou rediriger après la mise à jour
                return RedirectToAction("Finitions", "BackOffice");
            }
            catch (Exception)
            {
                // Logger l'exception et afficher un message d'erreur approprié
                // Par exemple: _logger.LogError(ex, "Erreur lors de la mise à jour du travaux");
                
                return Ok("Error");
            }
        }

    public IActionResult ImportPayement() 
    {
        return View();
    }
    public IActionResult ImportPayementConfirm(IFormFile file)
    {
    if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Le fichier est vide ou n'existe pas.";
                return Ok(TempData["Error"]);
            }
            try
            {
                _csv.ImportCsvToDatabase("temp_payement",file, TemporairePayement.MapTemporairePayement);
                _csv.InsertDataFromPayement();
                return RedirectToAction("Index","BackOffice");
            }
         
        catch (Exception ex)
        {
            // Gérez les erreurs, par exemple en affichant un message d'erreur
            string message = ex.Message;
            return View("Exception",message);  
        }
    }
    
}
