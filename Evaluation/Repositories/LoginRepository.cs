public class LoginRepository
{
    
    private readonly ClientRepository _context_client;
    private readonly AdministrateurRepository _context_admin;

    public LoginRepository(ClientRepository client,AdministrateurRepository admin)
    {
        _context_client = client;
        _context_admin = admin;
    }
    public string? LoginClient(string contact)
    {
        string[] validPrefixes = { "032", "033", "034", "037", "038", "020" };
        bool isValidPrefix = validPrefixes.Any(prefix => contact.StartsWith(prefix));
        bool isValidLength = contact.Length == 10;

        if (!isValidPrefix || !isValidLength)
        {
            if (!isValidPrefix)
            {
                Console.WriteLine("Le contact ne respecte pas les préfixes valides. L'insertion est annulée.");
            }
            if (!isValidLength)
            {
                Console.WriteLine("Le contact doit contenir exactement 10 caractères. L'insertion est annulée.");
            }
            return null;
        }

        var data_contact = _context_client.FindById(contact);
        if (data_contact == null)
        {
            // Le contact n'existe pas, créez un nouveau client
           
            _context_client.Add(new Client { Contact = contact });
            // Console.WriteLine($"Nouveau client créé avec le contact : ");
            return contact;
        }
        else
        {
            Console.WriteLine($"Client trouvé avec le contact : {data_contact}");
            return data_contact;
        }
    }

    public string? LoginAdministrateur(string email,string mot_de_passe)
    {
        string? id_admin = _context_admin.GetIdByEmailAndMotDePasseAdmin(email,mot_de_passe);
        if(id_admin != null){
            return id_admin;
        }
        return null;
    }

}