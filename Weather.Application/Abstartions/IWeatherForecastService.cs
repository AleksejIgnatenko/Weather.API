using Weather.Core.Entities;

namespace Weather.Application.Abstartions;

public interface IWeatherForecastService
{
    void AddForecast(WeatherForecastEntity forecast);
    WeatherForecastEntity GenerateRandomForecast(DateOnly date);
    IEnumerable<WeatherForecastEntity> GetAllForecasts();
    double GetAverageTemperature(DateOnly from, DateOnly to);
    WeatherForecastEntity GetHottestDay();
    bool RemoveForecastByDate(DateOnly date);
}