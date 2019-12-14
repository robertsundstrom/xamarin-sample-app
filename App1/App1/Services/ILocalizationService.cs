namespace App1.Services
{
    internal interface ILocalizationService
    {
        string GetString(string key);
        string GetString(string key, params object[] args);
    }
}