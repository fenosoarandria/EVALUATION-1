using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class HistoriqueDevisFinitionRepository
{
    private readonly ApplicationDbContext _context;

    public HistoriqueDevisFinitionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<HistoriqueDevisFinition> FindAll()
    {
        return _context._historique_devis_finition?.ToList()?? new List<HistoriqueDevisFinition>();
    }

    public void Add(string sql)
    {
        _context.Database.ExecuteSqlRaw(sql);
    }
}