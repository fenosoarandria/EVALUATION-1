using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Evaluation.Models;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Controllers;
[ServiceFilter(typeof(SessionVerificationFilter))]
public class FrontOfficeController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly TypeMaisonRepository _type_maison;
    private readonly ClientRepository _client;
    private readonly DevisRepository _devis;
    private readonly TypeFinitionRepository _type_finition;
    private readonly HistoriqueDevisTravauxRepository _historique_travaux;
    private readonly HistoriqueDevisFinitionRepository _historique_finition;
    private readonly ILogger<FrontOfficeController> _logger;
    private readonly PayementRepository _payement;
    private readonly LieuRepository _lieu;

    public FrontOfficeController(ILogger<FrontOfficeController> logger,LieuRepository lr,ApplicationDbContext ap,TypeMaisonRepository typeM,TypeFinitionRepository typeF,DevisRepository d,ClientRepository c,HistoriqueDevisTravauxRepository ht,HistoriqueDevisFinitionRepository hf,PayementRepository p)
    {
        _logger = logger;
        _lieu = lr;
        _context = ap;
        _type_maison = typeM;
        _type_finition = typeF;
        _devis = d;
        _client = c;
        _historique_travaux = ht;
        _historique_finition = hf;
        _payement
         = p;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Devis(int? page)
    {
        ViewBag.TypeMaison = _type_maison.FindAll();
        ViewBag.TypeFinition = _type_finition.FindAll();
        ViewBag.Payement = _payement.FindAll();
        int pageSize = 3; // Nombre d'éléments par page
        int pageNumber = (page ?? 1);
        #pragma warning disable CS8604 // Possible null reference argument.
            string? id_client = _client?.FindIdByContact(HttpContext.Session.GetString("Client"));
#pragma warning restore CS8604 // Possible null reference argument. 
#pragma warning disable CS8604 // Possible null reference argument.
        ViewBag.Devis = _devis.FindAllByIdClient(id_client).ToPagedList(pageNumber, pageSize);
#pragma warning restore CS8604 // Possible null reference argument.
        return View();
    }
    public IActionResult CreeDevis(int? page)
    {
        try
        {
            int pageSize = 4; // Nombre d'éléments par page
            int pageNumber = (page ?? 1); 
            ViewBag.TypeMaison = _type_maison.FindAll().ToPagedList(pageNumber, pageSize);
            ViewBag.TypeFinition = _type_finition.FindAll();
            ViewBag.Lieu = _lieu.FindAll();
        }
        catch (Exception ex)
        {
            // Gérez les erreurs, par exemple en affichant un message d'erreur
            string message = ex.Message;
            return View("Exception",message);  
        }    
        return View();
    }
    [HttpPost]
    public IActionResult AjouteDevis(string type_maison,string type_finition,DateTime date_debut_travaux,string id_lieu)
    {
         using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                #pragma warning disable CS8604 // Possible null reference argument.
                    string? id_client = _client?.FindIdByContact(HttpContext.Session.GetString("Client"));
                #pragma warning restore CS8604 // Possible null reference argument.
                var data_devis = new Devis{
                    RefDevis = "D001",
                    IdClient = id_client,
                    IdTypeMaison = type_maison,
                    IdTypeFinition = type_finition,
                    DateCreation = DateTime.UtcNow,
                    DateDebutTravaux = date_debut_travaux.ToUniversalTime(),
                    MontantTotal = 0.0,
                    IdLieu = id_lieu
                };
               string sql = $"INSERT INTO devis (ref_devis, id_client, id_type_maison, id_type_finition, date_creation, date_debut_travaux, montant_total, id_lieu) " +
                $"VALUES ('{data_devis.RefDevis}', '{data_devis.IdClient}', '{data_devis.IdTypeMaison}', '{data_devis.IdTypeFinition}', " +
                $"'{data_devis.DateCreation}', '{data_devis.DateDebutTravaux}', {data_devis.MontantTotal}, '{data_devis.IdLieu}')";

                    _context.Database.ExecuteSqlRaw(sql);
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();

                // Gérez les erreurs, par exemple en affichant un message d'erreur
                string message = ex.Message;
                return View("Exception",message);  
            }  
        }      
            return RedirectToAction("Devis","FrontOffice");
    }
   
    public IActionResult Payement(string id_devis)
    {
        ViewBag.Devis = id_devis;
        return View();
    }
    [HttpPost]
public IActionResult AjoutePayement(double montant, DateTime date_payement, string id_devis)
{
    var errors = new Dictionary<string, string>();

    try
    {
        // Récupérer le montant total des paiements pour le devis donné
        double payement_total = _payement.TotalPayementByDevis(id_devis);
        
        // Récupérer le montant total du devis
        var montantDevis = _devis.GetMontantDevis(id_devis)?.MontantTotal;
        if (montantDevis == null)
        {
            errors["message"] = "Le montant total du devis n'a pas été trouvé.";
        }
       
        double total_faire = payement_total + montant;

        // Vérifier si le montant total du paiement ne dépasse pas le montant total du devis
        if (total_faire > montantDevis)
        {
            errors["message"] = "Vous ne pouvez pas payer plus que le montant total du devis.";
        }

        if (errors.Any())
        {
            return Json(new { success = false, errors });
        }

            // Récupérer l'ID du client à partir de la session
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            string id_client = _client.FindIdByContact(HttpContext.Session.GetString("Client"));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // Créer un nouvel objet de paiement
            var payement = new Payement
        {
            IdDevis = id_devis,
            IdClient = id_client,
            Montant = montant,
            DatePayement = date_payement.ToUniversalTime()
        };

        // Ajouter le paiement
        _payement.Add(payement);

        // Retourner une réponse JSON indiquant le succès et rediriger vers la page "Devis" du FrontOffice
        return Json(new { success = true, redirectUrl = Url.Action("Devis", "FrontOffice") });
    }
    catch (Exception ex)
    {
        // En cas d'erreur, retourner une réponse JSON indiquant l'échec avec le message d'erreur
        errors["message"] = ex.Message;
        return Json(new { success = false, errors });
    }
}


    public IActionResult GeneratePdf(string id_devis)
    {
        byte[] pdfBytes =PdfGenerator.GeneratePdfOrderForm(id_devis,_historique_travaux,_context);        
        // Renvoyer le PDF généré comme un fichier téléchargeable
        return File(pdfBytes, "application/pdf", $"{DateTime.Now}.pdf");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
