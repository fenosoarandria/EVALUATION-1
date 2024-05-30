using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionVerificationFilter : IActionFilter
{
   public void OnActionExecuting(ActionExecutingContext context)
{
    var clientSession = context.HttpContext.Session.GetString("Client");
    var adminSession = context.HttpContext.Session.GetString("Admin");
    
    // Suivi de l'état de chaque session
    bool clientSessionExists = !string.IsNullOrEmpty(clientSession);
    bool adminSessionExists = !string.IsNullOrEmpty(adminSession);

    // Vérification si les deux sessions sont vides
    if (!clientSessionExists && !adminSessionExists)
    {
        // Rediriger vers une page d'accueil générale si les deux sessions sont vides
        context.Result = new RedirectToActionResult("Login", "Connexion", null);
        return;
    }

    // Vérification si la session client est vide
    if (!clientSessionExists)
    {
        // Rediriger vers la page d'accueil du client si la session client est vide
        if (context.HttpContext.User.IsInRole("Client"))
        {
            context.Result = new RedirectToActionResult("Login", "Connexion", null);
            return;
        }
    }

    // Vérification si la session admin est vide
    if (!adminSessionExists)
    {
        // Rediriger vers la page d'accueil de l'admin si la session admin est vide
        if (context.HttpContext.User.IsInRole("Admin"))
        {
            context.Result = new RedirectToActionResult("Index", "AdminHome", null);
            return;
        }
    }

    // Les deux sessions sont valides, laisser l'action s'exécuter normalement
}




    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Ne rien faire après l'exécution de l'action
    }
}
