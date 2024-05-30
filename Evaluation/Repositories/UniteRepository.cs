using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class UniteRepository
{
    private readonly ApplicationDbContext _context;

    public UniteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Unite> FindAll()
    {
        return _context._unite?.ToList()?? new List<Unite>();
    }

    public void Add(Unite unite)
    {
        if (unite == null)
        {
            throw new ArgumentNullException(nameof(unite));
        }

        _context._unite.Add(unite);
        _context.SaveChanges();
    }

    public void Update(Unite  unite)
    {
        if (unite == null)
        {
            throw new ArgumentNullException(nameof(unite));
        }

        _context._unite.Update(unite);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var uniteToDelete = _context._unite.Find(id);
        if (uniteToDelete != null)
        {
            _context._unite.Remove(uniteToDelete);
            _context.SaveChanges();
        }
    }
}