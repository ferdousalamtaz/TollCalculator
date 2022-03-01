
using Moq;
using System;
using TollCalculator.Business;
using TollCalculator.Interfaces;
using TollCalculator.Models;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace TollCalculator.Tests.Business
{
    public class TollControllerShould
    {
        private readonly TollController _controller;
        private readonly TollManager _manager;
        private readonly Mock<IDatabaseProvider> _databaseMock;
        private readonly Mock<ILogger<TollController>> _loggerMock;

        public TollControllerShould()
        {
            _loggerMock = new Mock<ILogger<TollController>>();
            _databaseMock = new Mock<IDatabaseProvider>();
            _manager = new TollManager(_databaseMock.Object);
            _controller = new TollController(_manager, _loggerMock.Object);

        }


        [Fact]
        public void CalculateToll_ForSingleDay_WithTollCellingAndExemptVehicleAndTollForTimeStampInfoFromDB_ThenVerifyDailyTollLimit()
        {
            //Arrange
            VehicleTypes vehicleType = VehicleTypes.Car;
            decimal expectedDummyTollforTimestamp = 75M;
            int expectedDummyTollCelling = 60;
            DateTime passingDateOne = new DateTime(2022, 02, 28, 13, 10, 10, 10);
            DateTime passingDateTwo = new DateTime(2022, 02, 28, 15, 10, 10, 10);


            DateTime[]? passingDates = new DateTime[]
                {
                        passingDateOne,
                        passingDateTwo
                };
            Query q = new() { vehicleType = vehicleType, passingDates = passingDates };

            _databaseMock.Setup(x => x.IsVehicleTollFreeFromDB(vehicleType)).Returns(false);
            _databaseMock.Setup(x => x.GetTollByTimeStampFromDB(It.IsAny<DateTime>())).Returns(expectedDummyTollforTimestamp);
            _databaseMock.Setup(x => x.GetDailyTollCellingFromDB()).Returns(expectedDummyTollCelling);


            //Act
            ActionResult<decimal>? actionResult = _controller.CalculateToll(q);
            ObjectResult? objectResult = actionResult.Result as ObjectResult;
            decimal? actual = (decimal)objectResult?.Value;

            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.AtLeast(2));
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.AtMostOnce());
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(vehicleType), Times.AtMostOnce());


            actionResult.Should().NotBeNull();
            objectResult.Value.Should().NotBeNull();
            actual.Should().BeLessThanOrEqualTo(expectedDummyTollCelling);
            actual.Should().BeGreaterThanOrEqualTo(0);

        }

        [Fact]
        public void CalculateToll_ForTollFreeVehicleFromDB_ThenVerifyIfTollIsZero()
        {
            //Arrange
            VehicleTypes vehicleType = VehicleTypes.Diplomat;
            decimal expectedDummyTollforTimestamp = 75M;
            int expectedDummyTollCelling = 60;
            DateTime passingDateOne = new DateTime(2022, 02, 28, 13, 10, 10, 10);
            DateTime passingDateTwo = new DateTime(2022, 02, 28, 15, 10, 10, 10);


            DateTime[]? passingDates = new DateTime[]
                {
                        passingDateOne,
                        passingDateTwo
                };
            Query q = new() { vehicleType = vehicleType, passingDates = passingDates };

            _databaseMock.Setup(x => x.IsVehicleTollFreeFromDB(vehicleType)).Returns(true);
            _databaseMock.Setup(x => x.GetTollByTimeStampFromDB(It.IsAny<DateTime>())).Returns(expectedDummyTollforTimestamp);
            _databaseMock.Setup(x => x.GetDailyTollCellingFromDB()).Returns(expectedDummyTollCelling);


            //Act
            ActionResult<decimal>? actionResult = _controller.CalculateToll(q);
            ObjectResult? objectResult = actionResult.Result as ObjectResult;
            decimal? actual = (decimal)objectResult?.Value;

            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.Never);
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(vehicleType), Times.AtMostOnce());


            actionResult.Should().NotBeNull();
            objectResult.Value.Should().NotBeNull();
            actual.Should().BeLessThanOrEqualTo(expectedDummyTollCelling);
            actual.Should().Be(0);
        }

        [Fact]
        public void CalculateToll_ForHolidayFromDB_ThenVerifyIfTollIsZero()
        {
            //Arrange
            VehicleTypes vehicleType = VehicleTypes.Car;
            decimal expectedDummyTollforTimestamp = 75M;
            int expectedDummyTollCelling = 60;
            DateTime passingDateOne = new DateTime(2022, 02, 27, 13, 10, 10, 10);
            DateTime passingDateTwo = new DateTime(2022, 02, 27, 15, 10, 10, 10);


            DateTime[]? passingDates = new DateTime[]
                {
                        passingDateOne,
                        passingDateTwo
                };
            Query q = new() { vehicleType = vehicleType, passingDates = passingDates };

            _databaseMock.Setup(x => x.IsVehicleTollFreeFromDB(vehicleType)).Returns(false);
            _databaseMock.Setup(x => x.GetTollByTimeStampFromDB(It.IsAny<DateTime>())).Returns(expectedDummyTollforTimestamp);
            _databaseMock.Setup(x => x.GetDailyTollCellingFromDB()).Returns(expectedDummyTollCelling);


            //Act
            ActionResult<decimal>? actionResult = _controller.CalculateToll(q);
            ObjectResult? objectResult = actionResult.Result as ObjectResult;
            decimal? actual = (decimal)objectResult?.Value;

            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);

            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.AtMostOnce);
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(vehicleType), Times.AtMostOnce());


            actionResult.Should().NotBeNull();
            objectResult.Value.Should().NotBeNull();
            actual.Should().BeLessThanOrEqualTo(expectedDummyTollCelling);
            actual.Should().Be(0);
        }

        
        [Fact]
        public void Return_BadRequestResponse_WithNullRequest()
        {
            //Arrange
           
            Query q = new() { };

            //Act
            var actionResult = _controller.CalculateToll(q);
            BadRequestObjectResult? objectResult = actionResult.Result as BadRequestObjectResult;
            var errorMessage = objectResult?.Value;



            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.Never);
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(It.IsAny<VehicleTypes>()), Times.Never());


            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("The provided query string is not valid. Please provide a valid vehicleType and dates not from the future.", errorMessage);
        }

        [Fact]
        public void Return_BadRequestResponse_WithDatesFromFuture()
        {
            //Arrange
            VehicleTypes vehicleType = VehicleTypes.Car;
            DateTime passingDateOne = new DateTime(2023, 02, 27, 13, 10, 10, 10);
            DateTime passingDateTwo = new DateTime(2023, 02, 27, 15, 10, 10, 10);


            DateTime[]? passingDates = new DateTime[]
                {
                        passingDateOne,
                        passingDateTwo
                };
            Query q = new() { vehicleType = vehicleType, passingDates = passingDates };

            //Act
            var actionResult = _controller.CalculateToll(q);
            BadRequestObjectResult? objectResult = actionResult.Result as BadRequestObjectResult;
            var errorMessage = objectResult?.Value;



            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.Never);
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(It.IsAny<VehicleTypes>()), Times.Never());



            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("The provided query string is not valid. Please provide a valid vehicleType and dates not from the future.", errorMessage);
        }

        [Fact]
        public void Return_BadRequestResponse_WithNoDates()
        {
            //Arrange
            VehicleTypes vehicleType = VehicleTypes.Car;
            DateTime passingDateOne = new DateTime(2023, 02, 10, 13, 10, 10, 10);
            DateTime passingDateTwo = new DateTime(2023, 02, 28, 15, 10, 10, 10);


            DateTime[]? passingDates = new DateTime[]
                {
                        
                };
            Query q = new() { vehicleType = vehicleType, passingDates = passingDates };

            //Act
            var actionResult = _controller.CalculateToll(q);
            BadRequestObjectResult? objectResult = actionResult.Result as BadRequestObjectResult;
            var errorMessage = objectResult?.Value;


            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.Never);
            _databaseMock.Verify(mock => mock.IsVehicleTollFreeFromDB(It.IsAny<VehicleTypes>()), Times.Never());


            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("The provided query string is not valid. Please provide a valid vehicleType and dates not from the future.", errorMessage);
        }
    }
}