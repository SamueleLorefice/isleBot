using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace IsleBot;

public class DbManager : DbContext {
    public DbSet<User> Players { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Match> Matches { get; set; }

    public string dbpath { get; } = "database.db";

    public DbManager() {
        var fullPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + dbpath;
        dbpath = fullPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseSqlite($"Data Source={dbpath}");
}