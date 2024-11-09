namespace TiktokLiveRec.Core;

public sealed partial class TiktokSpider : ISpider
{
    public static Lazy<TiktokSpider> Instance { get; } = new(() => new TiktokSpider());

    public ISpiderResult GetResult(string url)
    {
        // TODO
        return null!;
    }
}
