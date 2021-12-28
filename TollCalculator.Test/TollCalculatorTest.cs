using System.IO;
using System.Reflection;
using Xunit;
using Newtonsoft.Json;
using TollCalculator.Models;
using Microsoft.AspNetCore.Mvc;

namespace TollCalculator.Test
{
    public class TollCalculatorTest
    {
        private readonly TollController _tollController;
        private readonly IExtension _extension;

        public TollCalculatorTest()
        {
            _extension = new Extensions();
           _tollController = new TollController(_extension);
        }

        [Fact]

        public void Post_WhenCalled_ReturnTotalToll()
        {
            Query query;
            // deserialize JSON directly from a file
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Mock.json");
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                query = (Query)serializer.Deserialize(file, typeof(Query));
            }

            //Act
            var okResult = _tollController.CalculateToll(query);

            //Assert
            Assert.IsType<decimal> (okResult.Value);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var query = new Query()
            {
                vehicleType = VehicleTypes.Car,
                passingDates = null
            };
            
            // Act
            var badResponse = _tollController.CalculateToll(query);
            // Assert
            Assert.Equal(400, (badResponse.Result as BadRequestResult)?.StatusCode);
        }
    }
}