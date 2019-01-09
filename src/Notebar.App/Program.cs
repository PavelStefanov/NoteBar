using CommandLine;
using System;
using System.ServiceModel;
using NoteBar.DeskBand;
using NoteBar.Core;
using NoteBar.App.NoteBarServiceReference;

namespace NoteBar.App
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
            var error = CheckOrShowDeskBand();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Cannot add indicator. Error: {error}");
                return;
            }

            error = AddIndicatorToNoteBarService(port);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Cannot add indicator. Error: {error}");
                return;
            }

            Console.WriteLine("Indicator was added");
        }

        private static string CheckOrShowDeskBand()
        {
            Guid notebarGuid = new Guid(Constants.NoteBarGuid);
            using (var trayDeskband = new TrayDeskband())
            {
                if (trayDeskband.IsDeskBandShown(notebarGuid))
                    return null;

                var showResult = trayDeskband.ShowDeskBand(notebarGuid);
                if (!showResult)
                    return "NoteBar not installed";

                return trayDeskband.IsDeskBandShown(notebarGuid) ? null :
                    "You have to turn NoteBar on";
            }
        }

        private static string AddIndicatorToNoteBarService(uint port)
        {
            var client = new NoteBarServiceClient();

            try
            {
                var response = client.AddIndicator(port);
                if (!string.IsNullOrEmpty(response))
                {
                    return response;
                }
            }
            catch (EndpointNotFoundException)
            {
                return "NoteBar is not running";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                try
                {
                    client.Close();
                }
                catch
                {
                    client.Abort();
                }
            }

            return null;
        }
    }
}