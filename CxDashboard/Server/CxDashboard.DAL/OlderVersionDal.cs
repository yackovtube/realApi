using CxDashboard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxDashboard.DAL
{
    public class OlderVersionDal
    {
        public ApplicationOlderVersionGrade SetApplictionOldVersionRC1(string version, double totalCalc)
        {
            ApplicationOlderVersionGrade appOldVer = new ApplicationOlderVersionGrade()
            { ApplicationVersion = version, RC1QuelityGrade = totalCalc, RC1DateTime = DateTime.Now };

            using (var dbContext = new DashboardContext())
            {
                var OpenOldVersion = dbContext.ApplicationOlderVersions.OrderByDescending(k => k.ApplicationVersionGradeId).FirstOrDefault();

                if (OpenOldVersion == null || OpenOldVersion.GAQuelityGrade > 0)
                {
                    var dashboard = dbContext.MainDashboard.FirstOrDefault();
                    if (dashboard != null)
                    {
                        appOldVer.MainDashboard = dashboard;

                        var entity = dbContext.ApplicationOlderVersions.Add(appOldVer);
                        dbContext.SaveChanges();
                        return entity;
                    } 
                }
            }
            return default(ApplicationOlderVersionGrade);
        }

        public ApplicationOlderVersionGrade SetApplicationGaGrade(double GAGrade)
        {
            using (var dbContext = new DashboardContext())
            {
                var OpenOldVersion = dbContext.ApplicationOlderVersions.OrderByDescending(k => k.ApplicationVersionGradeId).FirstOrDefault();

                if (OpenOldVersion != null && OpenOldVersion.GAQuelityGrade <= 0)
                {
                    var newGAGrade = OpenOldVersion;
                    newGAGrade.GAQuelityGrade = GAGrade;
                    newGAGrade.GADateTime = DateTime.Now;

                    dbContext.Entry(OpenOldVersion).CurrentValues.SetValues(newGAGrade);
                    dbContext.SaveChanges();
                    return newGAGrade;
                }
            }
            return default(ApplicationOlderVersionGrade);
        }
        public ApplicationOlderVersionGrade GetLastApplicationOldGrade()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.ApplicationOlderVersions.FirstOrDefault(a => a.RC1QuelityGrade > 0 && a.GAQuelityGrade == 0);
            }
        }

        public EngineVersion SetEngineOldVersion(string version)
        {
            EngineVersion newVersion = new EngineVersion() { IsCurrentVersion = true, EngineVersionNumber = version };

            var oldVersion = new List<EngineVersion>();

            using (var dbContext = new DashboardContext())
            {
                var OldCurrentVersion = dbContext.EngineVersions.Where(e => e.IsCurrentVersion == true).ToList();
                OldCurrentVersion.ForEach(c => c.IsCurrentVersion = false);

                dbContext.EngineVersions.Add(newVersion);
                dbContext.SaveChanges();

                return newVersion;
            }

        }

        public EngineVersion GetCurrentVersion()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.EngineVersions.FirstOrDefault(e => e.IsCurrentVersion == true);
            }
        }

        public List<EngineVersion> GetLastOldEngineVersion()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.EngineVersions.Where(e => e.IsCurrentVersion != true).OrderByDescending(e => e.VersionId).Take(6).ToList();
            }
        }
    }
}
