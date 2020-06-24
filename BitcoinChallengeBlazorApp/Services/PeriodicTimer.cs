using System;
using System.Threading;

namespace BitcoinChallengeBlazorApp.Services {
    public class PeriodicTimer {
        public int RefreshTimeInSeconds { get; private set; }
        public PeriodicTimer(AppSettings appSettings) {
            this.RefreshTimeInSeconds = appSettings.RefreshTimeInSeconds;
        }

        public Timer Start(TimerCallback callback) {
            TimeSpan startTimeSpan = TimeSpan.Zero;
            TimeSpan periodTimeSpan = TimeSpan.FromSeconds(this.RefreshTimeInSeconds);
            return new Timer(callback, null, startTimeSpan, periodTimeSpan);
        }
    }
}
