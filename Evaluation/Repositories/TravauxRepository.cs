using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class TravauxRepository
{
    private readonly ApplicationDbContext _context;

    public TravauxRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Travaux> FindAll()
    {
        return _context._travaux?
                .Include(t => t.Unite)
                .ToList()?? new List<Travaux>();
    }
    public Travaux FindById(string id)
    {

        return  _context._travaux?.Include(t => t.Unite).FirstOrDefault(a => a.Id == id)?? new Travaux();

    }

    public void Add(Travaux travaux)
    {
        if (travaux == null)
        {
            throw new ArgumentNullException(nameof(travaux));
        }

        _context._travaux.Add(travaux);
        _context.SaveChanges();
    }

    public void Update(Travaux  travaux)
    {
        if (travaux == null)
        {
            throw new ArgumentNullException(nameof(travaux));
        }

        _context._travaux.Update(travaux);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var travauxToDelete = _context._travaux.Find(id);
        if (travauxToDelete != null)
        {
            _context._travaux.Remove(travauxToDelete);
            _context.SaveChanges();
        }
    }
}