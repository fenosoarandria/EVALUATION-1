using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class TravauxTypeMaisonRepository
{
    private readonly ApplicationDbContext _context;

    public TravauxTypeMaisonRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<TravauxTypeMaison> FindAll()
    {
        return _context._travaux_type_maison?.ToList()?? new List<TravauxTypeMaison>();
    }

    public void Add(TravauxTypeMaison travaux_type_maison)
    {
        if (travaux_type_maison == null)
        {
            throw new ArgumentNullException(nameof(travaux_type_maison));
        }

        _context._travaux_type_maison.Add(travaux_type_maison);
        _context.SaveChanges();
    }

    public void Update(TravauxTypeMaison  travaux_type_maison)
    {
        if (travaux_type_maison == null)
        {
            throw new ArgumentNullException(nameof(travaux_type_maison));
        }

        _context._travaux_type_maison.Update(travaux_type_maison);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var travaux_type_maisonToDelete = _context._travaux_type_maison.Find(id);
        if (travaux_type_maisonToDelete != null)
        {
            _context._travaux_type_maison.Remove(travaux_type_maisonToDelete);
            _context.SaveChanges();
        }
    }

       
}