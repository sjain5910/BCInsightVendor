using BCInsight.BAL.Repository;
using BCInsight.DAL;
using Quartz;
using System;
using System.Linq;

namespace BCInsight.BAL.Services
{

    public class Jobclass : IJob
    {   
        public void Execute(IJobExecutionContext context)
        {
            // Generate week management every year eg.march month  
            WeeksRepository repo = new WeeksRepository();
            repo.Startshedular();
            QuartersRepository quarterrepo = new QuartersRepository();
            quarterrepo.Startquartershedular();
        }
    }
}
