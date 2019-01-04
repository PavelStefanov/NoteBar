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
            var error = CheckOrShowDeskBand();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Cannot add indicator. Error: {error}");
                return;
            }

            error = AddIndicatorToNotebarService(port);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Cannot add indicator. Error: {error}");
                return;
            }

            Console.WriteLine("Indicator was added");
        }

        private static string CheckOrShowDeskBand()
        {
            Guid notebarGuid = new Guid(Constants.NotebarGuid);
            using (var trayDeskband = new TrayDeskband())
            {
                if (trayDeskband.IsDeskBandShown(notebarGuid))
                    return null;

                var showResult = trayDeskband.ShowDeskBand(notebarGuid);
                if (!showResult)
                    return "Notebar not installed";

                return trayDeskband.IsDeskBandShown(notebarGuid) ? null :
                    "You have to turn Notebar on";
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
                    return response;
                }
            }
            catch (EndpointNotFoundException)
            {
                return "Notebar is not running";
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