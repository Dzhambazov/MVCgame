using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace FirebirdWars.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : User
    {
        private ICollection<UserUnit> units;
        private ICollection<UserBuilding> buildings;
        private ICollection<UnitsInProgress> unitsInProgress;
        private ICollection<BuildingsInProgress> buildingsInProgress;
        private ICollection<Message> messages;

        public ApplicationUser()
        {
            this.units = new HashSet<UserUnit>();
            this.buildings = new HashSet<UserBuilding>();
            this.unitsInProgress = new HashSet<UnitsInProgress>();
            this.buildingsInProgress = new HashSet<BuildingsInProgress>();
            this.messages = new HashSet<Message>();
        }

        public string Email { get; set; }

        public int Wood { get; set; }

        public int Food { get; set; }

        public int Iron { get; set; }

        public int Population { get; set; }

        public int MaxPopulation { get; set; }

        public int LocationX { get; set; }

        public int LocationY { get; set; }

        public DateTime? LastOnline { get; set; }

        public virtual ICollection<UserUnit> Units
        {
            get
            {
                return this.units;
            }
            set
            {
                this.units = value;
            }
        }

        public virtual ICollection<UserBuilding> Buildings
        {
            get
            {
                return this.buildings;
            }
            set
            {
                this.buildings = value;
            }
        }

        public virtual ICollection<UnitsInProgress> UnitsInProgress
        {
            get
            {
                return this.unitsInProgress;
            }
            set
            {
                this.unitsInProgress = value;
            }
        }

        public virtual ICollection<BuildingsInProgress> BuildingsInProgress
        {
            get
            {
                return this.buildingsInProgress;
            }
            set
            {
                this.buildingsInProgress = value;
            }
        }

        public virtual ICollection<Message> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserClaim, UserSecret, UserLogin, Role, UserRole, Token, UserManagement>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuildingRequirement>()
                        .HasRequired(br => br.RequiredBuilding)
                        .WithMany()
                        .HasForeignKey(br => br.RequiredBuildingId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<BuildingRequirement>()
                        .HasRequired(br => br.Building)
                        .WithMany(br => br.BuildingRequirements)
                        .HasForeignKey(br => br.BuildingId)
                        .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<UserUnit> UsersUnits { get; set; }

        public DbSet<UserBuilding> UsersBuildings { get; set; }

        public DbSet<BuildingRequirement> BuildingRequirements { get; set; }

        public DbSet<UnitRequirement> UnitRequirements { get; set; }

        public DbSet<UnitsInProgress> UnitsInProgress { get; set; }

        public DbSet<BuildingsInProgress> BuildingsInProgress { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}