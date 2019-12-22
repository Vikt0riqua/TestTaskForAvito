using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class MeetingParticipantsContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public MeetingParticipantsContext(DbContextOptions<MeetingParticipantsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>().HasIndex(u => new { Name = u.MeetingName, u.StartDateTime }).IsUnique();
            modelBuilder.Entity<Participant>().HasIndex(u => new { Name = u.ParticipantName, u.Email }).IsUnique();
            modelBuilder.Entity<MeetingParticipant>().HasKey(t => new { t.MId, t.PId });

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne<Meeting>(mp => mp.Meeting)
                .WithMany(m => m.MeetingParticipants)
                .HasForeignKey(mp => mp.MId);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne<Participant>(mp => mp.Participant)
                .WithMany(p => p.MeetingParticipants)
                .HasForeignKey(mp => mp.PId);
        }
    }
}