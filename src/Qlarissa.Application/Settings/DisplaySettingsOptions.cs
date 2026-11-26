namespace Qlarissa.Application.Settings;

public sealed class DisplaySettingsOptions
{
    public const string SectionName = "DisplaySettings";

    public int MaxRowsPerUser { get; set; } = 3000;
    public int MaxKeyLength { get; set; } = 128;
    public int MaxValueLength { get; set; } = 4000;
    public int MaxChangedKeysPerRequest { get; set; } = 100;
    public int MaxRequestBodyBytes { get; set; } = 65536;
    public int SyncDebounceMilliseconds { get; set; } = 500;
    public int RequestsPerWindow { get; set; } = 120;
    public int RateLimitWindowSeconds { get; set; } = 60;

    public bool IsValid =>
        MaxRowsPerUser >= 1
        && MaxKeyLength is >= 1 and <= 128
        && MaxValueLength is >= 1 and <= 4000
        && MaxChangedKeysPerRequest >= 1
        && MaxRequestBodyBytes >= 1
        && SyncDebounceMilliseconds >= 0
        && RequestsPerWindow >= 1
        && RateLimitWindowSeconds >= 1;
}
