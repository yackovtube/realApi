using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.BL
{
    public static class StaticParams
    {
        static StaticParams()
        {
            TfsUrl = ConfigurationManager.AppSettings["TfsBaseUrl"];
            EngineAPIUrl = ConfigurationManager.AppSettings["EngineAPIBaseUrl"];
            SmokeApiUrl = ConfigurationManager.AppSettings["SmokeAPIBaseUrl"];
            EngineBuildDefinition = ConfigurationManager.AppSettings["EngineBuildDefinition"];
            AppBuildDefinition = ConfigurationManager.AppSettings["AppBuildDefinition"];
        }
        public static string TfsUrl { get; private set; }
        public static string EngineAPIUrl { get; private set; }
        public static string SmokeApiUrl { get; set; }
        public static string EngineBuildDefinition { get; set; }
        public static string AppBuildDefinition { get; set; }
    }
}
