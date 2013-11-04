namespace FirebirdWars.Migrations
{
    using FirebirdWars.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<FirebirdWars.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FirebirdWars.Models.ApplicationDbContext context)
        {
            if (context.Buildings.Count() > 0)
            {
                return;
            }

            #region Buildings

            var castle = new Building
            {
                Id = 1,
                Type = "Castle",
                WoodCost = 80,
                FoodCost = 70,
                IronCost = 75,
                BuildTime = 139,
                PopulationCost = 2,
                Coords = "362, 286, 361, 290, 475, 241, 482, 224, 474, 188, 478, 160, 445, 141, 416, 157, 414, 88, 390, 77, 359, 88, 363, 49, 352, 35, 353, 10, 337, 0, 318, 10, 318, 34, 310, 48, 300, 133, 272, 118, 258, 99, 246, 115, 241, 144, 245, 244, 361, 291"
            };

            var barracks = new Building
            {
                Id = 2,
                Type = "Barracks",
                WoodCost = 200,
                FoodCost = 170,
                IronCost = 90,
                BuildTime = 114,
                PopulationCost = 5,
                Coords = "576, 228, 590, 234, 629, 213, 648, 174, 607, 156, 612, 150, 593, 127, 561, 142, 548, 160, 550, 173, 524, 191, 527, 202, 533, 205, 566, 191, 584, 198, 575, 207"
            };

            var stable = new Building
            {
                Id = 3,
                Type = "Stable",
                WoodCost = 300,
                FoodCost = 245,
                IronCost = 255,
                BuildTime = 286,
                PopulationCost = 5,
                Coords = "347, 381, 384, 405, 412, 405, 462, 401, 476, 390, 462, 378, 464, 343, 448, 319, 448, 305, 432, 284, 401, 290, 346, 318"
            };

            var lumberjacks = new Building
            {
                Id = 4,
                Type = "Lumberjacks",
                WoodCost = 50,
                FoodCost = 60,
                IronCost = 40,
                BuildTime = 82,
                PopulationCost = 2,
                Coords = "254, 328, 236, 331, 238, 340, 244, 352, 262, 352, 273, 345, 281, 349, 269, 375, 278, 380, 287, 378, 295, 358, 315, 348, 315, 321, 297, 294, 278, 301"
            };

            var farm = new Building
            {
                Id = 5,
                Type = "Farm",
                WoodCost = 45,
                FoodCost = 40,
                IronCost = 30,
                BuildTime = 84,
                PopulationCost = 2,
                Coords = "112, 147, 97, 175, 70, 192, 73, 217, 113, 238, 160, 221, 161, 178, 148, 165"
            };

            var ironMine = new Building
            {
                Id = 6,
                Type = "Iron Mine",
                WoodCost = 50,
                FoodCost = 45,
                IronCost = 50,
                BuildTime = 83,
                PopulationCost = 2,
                Coords = "184, 383, 157, 403, 155, 415, 176, 423, 198, 410, 212, 417, 201, 430, 213, 435, 253, 411, 250, 385, 237, 358, 199, 387"
            };

            var house = new Building
            {
                Id = 7,
                Type = "House",
                WoodCost = 40,
                FoodCost = 30,
                IronCost = 35,
                BuildTime = 40,
                PopulationCost = 3,
                Coords = "93, 264, 94, 288, 62, 298, 52, 310, 85, 327, 101, 318, 94, 312, 99, 305, 132, 324, 156, 315, 179, 328, 182, 340, 185, 328, 217, 309, 204, 299, 172, 288, 171, 266, 126, 245"
            };

            var workshop = new Building
            {
                Id = 8,
                Type = "Workshop",
                WoodCost = 180,
                FoodCost = 130,
                IronCost = 240,
                BuildTime = 320,
                PopulationCost = 3,
                Coords = "476, 274, 465, 289, 471, 303, 487, 313, 506, 307, 513, 308, 547, 322, 565, 314, 584, 323, 603, 312, 617, 287, 608, 273, 597, 253, 567, 258, 572, 245, 560, 226, 484, 261"
            };

            #endregion

            context.Buildings.AddOrUpdate(b => b.Type, castle, barracks, stable, lumberjacks, farm, ironMine, house, workshop);

            #region Units

            var pikeman = new Unit
            {
                Type = "Pikeman",
                WoodCost = 50,
                FoodCost = 15,
                IronCost = 30,
                CarryingCapacity = 25,
                InfantryPower = 20,
                CavalryPower = 70,
                SiegeArtilleryPower = 20,
                PopulationCost = 1,
                HealthPoints = 60,
                BuildTime = 720,
                Speed = 10
            };

            var swordsman = new Unit
            {
                Type = "Swordsman",
                WoodCost = 30,
                FoodCost = 30,
                IronCost = 70,
                CarryingCapacity = 15,
                InfantryPower = 35,
                CavalryPower = 35,
                SiegeArtilleryPower = 35,
                PopulationCost = 1,
                HealthPoints = 100,
                BuildTime = 960,
                Speed = 15
            };

            var lightCavalry = new Unit
            {
                Type = "Light Cavalry",
                WoodCost = 120,
                FoodCost = 150,
                IronCost = 250,
                CarryingCapacity = 90,
                InfantryPower = 150,
                CavalryPower = 100,
                SiegeArtilleryPower = 150,
                PopulationCost = 4,
                HealthPoints = 300,
                BuildTime = 1500,
                Speed = 6
            };

            var heavyCavalry = new Unit
            {
                Type = "Heavy Cavalry",
                WoodCost = 200,
                FoodCost = 240,
                IronCost = 550,
                CarryingCapacity = 55,
                InfantryPower = 250,
                CavalryPower = 150,
                SiegeArtilleryPower = 250,
                PopulationCost = 6,
                HealthPoints = 500,
                BuildTime = 1980,
                Speed = 7
            };

            var catapult = new Unit()
            {
                Type = "Catapult",
                WoodCost = 850,
                FoodCost = 250,
                IronCost = 700,
                CarryingCapacity = 15,
                InfantryPower = 180,
                CavalryPower = 150,
                SiegeArtilleryPower = 150,
                PopulationCost = 8,
                HealthPoints = 400,
                BuildTime = 3800,
                Speed = 30
            };

            var balista = new Unit()
            {
                Type = "Balista",
                WoodCost = 700,
                FoodCost = 300,
                IronCost = 500,
                CarryingCapacity = 25,
                InfantryPower = 200,
                CavalryPower = 300,
                SiegeArtilleryPower = 100,
                PopulationCost = 10,
                HealthPoints = 300,
                BuildTime = 3000,
                Speed = 32
            };

            var mechanicFirebird = new Unit()
            {
                Type = "Mechanic Firebird",
                WoodCost = 3000,
                FoodCost = 3000,
                IronCost = 3000,
                CarryingCapacity = 500,
                InfantryPower = 7000,
                CavalryPower = 7000,
                SiegeArtilleryPower = 7000,
                PopulationCost = 3000,
                HealthPoints = 5000,
                BuildTime = 11000,
                Speed = 1
            };

            #endregion

            context.Units.AddOrUpdate(u => u.Type, pikeman, swordsman, lightCavalry, heavyCavalry, catapult, balista, mechanicFirebird);

            #region Roles

            var administrator = new Role
            {
                Name = "Administrator"
            };

            var user = new Role
            {
                Name = "User"
            };

            #endregion

            context.Roles.AddOrUpdate(r => r.Name, administrator, user);

            #region BuildingRequirements

            barracks.BuildingRequirements.Add(new BuildingRequirement
            {
                Building = barracks,
                RequiredBuilding = castle,
                RequiredBuildingLevel = 3
            });

            stable.BuildingRequirements.Add(new BuildingRequirement
            {
                Building = stable,
                RequiredBuilding = castle,
                RequiredBuildingLevel = 10
            });

            stable.BuildingRequirements.Add(new BuildingRequirement
            {
                Building = stable,
                RequiredBuilding = barracks,
                RequiredBuildingLevel = 5
            });

            workshop.BuildingRequirements.Add(new BuildingRequirement
            {
                Building = workshop,
                RequiredBuilding = castle,
                RequiredBuildingLevel = 10
            });

            #endregion

            #region UnitRequirements

            pikeman.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = barracks,
                RequiredBuildingLevel = 1
            });

            swordsman.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = barracks,
                RequiredBuildingLevel = 2
            });

            lightCavalry.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = stable,
                RequiredBuildingLevel = 3
            });

            heavyCavalry.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = stable,
                RequiredBuildingLevel = 10
            });

            catapult.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = workshop,
                RequiredBuildingLevel = 1
            });

            balista.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = workshop,
                RequiredBuildingLevel = 2
            });

            mechanicFirebird.UnitRequirements.Add(new UnitRequirement
            {
                RequiredBuilding = workshop,
                RequiredBuildingLevel = 10
            });

            #endregion
        }
    }
}