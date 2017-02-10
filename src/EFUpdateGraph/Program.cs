using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFUpdateGraph
{
    public class Program
    {
        private static ILogger Logger;
        public static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            Logger = loggerFactory.CreateLogger<Program>();
            Logger.LogInformation("logging configured.");

            try
            {
                Logger.LogInformation("setting up db context options.");

                var options = new DbContextOptionsBuilder<InMemoryDbContext>()
                    .UseInMemoryDatabase()
                    .Options;

                // seed data
                using (var db = new InMemoryDbContext(options))
                {
                    SeedTestData(db);
                }

                // list floors
                using (var db = new InMemoryDbContext(options))
                {
                    ListFloors(db);
                }

                var floorDtos = new List<FloorDto>
                {
                    new FloorDto
                    {
                        Id = 1,
                        Name = "Floor one updated"
                    },
                    new FloorDto
                    {
                        Id = 2,
                        Name = "Floor two updated",
                        RoomDtos = new List<RoomDto>
                        {
                            new RoomDto { Name = "New Room 1"}
                        }
                    }
                };

                using (var db = new InMemoryDbContext(options))
                {
                    UpdateFloor(floorDtos.First(), db);
                }

                using (var db = new InMemoryDbContext(options))
                {
                    UpdateFloor(floorDtos[1], db);
                }

                // list floors
                using (var db = new InMemoryDbContext(options))
                {
                    ListFloors(db);
                }

                Logger.LogInformation("complete.");
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                Console.WriteLine("Finished.");
                Console.ReadLine();
            }
        }

        private static void UpdateFloor(FloorDto dto, InMemoryDbContext db)
        {
            var floor = db.Floors
                .Include(p => p.Rooms)
                .SingleOrDefault(p => p.Id == dto.Id);

            if (floor == null)
            {
                Logger.LogWarning($"Floor with id {dto.Id} could not be found.");
            }
            else
            {
                floor.Name = dto.Name;
                foreach (var roomDto in dto.RoomDtos)
                {
                    if (roomDto.Id.Equals(default(int)))
                    {

                    }
                    else
                    {
                        var existingRoom = floor.Rooms.SingleOrDefault(p => p.Id == roomDto.Id);
                        existingRoom.Name = roomDto.Name;
                    }
                }
                db.SaveChanges();
            }
        }

        private static void ListFloors(InMemoryDbContext context)
        {
            var floors = context.Floors.Include(n => n.Rooms).ToList();
            foreach (var floor in floors)
            {
                Logger.LogInformation($"Id: {floor.Id}, Name: {floor.Name}, No. rooms: {floor.Rooms.Count}");
                foreach (var room in floor.Rooms)
                {
                    Logger.LogInformation($"Room - Id: {room.Id}, Name: {room.Name}");
                }
            }

            Logger.LogInformation($"floors: {floors.Count}");
        }

        private static void SeedTestData(InMemoryDbContext context)
        {
            Logger.LogInformation("Seeding test data.");

            Logger.LogInformation("Creating floor one.");
            var floorOne = new Floor { Name = "Floor one" };

            Logger.LogInformation("Creating floor two.");
            var roomOne = new Room { Name = "Room one" };

            Logger.LogInformation("Adding room one to floor one.");
            floorOne.Rooms.Add(roomOne);

            var floorTwo = new Floor { Name = "Floor two" };
            Logger.LogInformation("Adding floor one to the context.");
            context.Add(floorOne);
            Logger.LogInformation("Adding floor two to the context.");
            context.Add(floorTwo);

            Logger.LogInformation("Saving changes.");
            context.SaveChanges();
        }
    }
}