using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class HistoriqueDevisTravauxRepository
{
    private readonly ApplicationDbContext _context;

    public HistoriqueDevisTravauxRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<HistoriqueDevisTravaux> FindAll()
    {
        return _context._historique_devis_travaux?.ToList()?? new List<HistoriqueDevisTravaux>();
    }

    // using Microsoft.EntityFrameworkCore;

    public List<HistoriqueDevisTravaux> FindAllByIdDevis(string id_devis)
    {
        return _context._historique_devis_travaux
                    .Include(d => d.Travaux) // Charger l'entité Travaux
                    .Include(d => d.Unite)   // Charger l'entité Unite si nécessaire
                    .Where(d => d.IdDevis == id_devis)
                    .ToList();
    }

    public void Add(string sql)
    {
        _context.Database.ExecuteSqlRaw(sql);
    }
}