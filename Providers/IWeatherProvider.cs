using System.Collections.Generic;
using regents_new.Models;

namespace regents_new.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
