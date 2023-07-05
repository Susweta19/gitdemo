using System;
using TxWebTests;
using TxWebTests.Config;
using XmlExecutor;
using XmlExecutor.Attributes;
using XmlExecutor.Factories;
using System.IO;
using TxSelenium;

namespace ConsoleLauncher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(args);
        }

        public void Run(string[] args)
        {
            string mode = args[0];
            //This is to force the TxSelenium assembly to load in the appdomain it's hackish but it works
            //TODO break this logic link to xmlexecutor it's bad practice,
            //TxWebTests should not have to know internal workings of XmlExecutor
            Console.WriteLine(typeof(TxWebDriver).ToString());

            AttributeUtils attributeUtils = new AttributeUtils(typeof(TxActionAttribute));
            var watch = System.Diagnostics.Stopwatch.StartNew();
            switch (mode)
            {
                case "run":
                    string configPath = args[1];
                    Configuration config = ConfigManager.GetConfig(attributeUtils, configPath);
                    ParsedTests test = new ParsedTests(attributeUtils, config);
                    test.RunTests();
                    watch.Stop();
                    long elapsedMs = watch.ElapsedMilliseconds;
                    TimeSpan span = TimeSpan.FromMilliseconds(elapsedMs);
                    double seconds = span.TotalSeconds;
                    using (StreamWriter fileWriter = new StreamWriter(config.FileTimePath))
                    {
                        fileWriter.WriteLine("Tests time (s): " + seconds);
                    }
                    break;
                case "generateschema":
                    Console.WriteLine("********** Generating Schema **********");
                    SchemaGenerator.GenerateXsd(attributeUtils, typeof(TestManager), "TxWebTests.xsd");
                    SchemaGenerator.GenerateXsd(attributeUtils, typeof(ConfigManager), "TxTestsConfig.xsd");
                    watch.Stop();
                    long generateMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("Generate time (ms): " + generateMs);
                    Console.WriteLine("********** Schema Generated **********");
                    break;
                default:
                    throw new System.Exception("First argument \"mode\" was not recognized: " + mode);
            }
        }

    }
}
