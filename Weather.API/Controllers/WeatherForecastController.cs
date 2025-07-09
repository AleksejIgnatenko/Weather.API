using Microsoft.AspNetCore.Mvc;
using Weather.Application.Abstartions;
using Weather.Core.Entities;

namespace Weather.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IWeatherForecastService service) : ControllerBase
{
    private readonly IWeatherForecastService _service = service;

    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecastEntity>> Get()
    {
        return Ok(_service.GetAllForecasts());
    }

    [HttpPost]
    public IActionResult Add([FromBody] WeatherForecastEntity forecast)
    {
        _service.AddForecast(forecast);
        return Ok(new { Message = "Forecast added" });
    }
}