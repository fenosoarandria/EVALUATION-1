using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Evaluation.Models;

namespace Evaluation.Controllers;

public class ConnexionController : Controller
{
    private readonly ILogger<ConnexionController> _logger;
    private readonly LoginRepository _login;

    public ConnexionController(ILogger<ConnexionController> logger,LoginRepository l)
    {
        _logger = logger;
        _login = l;
    }

    public IActionResult Login()
    {
        return View();
    }
    public IActionResult LoginAdmin()
    {
        return View();
    }
    public IActionResult CheckLoginClient(string contact)
    {
        string? contact_client = _login.LoginClient(contact);
        if(contact_client == null)
        {
            return RedirectToAction("Login","Connexion");
        }
        else
        {
            HttpContext.Session.SetString("Client", contact_client);
            return RedirectToAction("Index","FrontOffice");
        }
    }
    public IActionResult CheckLoginAdmin(string email,string mot_de_passe)
    {
        var id_admin = _login.LoginAdministrateur(email,mot_de_passe);
        if(id_admin == null){
            return RedirectToAction("Login","Connexion");

        }else{
            HttpContext.Session.SetString("Admin", id_admin);
            return RedirectToAction("Index","BackOffice");
        }
    }
    
    public IActionResult Deconnexion()
    {
         HttpContext.Session.Clear();
        // Redirigez l'utilisateur vers la page d'accueil ou de connexion après la déconnexion
        return RedirectToAction("Login", "Connexion");
    }

    
}
