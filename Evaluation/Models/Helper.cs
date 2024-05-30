public static class Helpers
{
    // Méthode pour obtenir le profil utilisateur basé sur l'ID utilisateur à partir de la session
    public static string? GetClientProfile(HttpContext context, ClientRepository client)
    {
        // Obtenez l'ID utilisateur de la session
        string? userId = context.Session.GetString("Client");

        // Si l'ID utilisateur est présent, récupérez le profil utilisateur
        if (!string.IsNullOrEmpty(userId))
        {
            return client.FindById(userId);
        }

        // Si l'ID utilisateur est absent, renvoyez null
        return null;
    }
    
    // Méthode pour obtenir le nom complet de l'utilisateur
   
}
