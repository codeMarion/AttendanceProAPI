using AttendanceProAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Data
{
    /// <summary>
    /// This class is used as a migration layer between C# domain classes and database through Entity Framework Core.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<FileRow> Students { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
    }
}
