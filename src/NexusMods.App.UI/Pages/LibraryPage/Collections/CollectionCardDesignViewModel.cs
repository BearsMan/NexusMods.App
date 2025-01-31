using Avalonia.Media.Imaging;
using Avalonia.Platform;
using NexusMods.Abstractions.Jobs;
using NexusMods.Abstractions.UI;
using NexusMods.App.UI.Controls.Navigation;
using NexusMods.Paths;

namespace NexusMods.App.UI.Pages.LibraryPage.Collections;

public class CollectionCardDesignViewModel : AViewModel<ICollectionCardViewModel>, ICollectionCardViewModel
{
    public string Name => "Vanilla+ [Quality of Life]";
    public Bitmap Image => new(AssetLoader.Open(new Uri("avares://NexusMods.App.UI/Assets/DesignTime/collection_tile_image.png")));
    public string Summary => "1.6.8 This visual mod collection is designed to create a witchy medieval cottage aethetic experience for Stardew Valley, and Stardew Valley Expanded.";
    public string Category => "Themed";
    public int NumDownloads => 9;
    public ulong EndorsementCount => 248;
    public ulong TotalDownloads => 30_000;
    public Size TotalSize => Size.From(907 * 1024 * 1024);
    public Percent OverallRating => Percent.CreateClamped(0.84);
    public bool IsAdult => true;
    public string AuthorName => "FantasyAuthor";
    public Bitmap AuthorAvatar => new(AssetLoader.Open(new Uri("avares://NexusMods.App.UI/Assets/DesignTime/avatar.webp")));
    public R3.ReactiveCommand<NavigationInformation> OpenCollectionDownloadPageCommand { get; } = new(canExecuteSource: R3.Observable.Return(false), initialCanExecute: false);
}
