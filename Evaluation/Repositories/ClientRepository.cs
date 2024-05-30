using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Client> FindAll()
    {
        return _context._client?.ToList()?? new List<Client>();
    }

   public string? FindById(string contact)
    {
        var client = _context._client?.FirstOrDefault(a => a.Contact == contact);
        return client?.Contact;
    }
    
   public string? FindIdByContact(string contact)
    {
        var client = _context._client?.FirstOrDefault(a => a.Contact == contact);
        return client?.Id;
    }


    public void Add(Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        _context._client.Add(client);
        _context.SaveChanges();
    }

    
}