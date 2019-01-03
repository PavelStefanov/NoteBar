using CommandLine;
using System;
using Notebar.App.NotebarServiceReference;

namespace Notebar.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(options => AddIndicator(options.Port));
        }

        private static void AddIndicator(uint port)
        {
            Console.WriteLine("Adding indicator...");
            using (var client = new NotebarServiceClient())
            {
                try
                {
                    var response = client.AddIndicator(port);
                    if (!string.IsNullOrEmpty(response))
                    {
                        Console.WriteLine($"Cannot add indicator. Error: {response}");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot add indicator. Error: {e.Message}");
                    return;
                }
            }
            Console.WriteLine("Indicator was added");
        }
    }
}
