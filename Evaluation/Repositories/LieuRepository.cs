using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class LieuRepository
{
    private readonly ApplicationDbContext _context;

    public LieuRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Lieu> FindAll()
    {
        return _context._lieu?.ToList()?? new List<Lieu>();
    }

    
}