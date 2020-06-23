namespace BitcoinChallengeBlazorApp {
    public class BitcoinChallengeSettings {
        public int RefreshTimeInSeconds { get; private set; }
        public BitcoinChallengeSettings(AppSettings appSettings) {
            this.RefreshTimeInSeconds = appSettings.RefreshTimeInSeconds;
        }
    }
}