using Weather.Application.Abstartions;
using Weather.Core.Entities;

namespace Weather.Application.Services;

/// <summary>
/// Сервис для работы с прогнозами погоды.
/// </summary>
public class WeatherForecastEntityService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly List<WeatherForecastEntity> _forecasts = [];

    /// <summary>
    /// Возвращает все доступные прогнозы.
    /// </summary>
    public IEnumerable<WeatherForecastEntity> GetAllForecasts()
    {
        return _forecasts;
    }

    /// <summary>
    /// Генерирует случайный прогноз на указанную дату.
    /// </summary>
    public WeatherForecastEntity GenerateRandomForecast(DateOnly date)
    {
        var random = new Random();
        var forecast = new WeatherForecastEntity
        {
            Date = date,
            TemperatureC = random.Next(-20, 55),
            Summary = Summaries[random.Next(Summaries.Length)],
        };
        return forecast;
    }

    /// <summary>
    /// Добавляет новый прогноз в список.
    /// </summary>
    public void AddForecast(WeatherForecastEntity forecast)
    {
        ArgumentNullException.ThrowIfNull(forecast);

        _forecasts.Add(forecast);
    }

    /// <summary>
    /// Удаляет прогноз по дате.
    /// </summary>
    public bool RemoveForecastByDate(DateOnly date)
    {
        var forecast = _forecasts.FirstOrDefault(f => f.Date == date);
        if (forecast == null) return false;
        return _forecasts.Remove(forecast);
    }

    /// <summary>
    /// Возвращает среднюю температуру за указанный период.
    /// </summary>
    public double GetAverageTemperature(DateOnly from, DateOnly to)
    {
        var periodForecasts = _forecasts
            .Where(f => f.Date >= from && f.Date <= to)
            .ToList();

        if (!periodForecasts.Any())
            return double.NaN;

        return periodForecasts.Average(f => f.TemperatureC);
    }

    /// <summary>
    /// Возвращает самый жаркий день в прогнозах.
    /// </summary>
    public WeatherForecastEntity GetHottestDay()
    {
        return _forecasts
            .OrderByDescending(f => f.TemperatureC)
            .FirstOrDefault();
    }
}