using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private static string NoTimeError = "Empty time";
        private static string IncorrectFormatError = "The time format is incorrect. Expected: HH:mm.ss (23:00:51)";
        private static string HoursError = "(Hours) parameter is not a number";
        private static string MinutesError = "(Minutes) parameter is not a number";
        private static string SecondsError = "(Seconds) parameter is not a number";
        private static string HoursValueError = "Hours must be between 0-24";
        private static string MinutesValueError = "Minutes must be between 0-59";
        private static string SecondsValueError = "Seconds must be between 0-59";

        /// <summary>
        /// Convert string time format HH:mm:ss into Berlin clock time.
        /// </summary>
        /// <param name="aTime">Time to convert. Format HH:mm:ss</param>
        /// <returns>Berlin clock time</returns>
        public string convertTime(string aTime)
        {
            // Empty parameter.
            if (string.IsNullOrWhiteSpace(aTime))
                throw new ArgumentException(NoTimeError);
           
            string[] split = aTime.Split(':');

            // Format incorrect. Expected: HH:mm:ss.
            if (split.Length != 3)
                throw new ArgumentException(IncorrectFormatError);

            int hours, minutes, seconds;

            // Try converting parameters into hours/minutes/seconds.
            if (!int.TryParse(split[0], out hours))
                throw new ArgumentException(HoursError);
            if(!int.TryParse(split[1], out minutes))
                throw new ArgumentException(MinutesError);
            if (!int.TryParse(split[2], out seconds))
                throw new ArgumentException(SecondsError);

            // Validate values are in range. Hours: (0-24) Minutes: (0-59) Seconds: (0-59).
            if (hours < 0 || hours > 24)
                throw new ArgumentException(HoursValueError);
            if (minutes < 0 || minutes > 59)
                throw new ArgumentException(MinutesValueError);
            if (seconds < 0 || seconds > 59)
                throw new ArgumentException(SecondsValueError);

            return ConvertTimeToBerlinTime(hours, minutes, seconds);
        }

        /// <summary>
        /// Convert a time into berlin time.
        /// </summary>
        /// <param name="hours">Hours</param>
        /// <param name="minutes">Minutes</param>
        /// <param name="seconds">Seconds</param>
        /// <returns>string contain values for Berlin clock</returns>
        public string ConvertTimeToBerlinTime(int hours, int minutes, int seconds)
        {
            // First line: blink if seconds is an odd number.
            var berlinTime = (seconds % 2 != 0) ? "O" : "Y";
            int n = hours / 5;

            // Second line: 5 hours lamps.
            berlinTime +=  Environment.NewLine + new String('R', n).PadRight(4, 'O');
           
            // Third line: 1 hour lamps.
            n = hours - (n * 5);
            berlinTime += Environment.NewLine + new String('R', n).PadRight(4, 'O');
           
            // Fourth line: 5 minutes lamps.
            n = minutes / 5;
            berlinTime += Environment.NewLine + new String('Y', n).PadRight(11, 'O').Replace("YYY", "YYR");        
            
            // Fifth line: 1 minute lamp.
            n = minutes - (n * 5);
            berlinTime += Environment.NewLine + new String('Y', n).PadRight(4, 'O');

            return berlinTime;
        }
    }
}
