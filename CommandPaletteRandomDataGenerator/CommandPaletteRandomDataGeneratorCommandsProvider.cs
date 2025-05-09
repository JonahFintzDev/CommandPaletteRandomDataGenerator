// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandPaletteRandomDataGenerator;

public partial class CommandPaletteRandomDataGeneratorCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public CommandPaletteRandomDataGeneratorCommandsProvider()
    {
        DisplayName = "Random Data Generator";
        Icon = IconHelpers.FromRelativePath("Assets\\ShuffleIcon.png");
        _commands = [
            new CommandItem(new CommandPaletteRandomDataGeneratorPage()) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }
}
