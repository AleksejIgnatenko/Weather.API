using Weather.Application.Services;
using Weather.Core.Entities;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;

namespace Weather.TestSuiteNUnit.Services
{
    [TestFixture]
    public class WeatherForecastEntityServiceTests
    {
        private WeatherForecastEntityService _service;
        private IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _service = new WeatherForecastEntityService();
        }

        [Test]
        public void GetAllForecasts_ReturnsEmptyCollection_WhenNoForecastsAdded()
        {
            // Act
            var result = _service.GetAllForecasts();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void AddForecast_ShouldAddForecastToList()
        {
            // Arrange
            var date = new DateOnly(2024, 1, 1);
            var forecast = _fixture.Build<WeatherForecastEntity>()
                                   .With(f => f.Date, date)
                                   .Create();

            // Act
            _service.AddForecast(forecast);
            var result = _service.GetAllForecasts();

            // Assert
            result.Should().ContainEquivalentOf(forecast);
        }

        [Test]
        public void AddForecast_ThrowsArgumentNullException_WhenNullGiven()
        {
            // Act & Assert
            _service.Invoking(s => s.AddForecast(null))
                    .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GenerateRandomForecast_ReturnsValidForecast()
        {
            // Arrange
            var date = new DateOnly(2024, 1, 1);

            // Act
            var result = _service.GenerateRandomForecast(date);

            // Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(date);
            result.TemperatureC.Should().BeGreaterThanOrEqualTo(-20).And.BeLessThanOrEqualTo(54);
            result.Summary.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void RemoveForecastByDate_RemovesExistingForecast()
        {
            // Arrange
            var date = new DateOnly(2024, 1, 1);
            var forecast = new WeatherForecastEntity
            {
                Date = date,
                TemperatureC = 20,
                Summary = "Sunny"
            };
            _service.AddForecast(forecast);

            // Act
            bool removed = _service.RemoveForecastByDate(date);

            // Assert
            removed.Should().BeTrue();
            _service.GetAllForecasts().Should().BeEmpty();
        }

        [Test]
        public void RemoveForecastByDate_ReturnsFalse_WhenDateNotFound()
        {
            // Arrange
            var date = new DateOnly(2024, 1, 1);

            // Act
            bool removed = _service.RemoveForecastByDate(date);

            // Assert
            removed.Should().BeFalse();
        }

        [Test]
        public void GetAverageTemperature_ReturnsCorrectValue()
        {
            // Arrange
            var date1 = new DateOnly(2024, 1, 1);
            var date2 = new DateOnly(2024, 1, 2);
            _service.AddForecast(new WeatherForecastEntity { Date = date1, TemperatureC = 20 });
            _service.AddForecast(new WeatherForecastEntity { Date = date2, TemperatureC = 30 });

            // Act
            double avg = _service.GetAverageTemperature(date1, date2);

            // Assert
            avg.Should().Be(25);
        }

        [Test]
        public void GetAverageTemperature_ReturnsNaN_WhenNoDataInRange()
        {
            // Arrange
            var date1 = new DateOnly(2024, 1, 1);
            var date2 = new DateOnly(2024, 1, 2);

            // Act
            double avg = _service.GetAverageTemperature(date1, date2);

            // Assert
            double.IsNaN(avg).Should().BeTrue();
        }

        [Test]
        public void GetHottestDay_ReturnsNull_WhenNoForecasts()
        {
            // Act
            var hottest = _service.GetHottestDay();

            // Assert
            hottest.Should().BeNull();
        }

        [Test]
        public void GetHottestDay_ReturnsCorrectForecast()
        {
            // Arrange
            var forecast1 = new WeatherForecastEntity { Date = new DateOnly(2024, 1, 1), TemperatureC = 20 };
            var forecast2 = new WeatherForecastEntity { Date = new DateOnly(2024, 1, 2), TemperatureC = 30 };
            var forecast3 = new WeatherForecastEntity { Date = new DateOnly(2024, 1, 3), TemperatureC = 25 };

            _service.AddForecast(forecast1);
            _service.AddForecast(forecast2);
            _service.AddForecast(forecast3);

            // Act
            var hottest = _service.GetHottestDay();

            // Assert
            hottest.Should().NotBeNull();
            hottest.TemperatureC.Should().Be(30);
        }
    }
}