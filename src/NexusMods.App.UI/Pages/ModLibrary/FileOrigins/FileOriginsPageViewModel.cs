using System.Collections.ObjectModel;
using Humanizer.Bytes;
using NexusMods.Abstractions.FileStore;
using NexusMods.Abstractions.FileStore.ArchiveMetadata;
using NexusMods.Abstractions.FileStore.Downloads;
using NexusMods.Abstractions.Installers;
using NexusMods.Abstractions.Loadouts;
using NexusMods.App.UI.Pages.ModLibrary.FileOriginEntry;
using NexusMods.App.UI.Windows;
using NexusMods.App.UI.WorkspaceSystem;
using NexusMods.Paths;
using ReactiveUI;

namespace NexusMods.App.UI.Pages.ModLibrary;

public class FileOriginsPageViewModel : APageViewModel<IFileOriginsPageViewModel>, IFileOriginsPageViewModel
{
    public LoadoutId LoadoutId { get; set; }

    public ReadOnlyObservableCollection<IFileOriginEntryViewModel> FileOrigins { get; }

    public FileOriginsPageViewModel(
        IArchiveInstaller archiveInstaller,
        IFileOriginRegistry fileOriginRegistry,
        IWindowManager windowManager) : base(windowManager)
    {
        var allFileOrigins = fileOriginRegistry.GetAll();

        FileOrigins = new ReadOnlyObservableCollection<IFileOriginEntryViewModel>(
            new ObservableCollection<IFileOriginEntryViewModel>(
                allFileOrigins.Select(fileOrigin =>
                    {
                        RelativePath name = fileOrigin.Contains(DownloadAnalysis.SuggestedName)
                            ? fileOrigin.Get(DownloadAnalysis.SuggestedName)
                            : fileOrigin.Get(FilePathMetadata.OriginalName);

                        return new FileOriginEntryViewModel
                        {
                            Name = name,
                            Size = ByteSize.FromBytes(fileOrigin.Size.Value).ToString(),
                            AddToLoadoutCommand = ReactiveCommand.CreateFromTask(async () =>
                                {
                                    await archiveInstaller.AddMods(LoadoutId, fileOrigin, name);
                                }
                            ),
                        };
                    }
                )
            )
        );
    }
}
