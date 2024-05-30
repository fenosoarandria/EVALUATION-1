using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class AdministrateurRepository
{
    private readonly ApplicationDbContext _context;

    public AdministrateurRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Administrateur> FindAll()
    {
        return _context._administrateur?.ToList()?? new List<Administrateur>();
    }

    public string? GetIdByEmailAndMotDePasseAdmin(string email, string mot_de_passe)
    {
        var admin = _context._administrateur?.FirstOrDefault(p => p.Email == email && p.MotDePasse == mot_de_passe);
        if (admin != null)
        {
            return admin.Id;
        }
        // Retourner une valeur par défaut si aucune personne correspondante n'est trouvée
        return null;
    }
    public void Update(Administrateur  administrateur)
    {
        if (administrateur == null)
        {
            throw new ArgumentNullException(nameof(administrateur));
        }

        _context._administrateur.Update(administrateur);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var administrateurToDelete = _context._administrateur.Find(id);
        if (administrateurToDelete != null)
        {
            _context._administrateur.Remove(administrateurToDelete);
            _context.SaveChanges();
        }
    }


}