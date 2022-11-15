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
    public class TollControllerTest
    {
        private readonly TollController _controller;
        private readonly Mock<TollManager> _tollManagerMock;
        private readonly Mock<IDataAccessor> _databaseMock;
        private readonly Mock<ILogger<TollController>> _loggerMock;

        public TollControllerTest()
        {
            _loggerMock = new Mock<ILogger<TollController>>();
            _databaseMock = new Mock<IDataAccessor>();
            _tollManagerMock = new Mock<TollManager>(_databaseMock.Object);
            _controller = new TollController(_tollManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Return_Exception_WithNullRequest()
        {
            //Arrange
            // Since we are not going to bind the model the model validation will not occour and get exception.
            //Act
            try
            {
                var actionResult = _controller.CalculateToll(null);
            }
            catch (Exception ex)
            {

            }
            //Assert
            _databaseMock.Verify(mock => mock.GetTollByTimeStampFromDB(It.IsAny<DateTime>()), Times.Never);
            _databaseMock.Verify(mock => mock.GetDailyTollCellingFromDB(), Times.Never);
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

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }



    }
}