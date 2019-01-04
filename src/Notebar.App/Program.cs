using CommandLine;
using System;
using Notebar.App.NotebarServiceReference;
using System.ServiceModel;
using Notebar.DeskBand;
using Notebar.Core;

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
            var isShown = CheckOrShowDeskBand();
            if (!isShown)
            {
                Console.WriteLine("Cannot add indicator. Error: You have to turn Notebar on");
                return;
            }

            var error = AddIndicatorToNotebarService(port);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
                return;
            }

            Console.WriteLine("Indicator was added");
        }

        private static bool CheckOrShowDeskBand()
        {
            Guid notebarGuid = new Guid(Constants.NotebarGuid);
            using (var trayDeskband = new TrayDeskband())
            {
                if (trayDeskband.IsDeskBandShown(notebarGuid))
                    return true;

                var showResult = trayDeskband.ShowDeskBand(notebarGuid);
                if (!showResult)
                    return false;

                return trayDeskband.IsDeskBandShown(notebarGuid);
            }
        }

        private static string AddIndicatorToNotebarService(uint port)
        {
            var client = new NotebarServiceClient();

            try
            {
                var response = client.AddIndicator(port);
                if (!string.IsNullOrEmpty(response))
                {
                    return $"Cannot add indicator. Error: {response}";
                }
            }
            catch (EndpointNotFoundException)
            {
                return "Cannot add indicator. Error: Notebar is not running";
            }
            catch (Exception e)
            {
                return $"Cannot add indicator. Error: {e.Message}";
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