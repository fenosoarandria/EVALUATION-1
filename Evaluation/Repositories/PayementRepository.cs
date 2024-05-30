using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class PayementRepository
{
    private readonly ApplicationDbContext _context;

    public PayementRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Payement> FindAll()
    {
        return _context._payement?.ToList()?? new List<Payement>();
    }

    public void Add(Payement payement)
    {
        if (payement == null)
        {
            throw new ArgumentNullException(nameof(payement));
        }

        _context._payement.Add(payement);
        _context.SaveChanges();
    }

    public double TotalPayement(){
        return _context._payement.Sum(s => s.Montant)?? 0;
    }
    public double TotalPayementByDevis(string id_devis){
        return _context._payement.Where(d => d.IdDevis == id_devis).Sum(s => s.Montant )?? 0;
    }
}