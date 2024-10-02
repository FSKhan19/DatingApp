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

            using HMACSHA512 hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123"));

            var appUserFaker = new AutoFaker<AppUser>()
                .RuleFor(up => up.Id, default(int))
                .RuleFor(up => up.Gender, f => f.PickRandom(new[] { Bogus.DataSets.Name.Gender.Male.ToString().ToLower(), Bogus.DataSets.Name.Gender.Female.ToString().ToLower() }))
                .RuleFor(up => up.UserName, (f, u) =>
                {
                    var gender = u.Gender.ToLower() == Bogus.DataSets.Name.Gender.Male.ToString().ToLower() ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;
                    return f.Name.FirstName(gender).ToLower();
                })
                .RuleFor(up => up.KnownAs, (f, u) => u.UserName) // Use UserName value for KnownAs
                .RuleFor(up => up.PasswordHash, hash)
                .RuleFor(up => up.PasswordSalt, salt)
                .RuleFor(up => up.CreatorUserId, default(int))
                .RuleFor(up => up.CreationTime, DateTime.UtcNow)
                .RuleFor(up => up.LastModificationTime, default(DateTime))
                .RuleFor(up => up.LastModifierUserId, default(int))
                .RuleFor(up => up.IsDeleted, false)
                .RuleFor(up => up.DeleterUserId, default(int))
                .RuleFor(up => up.DeletionTime, default(DateTime))
                .RuleFor(up => up.LastActive, default(DateTime))
                .RuleFor(up => up.Introduction, f => f.Lorem.Text())
                .RuleFor(up => up.LookingFor, f => f.Lorem.Text())
                .RuleFor(up => up.LookingFor, f => f.Lorem.Text())
                .RuleFor(up => up.City, f => f.Address.City())  
                .RuleFor(up => up.Country, f => f.Address.Country())
                .RuleFor(up => up.Photos, (f, u) =>
                {
                    // Generate a random number between 1 and 100
                    var randomNumber = f.Random.Int(1, 100);
                    var baseUrl = u.Gender == Bogus.DataSets.Name.Gender.Male.ToString().ToLower()
                        ? $"https://randomuser.me/api/portraits/men/{randomNumber}.jpg"
                        : $"https://randomuser.me/api/portraits/women/{randomNumber}.jpg";

                    return new List<Photo>
                    {
                        new Photo
                        {
                            Url = baseUrl,
                            IsMain = true
                        }
                    };
                });


            var appUsers = appUserFaker.Generate(5); // Generates a list of user profiles

            await context.Users.AddRangeAsync(appUsers);  
            await context.SaveChangesAsync();   

        }
    }
}
