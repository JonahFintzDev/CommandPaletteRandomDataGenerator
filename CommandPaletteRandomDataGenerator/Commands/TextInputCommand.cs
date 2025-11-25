using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandPaletteRandomDataGenerator;

internal sealed partial class TextInputCommand : InvokableCommand
{

    private readonly char[] _specialCharacters = { '{', '}', '+', '^', '%', '~', '(', ')' };
    private readonly string _textToInput;

    public TextInputCommand(string textToInput)
    {
        _textToInput = textToInput;
        Icon = IconHelpers.FromRelativePath("Assets\\ShuffleIcon.png");
    }

    public override ICommandResult Invoke()
    {
        var _ = Task.Run(() =>
        {
            var previousClipboard = Clipboard.GetText();

            // wait to ensure the command palette is closed
            Thread.Sleep(Math.Min(200 + _textToInput.Length, 1000));

            // copy and paste the text
            ClipboardHelper.SetText(_textToInput);
            SendKeys.SendWait("^v");

            // ensure the clipboard is restored
            if(previousClipboard != string.Empty)
            {
                Thread.Sleep(200);
                ClipboardHelper.SetText(previousClipboard);
            }
        });

        return CommandResult.Dismiss();
    }
}