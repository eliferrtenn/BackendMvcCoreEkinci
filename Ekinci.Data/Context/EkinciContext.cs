using Ekinci.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ekinci.Data.Context
{
    public class EkinciContext : DbContext
    {
        public EkinciContext(DbContextOptions<EkinciContext> options) : base(options)
        {
        }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<CommercialArea> CommercialAreas { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<HumanResource> HumanResources { get; set; }
        public DbSet<IdentityGuide> IdentityGuides { get; set; }
        public DbSet<Intro> Intros { get; set; }
        public DbSet<Kvkk> Kvkks { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementPhotos> AnnouncementPhotos { get; set; }
        public DbSet<Press> Press { get; set; }
        public DbSet<ProjectPhoto> ProjectPhotos { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Sustainability> Sustainabilities { get; set; }
        public DbSet<TechnicalServiceDemand> TechnicalServiceDemands{ get; set; }
        public DbSet<TechnicalServiceName> TechnicalServiceNames{ get; set; }
        public DbSet<TechnicalServiceStaff> TechnicalServiceStaffs{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
    }
}