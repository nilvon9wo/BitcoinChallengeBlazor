using System;
using System.Threading;

namespace BitcoinChallengeBlazorApp.Services {
    public class PeriodicTimer {
        public Timer Start(int RefreshTimeInSeconds, TimerCallback callback) {
            TimeSpan startTimeSpan = TimeSpan.Zero;
            TimeSpan periodTimeSpan = TimeSpan.FromSeconds(RefreshTimeInSeconds);
            return new Timer(callback, null, startTimeSpan, periodTimeSpan);
        }
    }
}
