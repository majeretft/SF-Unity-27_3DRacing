using System;

namespace SF3DRacing
{
    public static class StringTime
    {

        public static string SecondsToString(float seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:ff");
        }
    }
}
