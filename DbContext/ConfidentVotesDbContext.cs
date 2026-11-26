using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ConfidentVotesAPI{
    public class AppRole : IdentityRole<Guid> { }
   public  class ConfidentVotesDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {   
        public ConfidentVotesDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PsqlDatabase");
        }
        public  DbSet<Vote> Votes {get; set;}
        public  DbSet<Election> Elections {get; set;}
        public  DbSet<Option> Options {get; set;}
        public  DbSet<Voter> Voters {get; set;}
        private readonly string? _connectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            optionsBuilder
                    .UseNpgsql($"{_connectionString}")
                    .UseSnakeCaseNamingConvention();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vote>( entity =>
            {   
               // entity.Property(e => e.Id)
                 //     .ValueGeneratedOnAdd();
            }
            );
            // person <--> election  and person <--> vote  relationships
            modelBuilder.Entity<AppUser>( entity =>
            {
                entity
                    .HasMany(e => e.ElectionsVoters)
                    .WithOne(e => e.Person);
                entity
                    .HasMany(e => e.InitiatedElections)
                    .WithOne(e => e.Initiator);
                entity // Or your custom ApplicationUser class
                    .HasIndex(u => u.NormalizedEmail)
                    .IsUnique();
            }
            );
            // Option <--> vote and Option <--> election relationships
            modelBuilder.Entity<Option>( entity =>
            {
                entity
                    .HasMany(e => e.Votes)
                    .WithOne(e => e.Option);
                entity
                    .HasOne(e => e.Election)
                    .WithMany(e => e.Options);
            }
            );
            modelBuilder.Entity<Voter>( entity =>
            {
                entity
                    .HasOne(e => e.Election)
                    .WithMany(e => e.Voters);
                entity
                    .HasIndex(e => new{e.AppUserId, e.ElectionId})
                    .IsUnique();
                entity
                    .HasOne(e => e.Vote)
                    .WithOne(e => e.Voter)
                    .HasForeignKey<Vote>(e => e.VoterID);
            }
            );
        }

    }
}