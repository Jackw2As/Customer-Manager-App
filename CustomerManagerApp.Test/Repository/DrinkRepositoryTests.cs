using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.ValueObjects;
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
            DefaultDrinkList = loaderService.LoadDrinkTypes() as List<DrinkValueObject>;
            if (DefaultDrinkList == null)
            {
                throw new NullReferenceException
                    ($"{nameof(DefaultDrinkList)} returned null. Probably a Casting Error");
            }
                
        }
        private List<DrinkValueObject> DefaultDrinkList { get; init; }

        private IDrinkRepository constructMockRepository()
        {
            return new MockDrinkRepository();
        }

        [Fact(DisplayName = "Load Drinks Sync")]
        public void LoadDrinksSync()
        {
            var drinksRepo = constructMockRepository();
            var drinks = drinksRepo.LoadDrinkTypes();
            Assert.NotNull(drinks);
            Assert.NotEmpty(drinks);
            Assert.Equal(DefaultDrinkList, drinks);
        }
        [Fact(DisplayName = "Load Drinks Async")]
        public async void LoadDrinksAsync()
        {
            var drinksRepo = constructMockRepository();
            var drinks = await drinksRepo.LoadDrinkTypesAsync();
            Assert.NotNull(drinks);
            Assert.NotEmpty(drinks);
            Assert.Equal(DefaultDrinkList, drinks);
        }

    }
}
