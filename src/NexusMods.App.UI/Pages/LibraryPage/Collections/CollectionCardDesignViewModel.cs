using Avalonia.Media.Imaging;
using Avalonia.Platform;
using NexusMods.Abstractions.Jobs;
using NexusMods.Paths;

namespace NexusMods.App.UI.Pages.LibraryPage.Collections;

public class CollectionCardDesignViewModel : AViewModel<ICollectionCardViewModel>, ICollectionCardViewModel
{
    public string Name => "Vanilla+ [Quality of Life]";
    public Bitmap Image => new Bitmap(AssetLoader.Open(new Uri("avares://NexusMods.App.UI/Assets/DesignTime/cyberpunk_game.png")));
    public string Summary => "1.6.8 This visual mod collection is designed to create a witchy medieval cottage aethetic experience for Stardew Valley, and Stardew Valley Expanded.";
    public string Category => "Themed";
    public int ModCount => 9;
    public ulong EndorsementCount => 248;
    public ulong DownloadCount => 30000;
    public Size TotalSize => Size.From(907 * 1024 * 1024);
    public Percent OverallRating => Percent.CreateClamped(0.84);
    public string AuthorName => "FantasyAuthor";
    public Bitmap AuthorAvatar => new("https://avatars.nexusmods.com/17252164/100");
}
