using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KeepNotes.Models
{
    public class KeepNotesContext : DbContext
    {
        public KeepNotesContext (DbContextOptions<KeepNotesContext> options)
            : base(options)
        {
        }

        public DbSet<KeepNotes.Models.Notes> Notes { get; set; }
        public DbSet<KeepNotes.Models.Label> Label  { get; set; }
        public DbSet<KeepNotes.Models.CheckList> Check { get; set; }

    }
}
