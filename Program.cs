using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Linq;

internal class Program
{
    static string _path = "note1.txt";

    public static HashSet<string> Table_1 { get; set; }
    public static SortedSet<string> Table_2 { get; set; }
    private static async Task Main(string[] args)
    {
        await WriteIdentificators(20000);

        var stopWatchTable1 = new Stopwatch();
        var stopWatchTable2 = new Stopwatch();

        var identificators = await ReadIdentificators();

        stopWatchTable1.Start();
        Table_1 = new HashSet<string>(identificators);
        stopWatchTable1.Stop();

        stopWatchTable2.Start();
        Table_2 = new SortedSet<string>(identificators);
        stopWatchTable2.Stop();

        Console.WriteLine("Инициализация 1 таблицы: " + stopWatchTable1.Elapsed.ToString());
        Console.WriteLine("Инициализация 1 таблицы: " + stopWatchTable2.Elapsed.ToString());

        Random rnd = new Random();
        var indexes = new int[2000];

        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = rnd.Next(0, 20000-1);
        }

        stopWatchTable1.Restart();
        for (int i = 0; i < indexes.Length; i++)
        {
            Table_1.Contains(identificators[indexes[i]]);
        }
        stopWatchTable1.Stop();

        stopWatchTable2.Restart();
        for (int i = 0; i < indexes.Length; i++)
        {
            Table_2.Contains(identificators[indexes[i]]);
        }
        stopWatchTable2.Stop();

        Console.WriteLine("Случайный поиск 1 таблицы: " + stopWatchTable1.Elapsed.ToString());
        Console.WriteLine("Случайный поиск 2 таблицы: " + stopWatchTable2.Elapsed.ToString());
    }

    private static async Task WriteIdentificators(int count)
    {
        var identificators = new List<string>();

        for (int i = 0; i < count; i++)
        {
            var newIdentificator = Guid.NewGuid().ToString();
            identificators.Add(newIdentificator);
        }

        var sb = new StringBuilder();

        sb.AppendJoin('\n', identificators);

        using (var writer = new StreamWriter(_path, false))
        {
            await writer.WriteLineAsync(sb.ToString());
        }
    }
    private static async Task<List<string>> ReadIdentificators()
    {
        var identificators = new List<string>();

        using (var reader = new StreamReader(_path))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                identificators.Add(line);
            }
        }

        return identificators;
    }
}