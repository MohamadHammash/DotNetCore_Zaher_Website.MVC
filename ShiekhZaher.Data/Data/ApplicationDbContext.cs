using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Zaher.Core.Entities;
using Zaher.Data.Data;

namespace Zaher.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideosList> VideosLists { get; set; }
        public DbSet<PostingCard> PostingCards { get; set; }
        public DbSet<QA> QAs { get; set; }
        public DbSet<FBook> FBooks { get; set; }
        public DbSet<Section> Sections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FBook>().HasData(
              new FBook
              {
                  Id = Guid.Parse("b303404b-881c-4bda-95e2-5207dcc2d60e"),
                  Title = "غير مصنف",
                  Description = "يحتوي على الأسئلة التي لم يتم تصنيفها بعد",
              }

              );

            modelBuilder.Entity<Section>().HasData(

                       new Section
                       {
                           Id = Guid.Parse("85761b0c-d3f5-4456-b178-b5bcb07e6078"),
                           Title = "غير مصنف",
                           Description = "يحتوي على الأسئلة التي لم يتم تصنيفها بعد",
                           FBookId = Guid.Parse("b303404b-881c-4bda-95e2-5207dcc2d60e")
                       },
                        new Section
                        {
                            Id = Guid.Parse("cb231d1c-5d5b-4ccd-ad0c-9713d4c460e5"),
                            Title = "الأسئلة المشبوهة",
                            Description = "يحتوي على الأسئلة التي على أحد المشرفين مراجعتها أو حذفها",
                            FBookId = Guid.Parse("b303404b-881c-4bda-95e2-5207dcc2d60e")
                        }


               );
        }
    }
}
