using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PingPong.Models
{
    public class PingPongContext : DbContext
    {
        public PingPongContext (DbContextOptions<PingPongContext> options)
            : base(options)
        {
        }

        public DbSet<PingPong.Models.Player> Player { get; set; }
    }
}
