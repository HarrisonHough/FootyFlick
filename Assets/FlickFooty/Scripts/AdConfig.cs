public static class AdConfig
{
    public static string AppKey => GetAppKey();
    public static string BannerAdUnitId => GetBannerAdUnitId();
    public static string InterstitalAdUnitId => GetInterstitialAdUnitId();
    public static string RewardedVideoAdUnitId => GetRewardedVideoAdUnitId();

    static string GetAppKey()
    {
        #if UNITY_ANDROID
            return "229655ad5";
        #elif UNITY_IPHONE
            return "22965cde5";
        #else
            return "unexpected_platform";
        #endif
    }

    static string GetBannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "xj4738s4gl2sitac";
        #elif UNITY_IPHONE
            return "r9f7tswfw79u2anc";
        #else
            return "unexpected_platform";
        #endif
    }
    static string GetInterstitialAdUnitId()
    {
        #if UNITY_ANDROID
            return "ull6iieihfjbzbnw";
        #elif UNITY_IPHONE
            return "h1r0uac0lfckl6bq";
        #else
            return "unexpected_platform";
        #endif
    }

    static string GetRewardedVideoAdUnitId()
    {
        #if UNITY_ANDROID
            return "nydo690p7g5106p1";
        #elif UNITY_IPHONE
            return "9l87rn014me3otqi";
        #else
            return "unexpected_platform";
        #endif
    }
}
