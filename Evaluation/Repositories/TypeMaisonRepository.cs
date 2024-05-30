using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class TypeMaisonRepository
{
    private readonly ApplicationDbContext _context;

    public TypeMaisonRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<TypeMaison> FindAll()
    {
        return _context._type_maison?.ToList()?? new List<TypeMaison>();
    }

    public void Add(TypeMaison type_maison)
    {
        if (type_maison == null)
        {
            throw new ArgumentNullException(nameof(type_maison));
        }

        _context._type_maison.Add(type_maison);
        _context.SaveChanges();
    }

    public void Update(TypeMaison  type_maison)
    {
        if (type_maison == null)
        {
            throw new ArgumentNullException(nameof(type_maison));
        }

        _context._type_maison.Update(type_maison);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var type_maisonToDelete = _context._type_maison.Find(id);
        if (type_maisonToDelete != null)
        {
            _context._type_maison.Remove(type_maisonToDelete);
            _context.SaveChanges();
        }
    }
}