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
        public  DbSet<ElectionOption> ElectionOptions {get; set;}
        public  DbSet<ElectionVoter> ElectionVoters {get; set;}
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
                entity.Property(e => e.VoteID)
                      .ValueGeneratedOnAdd();
                entity.HasIndex(e => e.ElectionVoterID)
                      .IsUnique();
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
            // choice <--> vote and choice <--> election relationships
            modelBuilder.Entity<Choice>( entity =>
            {
                entity.Property(e => e.ChoiceID)
                    .ValueGeneratedOnAdd();
                entity
                    .HasMany(e => e.Votes)
                    .WithOne(e => e.Choice);
                entity
                    .HasOne(e => e.Election)
                    .WithMany(e => e.Choices);
            }
            );
            modelBuilder.Entity<ElectionVoter>( entity =>
            {
                entity.Property(e => e.ElectionVoterID)
                    .ValueGeneratedOnAdd();
                entity
                    .HasOne(e => e.Election)
                    .WithMany(e => e.Voters);
                entity
                    .HasIndex(e => new{e.PersonID, e.ElectionID})
                    .IsUnique();
                entity
                    .HasOne(e => e.Vote)
                    .WithOne(e => e.ElectionVoter)
                    .HasForeignKey<Vote>(e => e.ElectionVoterID);
            }
            );
        }

    }
}