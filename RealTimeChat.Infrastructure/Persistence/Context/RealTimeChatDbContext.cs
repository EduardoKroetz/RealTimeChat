
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Infrastructure.Persistence.Context;

public class RealTimeChatDbContext : DbContext
{
    public RealTimeChatDbContext(DbContextOptions<RealTimeChatDbContext> options) : base(options)
    {
        
    }

    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RoomParticipant> RoomParticipants { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.ChatRoom)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomParticipant>()
            .HasOne(rp => rp.User)
            .WithMany(u => u.RoomParticipants)
            .HasForeignKey(rp => rp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RoomParticipant>()
            .HasOne(rp => rp.ChatRoom)
            .WithMany(c => c.RoomParticipants)
            .HasForeignKey(rp => rp.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}