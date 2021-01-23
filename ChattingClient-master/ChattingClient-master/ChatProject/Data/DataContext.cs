using ChatProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Models.Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<History> HistoryList { get; set; }
    }
}
