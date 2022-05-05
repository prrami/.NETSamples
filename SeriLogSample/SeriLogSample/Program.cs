using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SeriLogSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                configureSeriLog(Guid.NewGuid().ToString());
                Trace.TraceInformation("Hello World! This is serilog test");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
        static void configureSeriLog(string CorrelationID)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(getExecutingDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var listener = new global::SerilogTraceListener.SerilogTraceListener();
            Trace.Listeners.Clear();
            Trace.Listeners.Add(listener);

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("CorrelationID", CorrelationID)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        private static string getExecutingDirectory()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
        }
    }
}
