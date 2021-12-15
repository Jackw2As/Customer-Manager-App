using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Services.DrinkRoleLoader;
using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManagerApp.Test.Backend_Tests
{
    public class DrinkTypeLoaderTests
    {
        public DrinkTypeLoaderTests()
        {
            IDrinkLoaderService loaderService = new MockDrinkRepository();
            DefaultDrinkList = loaderService.LoadDrinkTypes() as List<DrinkValueObject>;
            if (DefaultDrinkList == null)
            {
                throw new NullReferenceException
                    ($"{nameof(DefaultDrinkList)} returned null. Probably a Casting Error");
            }
                
        }
        private List<DrinkValueObject> DefaultDrinkList { get; init; }

        private IDrinkLoaderService ConstructLoaderService()
        {
            return new MockDrinkRepository();
        }

        [Fact(DisplayName = "Load Drinks Sync")]
        public void LoadDrinksSync()
        {
            var jobsLoader = ConstructLoaderService();
            var jobs = jobsLoader.LoadDrinkTypes();
            Assert.NotNull(jobs);
            Assert.NotEmpty(jobs);
            Assert.Equal(DefaultDrinkList, jobs);
        }
        [Fact(DisplayName = "Load Drinks Async")]
        public async void LoadJobsAsync()
        {
            var jobsLoader = ConstructLoaderService();
            var jobs = await jobsLoader.LoadDrinkTypesAsync();
            Assert.NotNull(jobs);
            Assert.NotEmpty(jobs);
            Assert.Equal(DefaultDrinkList, jobs);
        }

    }
}
