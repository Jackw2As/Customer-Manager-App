using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerManagerApp.Test.Backend_Tests
{
    public class DrinkRepositoryTests
    {
        public DrinkRepositoryTests()
        {
            IDrinkRepository loaderService = new MockDrinkRepository();
            DefaultDrinkList = loaderService.ReadAll().Result ?? throw new NullReferenceException($"LoadDrinkTypes() method wasn't properly created on {loaderService.GetType()} Class.");
            if (DefaultDrinkList == null)
            {
                throw new NullReferenceException
                    ($"{nameof(DefaultDrinkList)} returned null. Probably a Casting Error");
            }
                
        }
        private List<DrinkEntity> DefaultDrinkList { get; init; }

        private IDrinkRepository constructMockRepository()
        {
            return new MockDrinkRepository();
        }

        [Fact(DisplayName = "Load Drinks Sync")]
        public async void LoadDrinksSync()
        {
            var drinksRepo = constructMockRepository();
            var drinks = await drinksRepo.ReadAll();
            Assert.NotNull(drinks);
            Assert.NotEmpty(drinks);
            Assert.Equal(DefaultDrinkList, drinks);
        }
        [Fact(DisplayName = "Load Drinks Async")]
        public async void LoadDrinksAsync()
        {
            var drinksRepo = constructMockRepository();
            var drinks = await drinksRepo.ReadAll();
            Assert.NotNull(drinks);
            Assert.NotEmpty(drinks);
            Assert.Equal(DefaultDrinkList, drinks);
        }

    }
}
