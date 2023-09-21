using System.Text;

namespace WakeOnLan;

internal class Menu
{
    private readonly int _maxWidth;
    private readonly Option[] _options;

    private Menu(Option[] options)
    {
        _options = options;
        _maxWidth = _options.Sum(x => x.Name.Length + 2);
    }

    public static Menu CreateMenu(Option[] options)
    {
        if (options.Length == 0)
            throw new ArgumentException("Options cannot be empty", nameof(options));

        return new Menu(options);
    }


    private void DisplayMultiLineMenu(Option selectedOption)
    {
        var linebreaks = GetLinebreaks();
        for (var i = 0; i < linebreaks.Count; i++)
            DisplayLine(linebreaks[i], selectedOption, i);

        Console.ResetColor();
    }

    private void DisplayLine(Linebreak linebreak, Option selectedOption, int height)
    {
        var length = 0;
        foreach (var option in linebreak.GetLine(_options))
        {
            Console.SetCursorPosition((Console.WindowWidth - linebreak.TextLength) / 2 + length,
                Console.WindowHeight / 2 + height);

            length += option.Name.Length + 2;

            var isSelected = option == selectedOption;
            Console.ForegroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;

            Console.Write($" {option.Name} ");
        }
    }

    private record Linebreak(int StartIndex, int EndIndex, int TextLength)
    {
        public ReadOnlySpan<Option> GetLine(Option[] options) =>
            options.AsSpan(new Range(StartIndex, EndIndex));
    }

    private List<Linebreak> GetLinebreaks()
    {
        var extraLines = _maxWidth / Console.WindowWidth;
        var lineBreaks = new List<Linebreak>(extraLines + 1);

        var length = 0;
        var previous = 0;
        for (var i = 0; i < _options.Length; i++)
        {
            var option = _options[i];

            // [i] doesn't fit
            if (length + option.Name.Length + 2 > Console.WindowWidth)
            {
                lineBreaks.Add(new Linebreak(previous, i, length));
                previous = i;
                length = 0;
            }

            length += option.Name.Length + 2;
        }

        //don't forget the last line
        lineBreaks.Add(new Linebreak(previous, _options.Length, length));
        return lineBreaks;
    }

    public void RunMenu()
    {
        var index = 0;
        DisplayMultiLineMenu(_options[index]);

        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.Unicode;

        do
        {
            var key = Console.ReadKey(true);
            var update = true;
            var stop = false;

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    index = index == _options.Length - 1 ? 0 : index + 1;
                    break;

                case ConsoleKey.LeftArrow:
                    index = index == 0 ? _options.Length - 1 : index - 1;
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();

                    _options[index].Action?.Invoke();
                    stop = _options[index].Stop;
                    break;

                default:
                    update = false;
                    break;
            }

            if (stop)
                break;

            if (update)
                DisplayMultiLineMenu(_options[index]);
        } while (true);
    }
}

public class Option
{
    public string Name { get; }
    public Action? Action { get; }
    public bool Stop { get; }

    public Option(string name, Action? action = null, bool stop = false)
    {
        Name = name;
        Action = action;
        Stop = stop;
    }
}