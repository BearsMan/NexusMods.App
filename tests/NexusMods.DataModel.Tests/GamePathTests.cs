using FluentAssertions;
using NexusMods.Paths;
using NexusMods.Paths.Extensions;
using NexusMods.Paths.Utilities;

namespace NexusMods.DataModel.Tests;

public class GamePathTests
{
    private readonly InMemoryFileSystem _fileSystem;

    public GamePathTests()
    {
        _fileSystem = new InMemoryFileSystem();
    }

    [Fact]
    public void CanComparePaths()
    {
        var pathA = new GamePath(LocationId.Game, "foo/bar.zip");
        var pathB = new GamePath(LocationId.Game, "Foo/bar.zip");
        var pathC = new GamePath(LocationId.Preferences, "foo/bar.zip");
        var pathD = new GamePath(LocationId.Game, "foo/bar.pex");

        Assert.Equal(pathA, pathB);
        Assert.NotEqual(pathA, pathC);
        Assert.NotEqual(pathA, pathD);

        Assert.True(pathA == pathB);
        Assert.False(pathA == pathC);
        Assert.True(pathA != pathC);
    }

    [Fact]
    public void CanTreatLikeIPath()
    {
        var pathA = new GamePath(LocationId.Game, "foo/bar.zip");
        var ipath = (IPath)pathA;

        Assert.Equal(KnownExtensions.Zip, ipath.Extension);
        Assert.Equal("bar.zip".ToRelativePath(), ipath.FileName);
    }

    [Fact]
    public void CanGetHashCode()
    {
        var pathA = new GamePath(LocationId.Game, "foo/bar.zip");
        var pathB = new GamePath(LocationId.Game, "foo/ba.zip");

        Assert.Equal(pathA.GetHashCode(), pathA.GetHashCode());
        Assert.NotEqual(pathA.GetHashCode(), pathB.GetHashCode());
    }

    [Fact]
    public void CanConvertToString()
    {
        var pathA = new GamePath(LocationId.Game, "foo/bar.zip");
        var pathB = new GamePath(LocationId.Saves, "foo/ba.zip");
        Assert.Equal("{Game}/foo/bar.zip", pathA.ToString());
        Assert.Equal("{Saves}/foo/ba.zip", pathB.ToString());
    }

    [Fact]
    public void CanGetPathRelativeTo()
    {
        var baseFolder = _fileSystem.GetKnownPath(KnownPath.CurrentDirectory);
        var pathA = new GamePath(LocationId.Game, "foo/bar");
        Assert.Equal(baseFolder.Combine("foo/bar"), pathA.Combine(baseFolder));
    }

