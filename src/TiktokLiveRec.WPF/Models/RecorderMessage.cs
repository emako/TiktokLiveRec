namespace TiktokLiveRec.Models;

internal sealed class RecorderMessage
{
    public object? Sender { get; set; }
    public string? Data { get; set; }
    public StandardData DataType { get; set; }
}

public enum StandardData
{
    None,
    StandardError,
    StandardOutput,
}
