using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class TypeFinitionRepository
{
    private readonly ApplicationDbContext _context;

    public TypeFinitionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<TypeFinition> FindAll()
    {
        return _context._type_finition?.ToList()?? new List<TypeFinition>();
    }
    public TypeFinition FindById(string id)
    {
        return  _context._type_finition?.FirstOrDefault(a => a.Id == id)?? new TypeFinition();
    }

    public void Add(TypeFinition type_finition)
    {
        if (type_finition == null)
        {
            throw new ArgumentNullException(nameof(type_finition));
        }

        _context._type_finition.Add(type_finition);
        _context.SaveChanges();
    }

    public void Update(TypeFinition  type_finition)
    {
        if (type_finition == null)
        {
            throw new ArgumentNullException(nameof(type_finition));
        }

        _context._type_finition.Update(type_finition);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var type_finitionToDelete = _context._type_finition.Find(id);
        if (type_finitionToDelete != null)
        {
            _context._type_finition.Remove(type_finitionToDelete);
            _context.SaveChanges();
        }
    }
}