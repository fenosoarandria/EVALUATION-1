using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class DevisRepository
{
    private readonly ApplicationDbContext _context;

    public DevisRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Devis> FindAll()
    {
        return _context._devis?
            .Include(d => d.TypeMaison)
            .Include(d => d.TypeFinition)
            .Include(d => d.Lieu)
            .ToList()?? new List<Devis>();
    }
    public List<Devis> FindAllByIdClient(string id)
    {
        return _context._devis?.Where(c => c.IdClient == id).ToList()?? new List<Devis>();
    }
    public Devis FindByIdDevis(string id)
    {
        return  _context._devis?
                .Include(d => d.TypeMaison)
                .Include(d => d.TypeFinition)
                .Include(d => d.Lieu)
                .FirstOrDefault(a => a.Id == id)?? new Devis();
    }

    public void AddDevis(Devis devis)
    {
        _context._devis.Add(devis);
        _context.SaveChanges();
    }

    public void Add(string sql)
    {
        _context.Database.ExecuteSqlRaw(sql);
    }
    public void Update(string sql)
    {
        _context.Database.ExecuteSqlRaw(sql);
    }
   public decimal TotalDevis()
    {
        return (decimal)(_context._devis.Sum(s => s.MontantTotal) ?? 0);
    }

    public Devis GetMontantDevis(string id_devis){
        return  _context._devis?.FirstOrDefault(a => a.Id == id_devis)?? new Devis();
    }
}