using NexusMods.Abstractions.Serialization.ExpressionGenerator;
using NexusMods.App.UI.Pages.Changelog;
using NexusMods.App.UI.Pages.Diagnostics;
using NexusMods.App.UI.Pages.Diff.ApplyDiff;
using NexusMods.App.UI.Pages.Downloads;
using NexusMods.App.UI.Pages.Library;
using NexusMods.App.UI.Pages.LoadoutGrid;
using NexusMods.App.UI.Pages.LoadoutGroupFiles;
using NexusMods.App.UI.Pages.ModLibrary;
using NexusMods.App.UI.Pages.MyGames;
using NexusMods.App.UI.Pages.MyLoadouts;
using NexusMods.App.UI.Pages.Settings;
using NexusMods.App.UI.Pages.TextEdit;
using NexusMods.App.UI.WorkspaceSystem;

namespace NexusMods.App.UI;

internal class TypeFinder : ITypeFinder
{
    public IEnumerable<Type> DescendentsOf(Type type)
    {
        return AllTypes.Where(t => t.IsAssignableTo(type));
    }

    private static IEnumerable<Type> AllTypes => new[]
    {
        // factory context
        typeof(LoadoutGridContext),
        typeof(InProgressPageContext),
        typeof(MyGamesPageContext),
        typeof(DiagnosticListPageContext),
        typeof(ApplyDiffPageContext),
        typeof(SettingsPageContext),
        typeof(ChangelogPageContext),
        typeof(FileOriginsPageContext),
        typeof(TextEditorPageContext),
        typeof(MyLoadoutsPageContext),
        typeof(LoadoutGroupFilesPageContext),
        typeof(LibraryPageContext),

        // workspace context
        typeof(EmptyContext),
        typeof(HomeContext),
        typeof(LoadoutContext),
        typeof(DownloadsContext),

        // other
        typeof(WindowData),
    };
}
