using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Tasker
{
    public class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string BaseUrlKey = "base_url_key";
        private static readonly string BaseUrlKeyDefault = @"http://10.0.3.2:5000";

        private const string CurrentTokenKey = "current_token";
        private static readonly string CurrentTokenDefault = string.Empty;

        #endregion


        public static string BaseUrl 
        {
            get => AppSettings.GetValueOrDefault(BaseUrlKey, BaseUrlKeyDefault);
            set => AppSettings.AddOrUpdateValue(BaseUrlKey, value);
        }

        public static string CurrentToken
        {
            get => AppSettings.GetValueOrDefault(CurrentTokenKey, CurrentTokenDefault);
            set => AppSettings.AddOrUpdateValue(CurrentTokenKey, value);
        }
    }
}
