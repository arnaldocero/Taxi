using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDb(
    DataContext dataContext,
    IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            var admin = await CheckUserAsync("1010", "Arnaldo", "Pacheco", "arnaldopachecosarmiento@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Admin);
            var driver = await CheckUserAsync("2020", "Arnaldo", "Pacheco", "p.arnaldoalfonso@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Driver);
            var user1 = await CheckUserAsync("3030", "Arnaldo", "Pacheco", "arnaldopacheco1@outlook.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            var user2 = await CheckUserAsync("4040", "Arnaldo", "Pacheco", "arnaldopacheco1@outlook.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            await CheckTaxisAsync(driver, user1, user2);

        }

        private async Task<UserEntity> CheckUserAsync(
    string document,
    string firstName,
    string lastName,
    string email,
    string phone,
    string address,
    UserType userType)
        {
            UserEntity user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckTaxisAsync(
            UserEntity driver,
            UserEntity user1,
            UserEntity user2)
        {
            if (!_dataContext.Taxis.Any())
            {
                _dataContext.Taxis.Add(new TaxiEntity
                {
                    User = driver,
                    Plaque = "TPQ123",
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eget metus id orci malesuada viverra. Integer semper convallis accumsan. Mauris vulputate sagittis augue at porta. In iaculis elementum rhoncus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nunc non faucibus dui. Ut risus orci, imperdiet sit amet mauris quis, consectetur interdum orci. Nullam id sem eros. Duis ut elit odio. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Integer feugiat eros et interdum volutpat. Integer pretium iaculis arcu vitae vulputate. Sed et lectus faucibus, venenatis erat non, scelerisque ipsum.",
                            User = user1
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Nullam at venenatis felis. Nulla tincidunt mi justo, id hendrerit metus hendrerit eget. Suspendisse sodales nisi vitae nibh varius commodo. Etiam ullamcorper ullamcorper nisl in tincidunt. Etiam tellus erat, imperdiet a tellus ut, semper accumsan odio. Proin mollis rhoncus leo, ac tristique neque porttitor et. Nunc augue tellus, elementum luctus lorem eget, ornare scelerisque augue. Nunc tempor porttitor laoreet. Curabitur efficitur neque quis suscipit ornare. Nam sit amet enim et ex ullamcorper venenatis nec ac orci.",
                            User = user1
                        }
                    }
                });

                _dataContext.Taxis.Add(new TaxiEntity
                {
                    Plaque = "THW321",
                    User = driver,
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Muy buen servicio",
                            User = user2
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Conductor muy amable",
                            User = user2
                        }
                    }
                });

                await _dataContext.SaveChangesAsync();
            }
        }



        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Driver.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());

        }
    }
}
