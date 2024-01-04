using System.Collections.Generic;

namespace MMIM.Enum
{
    public static partial class Settings
    {
        public static readonly IEnumerable<string> TimerSettings = new[]
        {
            "No Restriction",
            "Cannot Change",
            "Blitz: 10s - 50s",
            "Rush: 50s - 100s",
            "Fast: 100s - 200s",
            "Normal: 200s - 500s"
        };
    }
}
