﻿using NexusMods.CLI;
using NexusMods.CLI.DataOutputs;

namespace NexusMods.Networking.NexusWebApi.Verbs;

public class NexusGames : AVerb
{
    private readonly Client _client;

    public NexusGames(Client client, Configurator configurator)
    {
        _client = client;
        _renderer = configurator.Renderer;
    }
    public static readonly VerbDefinition Definition = new VerbDefinition("nexus-games", "Lists all games available on Nexus Mods", 
        Array.Empty<OptionDefinition>());

    private readonly IRenderer _renderer;


    protected override async Task<int> Run(CancellationToken token)
    {
        var results = await _client.Games(token);

        await _renderer.Render(new Table(new[] { "Name", "Domain", "Downloads", "Files" },
            results.Data
                .OrderByDescending(x => x.FileCount)
                .Select(x => new object[] { x.Name, x.DomainName, x.Downloads, x.FileCount })));

        return 0;
    }
}