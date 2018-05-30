using CxDashboard.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CxDashboard.DAL
{
    public class MainDashboardDal
    {
        public void SaveTimeLine(byte[] timeLine)
        {
            if (timeLine != null)
            {
                using (var db = new DashboardContext())
                {
                    db.MainDashboard.FirstOrDefault().Timeline = timeLine;
                    db.SaveChanges();
                } 
            }
        }

        public byte[] GetTimeLine()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.MainDashboard.FirstOrDefault().Timeline;
            }
        }

        public List<ApplicationOlderVersionGrade> GetLastApplicationVersions()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.ApplicationOlderVersions.OrderByDescending(v => v.ApplicationVersionGradeId).Take(6).ToList();
            }
        }
    }
}
