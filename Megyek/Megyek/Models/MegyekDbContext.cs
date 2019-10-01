using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Megyek.Models
{
    public partial class MegyekDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public MegyekDbContext()
        {
        }

        public MegyekDbContext(DbContextOptions<MegyekDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<Participation> Participation { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Post> Post { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_Team");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.TeamId });

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Membership)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Membership_Person");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Membership)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Membership_Team");
            });

            modelBuilder.Entity<Participation>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.EventId });

                entity.Property(e => e.Participate)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participation)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participation_Event");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Participation)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participation_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Person");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Team");
            });
        }
    }
}
