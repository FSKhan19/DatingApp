using AutoBogus;
using Bogus.DataSets;
using DatingApp.Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Backend.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DatingAppContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var appUserFaker = new AutoFaker<AppUser>()
                .RuleFor(up => up.Id, f => 0) // EF Core will auto-generate this
                .RuleFor(up => up.Gender, f => f.PickRandom(new[] { Bogus.DataSets.Name.Gender.Male.ToString().ToLower(), Bogus.DataSets.Name.Gender.Female.ToString().ToLower() }))
                .RuleFor(up => up.UserName, (f, u) =>
                {
                    var gender = u.Gender?.ToLower() == Bogus.DataSets.Name.Gender.Male.ToString().ToLower() ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;
                    return f.Name.FirstName(gender).ToLower();
                })
                .RuleFor(up => up.KnownAs, (f, u) => u.UserName)
                .RuleFor(up => up.PasswordSalt, f =>
                {
                    using HMACSHA512 hmac = new HMACSHA512();
                    return hmac.Key;
                })
                .RuleFor(up => up.PasswordHash, (f, u) =>
                {
                    using HMACSHA512 hmac = new HMACSHA512(u.PasswordSalt);
                    return hmac.ComputeHash(Encoding.UTF8.GetBytes("P@ssword@123"));
                })
                .RuleFor(up => up.CreatorUserId, f => null)
                .RuleFor(up => up.CreationTime, DateTime.UtcNow)
                .RuleFor(up => up.LastModificationTime, f => null)
                .RuleFor(up => up.LastModifierUserId, f => null)
                .RuleFor(up => up.IsDeleted, false)
                .RuleFor(up => up.DeleterUserId, f => null)
                .RuleFor(up => up.DeletionTime, f => null)
                .RuleFor(up => up.LastActive, f => default(DateTime))
                .RuleFor(up => up.Introduction, f => f.Lorem.Text())
                .RuleFor(up => up.LookingFor, f => f.Lorem.Text())
                .RuleFor(up => up.City, f => f.Address.City())
                .RuleFor(up => up.Country, f => f.Address.Country())
                .RuleFor(up => up.Photos, (f, u) =>
                {
                    var photos = new List<Photo>();
                    var photoCount = f.Random.Int(1, 5);
                    for (int i = 0; i < photoCount; i++)
                    {
                        var randomNumber = f.Random.Int(1, 100);
                        var baseUrl = u.Gender == Bogus.DataSets.Name.Gender.Male.ToString().ToLower()
                            ? $"https://randomuser.me/api/portraits/men/{randomNumber}.jpg"
                            : $"https://randomuser.me/api/portraits/women/{randomNumber}.jpg";

                        photos.Add(new Photo
                        {
                            Url = baseUrl,
                            IsMain = i == 0,
                            IsDeleted = false,
                            CreationTime = DateTime.UtcNow,
                            CreatorUserId = null,
                            DeleterUserId = null,
                            DeletionTime = null,
                            LastModificationTime = null,
                            LastModifierUserId = null,
                            PublicId = null,
                            AppUserId = 0, // Temporarily set to 0
                            Id = 0
                        });
                    }
                    return photos;
                });

            var appUsers = appUserFaker.Generate(5);

            // Add users to the database
            await context.Users.AddRangeAsync(appUsers);
            await context.SaveChangesAsync();

            // Assign AppUserId to each photo
            foreach (var user in appUsers)
            {
                foreach (var photo in user.Photos)
                {
                    photo.AppUserId = user.Id; // Assign the correct AppUserId
                }
            }

            // Save changes again to persist the updated Photos
            await context.SaveChangesAsync();
        }
    }
}
