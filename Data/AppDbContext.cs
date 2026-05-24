using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Community> Communities { get; set; }

    public DbSet<CommunityMember> CommunityMembers { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<PostVote> PostVotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);

        ConfigureCommunities(modelBuilder);

        ConfigureCommunityMembers(modelBuilder);

        ConfigurePosts(modelBuilder);

        ConfigureComments(modelBuilder);

        ConfigurePostVotes(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }

    private static void ConfigureCommunities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Community>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.OwnedCommunities)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureCommunityMembers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommunityMember>()
            .HasKey(cm => new { cm.UserId, cm.CommunityId });

        modelBuilder.Entity<CommunityMember>()
            .HasOne(cm => cm.User)
            .WithMany(u => u.CommunityMembers)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CommunityMember>()
            .HasOne(cm => cm.Community)
            .WithMany(c => c.CommunityMembers)
            .HasForeignKey(cm => cm.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePosts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Community)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureComments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePostVotes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostVote>()
            .HasKey(v => new { v.UserId, v.PostId });

        modelBuilder.Entity<PostVote>()
            .HasOne(v => v.User)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PostVote>()
            .HasOne(v => v.Post)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PostVote>()
            .Property(v => v.VoteType)
            .IsRequired();
    }
}