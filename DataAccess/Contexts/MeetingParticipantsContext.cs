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
            modelBuilder.Entity<MeetingParticipant>()
                .HasKey(t => new { t.MeetingId, t.ParticipantId });

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(pt => pt.Meeting)
                .WithMany(p => p.MeetingParticipants)
                .HasForeignKey(pt => pt.MeetingId);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(pt => pt.Participant)
                .WithMany(t => t.MeetingParticipants)
                .HasForeignKey(pt => pt.ParticipantId);

            modelBuilder.Entity<Meeting>().HasIndex(u => new {u.Name, u.StartDateTime}).IsUnique();
            modelBuilder.Entity<Participant>().HasIndex(u => new { u.Name, u.Email}).IsUnique();
        }
    }
}