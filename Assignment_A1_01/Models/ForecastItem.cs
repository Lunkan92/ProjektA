using System;

namespace Assignment_A1_01.Models
{
    public class ForecastItem
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public override string ToString()
        {
            return ($"Datum:{DateTime} - {DateTime.TimeOfDay}\n Temp:{Temperature} degC, " +
                $"{Icon} {Description} Vind:{WindSpeed} m/s");
        }
    }
}
