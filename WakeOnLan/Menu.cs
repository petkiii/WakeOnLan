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

    private void DisplayMenu(Option selectedOption)
    {
        var length = 0;
        var line = new List<Option>();
        var height = 0;
        foreach (var option in _options)
        {
            if ((length += option.Name.Length + 2) > Console.WindowWidth)
            {
                DisplayLine(line, selectedOption, height);
                line.Clear();
                length = 0;
                height++;
            }

            line.Add(option);
        }

        DisplayLine(line, selectedOption, height);
        Console.SetCursorPosition(0, 0);
        Console.ResetColor();
    }

    private static void DisplayLine(List<Option> options, Option selectedOption, int height)
    {
        var length = 0;
        foreach (var option in options)
        {
            var width = options.Sum(x => x.Name.Length + 2);
            Console.SetCursorPosition((Console.WindowWidth - width) / 2 + length, Console.WindowHeight / 2 + height);

            length += option.Name.Length + 2;
            var isSelected = option == selectedOption;

            Console.ForegroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;

            Console.Write($" {option.Name} ");
        }
    }

    public void RunMenu()
    {
        var index = 0;
        DisplayMenu(_options[index]);

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
                DisplayMenu(_options[index]);
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