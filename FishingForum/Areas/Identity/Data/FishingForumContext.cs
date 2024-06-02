using FishingForum.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FishingForum.Models;
using System.Reflection.Emit;

namespace FishingForum.Data;

public class FishingForumContext : IdentityDbContext<FishingForumUser>
{
    public FishingForumContext(DbContextOptions<FishingForumContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);


        builder.Entity<PostUserPicture>()
                .HasKey(pup => new { pup.PostId, pup.UserPictureId });

        builder.Entity<PostUserPicture>()
            .HasOne(pup => pup.Post)
            .WithMany(p => p.PostUserPictures)
            .HasForeignKey(pup => pup.PostId);

        builder.Entity<PostUserPicture>()
            .HasOne(pup => pup.UserPicture)
            .WithMany(up => up.PostUserPictures)
            .HasForeignKey(pup => pup.UserPictureId);

        builder.Entity<Post>()
        .HasOne(p => p.FishingForumUser)
        .WithMany()
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.NoAction); // If a User is deleted don't delete post

        builder.Entity<Post>()
        .HasOne(p => p.Thread)
        .WithMany()
        .HasForeignKey(p => p.ThreadId)
        .OnDelete(DeleteBehavior.NoAction); // If a Thread is deleted don't delete post

    }

    public DbSet<FishingForum.Models.Category> Category { get; set; } = default!;

    public DbSet<FishingForum.Models.ForumPicture> ForumPicture { get; set; } = default!;

    public DbSet<FishingForum.Models.SubCategory> SubCategory { get; set; } = default!;

    public DbSet<FishingForum.Models.Thread> Thread { get; set; } = default!;

    public DbSet<FishingForum.Models.Post> Post { get; set; } = default!;

    public DbSet<FishingForum.Models.UserPicture> UserPicture { get; set; } = default!;

    public DbSet<FishingForum.Models.PostUserPicture> PostUserPicture { get; set; } = default!;

public DbSet<FishingForum.Models.Message> Message { get; set; } = default!;

public DbSet<FishingForum.Models.ProfilePicture> ProfilePicture { get; set; } = default!;
}