    [Theory]
    [InlineData("Game", "", "")]
    [InlineData("Game", "foo", "foo")]
    [InlineData("Game", "foo/bar", "bar")]
    public void Test_Name(LocationId folderType, string input, string expected)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.Name.Should().Be(expected);
    }


    [Theory]
    [InlineData("Game", "", "")]
    [InlineData("Game", "foo", "")]
    [InlineData("Game", "foo/bar", "foo")]
    [InlineData("Game", "foo/bar/baz", "foo/bar")]
    public void Test_Parent(LocationId folderType, string input, string expectedParent)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.Parent.Should().Be(new GamePath(folderType, (RelativePath)expectedParent));
    }

    [Theory]
    [InlineData("Game", "", "")]
    [InlineData("Game", "foo", "")]
    [InlineData("Preferences", "foo/bar", "")]
    [InlineData("Saves", "foo/bar/baz", "")]
    public void Test_GetRootComponent(LocationId folderType, string input, string expectedRootComponent)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.GetRootComponent.Should().Be(new GamePath(folderType, (RelativePath)expectedRootComponent));
    }

    [Theory]
    [InlineData("Game", "", new string[] { })]
    [InlineData("Game", "foo", new string[] { "foo" })]
    [InlineData("Game", "foo/bar", new string[] { "foo", "bar" })]
    [InlineData("Saves", "foo/bar/baz", new string[] { "foo", "bar", "baz" })]
    public void Test_Parts(LocationId folderType, string input, string[] expectedParts)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.Parts.Should().BeEquivalentTo(expectedParts.Select(x => new RelativePath(x)));
    }

    [Theory]
    [InlineData("Game", "", new string[] { ""})]
    [InlineData("Game", "foo", new string[] { "foo", "" })]
    [InlineData("Saves", "foo/bar", new string[] { "foo/bar", "foo", "" })]
    [InlineData("Game", "foo/bar/baz", new string[] { "foo/bar/baz", "foo/bar", "foo", "" })]
    public void Test_GetAllParents(LocationId folderType, string input, string[] expectedParts)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.GetAllParents().Should()
            .BeEquivalentTo(expectedParts.Select(x => new GamePath(folderType, (RelativePath)x)));
    }

    [Theory]
    [InlineData("Game", "", "")]
    [InlineData("Game", "foo", "foo")]
    [InlineData("Preferences", "foo/bar", "foo/bar")]
    [InlineData("Saves", "foo/bar/baz", "foo/bar/baz")]
    public void Test_GetNonRootPart(LocationId folderType, string input, string expected)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.GetNonRootPart().Should().Be((RelativePath)expected);
    }

    [Theory]
    [InlineData("Game", "", true)]
    [InlineData("Saves", "foo", true)]
    [InlineData("Preferences", "foo/bar", true)]
    public void Test_IsRooted(LocationId folderType, string input, bool expected)
    {
        var path = new GamePath(folderType, (RelativePath)input);
        path.IsRooted.Should().Be(expected);
    }

    [Theory]
    [InlineData("Game", "foo", "Game", "bar", false)]
    [InlineData("Game", "foo/bar", "Game", "foo", true)]
    [InlineData("Game", "foo/bar/baz", "Game", "foo/bar", true)]
    [InlineData("Game", "foo", "Saves", "bar", false)]
    [InlineData("Saves", "foo/bar", "Game", "foo", false)]
    [InlineData("Game", "foo/bar/baz", "Preferences", "foo/bar", false)]
    public void Test_InFolder(LocationId folderTypeLeft, string left, LocationId folderTypeRight, string right,
        bool expected)
    {
        var leftPath = new GamePath(folderTypeLeft, (RelativePath)left);
        var rightPath = new GamePath(folderTypeRight, (RelativePath)right);
        var actual = leftPath.InFolder(rightPath);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("Game","","Game", "", true)]
    [InlineData("Game","","Game", "foo", false)]
    [InlineData("Game","foo","Game", "bar", false)]
    [InlineData("Game","foo","Game", "", true)]
    [InlineData("Game","foo/bar/baz","Game", "", true)]
    [InlineData("Game","foo/bar/baz","Game", "foo", true)]
    [InlineData("Game","foo/bar/baz","Game", "foo/bar", true)]
    [InlineData("Game","foobar","Game", "foo", false)]
    [InlineData("Game","foo/bar/baz","Game", "foo/baz", false)]
    [InlineData("Game","","Preferences", "", false)]
    [InlineData("Game","foo/bar/baz","Preferences", "", false)]
    [InlineData("Game","foo/bar/baz","Preferences", "foo", false)]
    [InlineData("Game","foo/bar/baz","Preferences", "foo/bar", false)]
    [InlineData("Game","foo","Preferences", "", false)]
    public void Test_StartsWithGamePath(LocationId folderTypeChild, string child,LocationId folderTypeParent, string parent, bool expected)
    {
        var childPath = new GamePath(folderTypeChild, (RelativePath)child);
        var parentPath = new GamePath(folderTypeParent, (RelativePath)parent);
        var actual = childPath.StartsWith(parentPath);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("Game","", "", true)]
    [InlineData("Game","", "foo", false)]
    [InlineData("Game","foo", "bar", false)]
    [InlineData("Game","foo", "", true)]
    [InlineData("Game","foo/bar/baz", "", true)]
    [InlineData("Game","foo/bar/baz", "bar/baz", true)]
    [InlineData("Game","foo/bar/baz", "foo/bar/baz", true)]
    [InlineData("Game","foobar", "bar", false)]
    [InlineData("Game","foo/bar/baz", "foo/baz", false)]
    public void Test_EndsWithRelative(LocationId folderType,string child, string parent, bool expected)
    {
        var childPath = new GamePath(folderType, (RelativePath)child);
        var parentPath = (RelativePath)parent;
        var actual = childPath.EndsWith(parentPath);
        actual.Should().Be(expected);
    }
}
