
using Cards.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.API.Data
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Card> Cards { get; set; }
    }
}
