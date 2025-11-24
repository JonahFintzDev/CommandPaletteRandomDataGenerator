// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommandPaletteRandomDataGenerator;

internal sealed partial class CommandPaletteRandomDataGeneratorPage : DynamicListPage
{
    public CommandPaletteRandomDataGeneratorPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\ShuffleIcon.png");
        Title = "Random Data";
        Name = "Open";
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        this.RaiseItemsChanged();
    }

    private static string GetRandomString(int length, bool includeSpecialChars)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        if (includeSpecialChars)
        {
            chars += "!@#$%^&*()_+[]{}|;:,.<>?";
        }

        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string GetRandomNumber(int length)
    {
        string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // New: Generate a random port between 12000 and 65534
    private static string GetRandomPort()
    {
        // RandomNumberGenerator.GetInt32 upper bound is exclusive, so use 65535
        int port = RandomNumberGenerator.GetInt32(12000, 65535);
        return port.ToString(CultureInfo.InvariantCulture);
    }

    // New: Generate a random color in hex format #RRGGBB
    private static string GetRandomColorHex()
    {
        int r = RandomNumberGenerator.GetInt32(0, 256);
        int g = RandomNumberGenerator.GetInt32(0, 256);
        int b = RandomNumberGenerator.GetInt32(0, 256);
        return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", r, g, b);
    }

    // New: Generate a random RGB color (comma separated)
    private static string GetRandomColorRgb()
    {
        int r = RandomNumberGenerator.GetInt32(0, 256);
        int g = RandomNumberGenerator.GetInt32(0, 256);
        int b = RandomNumberGenerator.GetInt32(0, 256);
        return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", r, g, b);
    }

    private static string GetSha256Hash()
    {
        var Hash = SHA256.HashData(Encoding.UTF8.GetBytes(GetRandomString(32, true)));
        return Convert.ToHexStringLower(Hash);
    }

    private static string GetSha512Hash()
    {
        var Hash = SHA512.HashData(Encoding.UTF8.GetBytes(GetRandomString(32, true)));
        return Convert.ToHexStringLower(Hash);
    }

    public override IListItem[] GetItems()
    {

        if (int.TryParse(SearchText, out int length))
        {
            // check if length is between 1 and 999
            if (length > 0 && length < 999)
            {
                // return random string of length
                return [
                    // random string
                    new ListItem(new TextInputCommand(GetRandomString(length, false))) { Title = $"Random String ({length})" },
                    new ListItem(new TextInputCommand(GetRandomString(length, true))) { Title = $"Random String ({length}) with special chars" },
                    // lorem ipsum
                    new ListItem(new TextInputCommand(LoremIpsum.GetLoremIpsum(length))) { Title = $"Lorem Ipsum ({length})" }
                ];
            }
            else
            {
                return [
                    // random string
                    new ListItem(new TextInputCommand(LoremIpsum.GetLoremIpsum(length))) { Title = $"Lorem Ipsum ({length})" }
                ];
            }
        }
        else
        {
            var items = new List<ListItem>()
            {
                // random 16 char string
                new ListItem(new TextInputCommand(GetRandomString(16, false))) { Title = "String (16)" },
                // random 16 char string with special chars
                new ListItem(new TextInputCommand(GetRandomString(16, true))) { Title = "String (16) with special chars" },
                // random 32 char string
                new ListItem(new TextInputCommand(GetRandomString(32, false))) { Title = "String (32)" },
                // random 32 char string with special chars
                new ListItem(new TextInputCommand(GetRandomString(32, true))) { Title = "String (32) with special chars" },
                // random 16 char number
                new ListItem(new TextInputCommand(GetRandomNumber(16))) { Title = "Number (16)" },
                // random 32 char number
                new ListItem(new TextInputCommand(GetRandomNumber(32))) { Title = "Number (32)" },
                // uuid
                new ListItem(new TextInputCommand(System.Guid.NewGuid().ToString())) { Title = "UUID",  },
                // port
                new ListItem(new TextInputCommand(GetRandomPort())) { Title = "Port (12000-65534)" },
                // color hex
                new ListItem(new TextInputCommand(GetRandomColorHex())) { Title = "Color Hex" },
                // color rgb
                new ListItem(new TextInputCommand(GetRandomColorRgb())) { Title = "Color RGB (R,G,B)" },
                // sha256
                new ListItem(new TextInputCommand(GetSha256Hash())) { Title = "SHA-256" },
                // sha256
                new ListItem(new TextInputCommand(GetSha512Hash())) { Title = "SHA-512" },
                // lorem ipsum
                new ListItem(new TextInputCommand(LoremIpsum.GetLoremIpsum(32))) { Title = "Lorem Ipsum (32)" },
                new ListItem(new TextInputCommand(LoremIpsum.GetLoremIpsum(64))) { Title = "Lorem Ipsum (64)" },
                // hint
                new ListItem(new NoOpCommand()) {
                    Title = "Enter a number to generate a random string of that length",
                    Subtitle = "e.g. 16, 32, 64",
                    Icon = new IconInfo("\xE946") // info icon
                }
            };

            // filter items based on search text
            if (SearchText != null && SearchText.Length > 0)
            {
                items = items.Where(x => x.Title.ToLower(CultureInfo.InvariantCulture).Contains(SearchText.ToLower(CultureInfo.InvariantCulture))).ToList();
            }

            if (items.Count == 0)
            {
                return new List<ListItem>() {
                    new ListItem(new NoOpCommand())
                    {
                        Title = "Enter a number to generate a random string of that length",
                        Subtitle = "e.g. 16, 32, 64",
                        Icon = new IconInfo("\xE946") // info icon
                    }
                }.ToArray();
            } else
            {
                return items.ToArray();
            }

        }
    }
}
