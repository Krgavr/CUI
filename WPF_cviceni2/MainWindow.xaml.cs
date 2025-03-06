using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using WPF_cviceni2.Models;
using WPF_cviceni2.Services;

namespace WPF_cviceni2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private readonly WeatherService weatherService;
    private WeatherModel weather;
    public MainWindow()
    {
        InitializeComponent();

        this.weatherService = new WeatherService();

    }

    private void UpdateSunPosition(long sunriseUnix, long sunsetUnix, long currentUnix)
    {
        bool isDaytime = currentUnix > sunriseUnix && currentUnix < sunsetUnix;

        if (!isDaytime)
        {
            DayLine.Stroke = new SolidColorBrush(Color.FromRgb(170, 170, 200));
            Sun.Visibility = Visibility.Collapsed;
        }
    }

    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        string city = CityName.Text;

        try
        {
            weather = await weatherService.GetWeatherInfo(city);

            CityNameText.Text = weather.name;
            TemperatureText.Text = $"Temp: {weather.main.temp}";
            FeelsLikeText.Text = $"Feels like: {weather.main.feelsLike}";
            HumidityText.Text = $"Humidity: {weather.main.humidity}";

            string currentTime = weatherService.ConvertTime(weather.datetime, weather.timezoneOffset);
            CurrentTimeText.Text = $"Current time in {weather.name}: {currentTime}";

            UpdateSunPosition(weather.sys.sunrise, weather.sys.sunset, weather.datetime);

            SunCanvas.Visibility = Visibility.Visible;

            string sunriseTime = weatherService.ConvertTime(weather.sys.sunrise, weather.timezoneOffset);
            SunriseText.Text = sunriseTime;
        }
        catch(Exception ex)
        {
            Trace.WriteLine("Error occured!");
        }
    }

    public void Save_Click(object sender, RoutedEventArgs e)
    {
        if (weather == null)
        {
            Trace.WriteLine("Error: weather object is null!");
            return;
        }


        try
        {
            string json = JsonSerializer.Serialize(weather, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("weatherData.json", json);
            Trace.WriteLine("Weather data saved successfully!");
        }
        catch (Exception ex )
        {
            Trace.WriteLine("Could not be saved.");
        }
    }
}