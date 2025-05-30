﻿using Echeckdem.Handlers;
using Echeckdem.Models;
using Echeckdem.ViewModel;

namespace Echeckdem.Services
{
    public class DashboardSummary: IDashboardSummary
        

    {
        private readonly DbEcheckContext _context;
        private readonly HttpContext _httpContext;
        public DashboardSummary(DbEcheckContext context, IHttpContextAccessor _httpContextAccessor)
        {
            _context = context;
            _httpContext = _httpContextAccessor.HttpContext;
        }
        public  List<ProjectDashboardStatus> GetProjectDashboardSummary()
        {
            var uno = _httpContext.Session.GetInt32("UNO");
            var ulevel = _httpContext.Session.GetInt32("User Level");
            if (ulevel==1) {
                var result = (
                    from task in _context.Ncbocws
                    join scope in _context.BocwScopes on task.ScopeId equals scope.ScopeId
                    join project in _context.Ncmlocs on task.Lcode equals project.Lcode
                    
                    where scope.ScopeActive == 1 && project.Lactive == 1
                    group new { task, scope, project } by new { task.Lcode, project.Lname } into g
                    select new ProjectDashboardStatus
                    {

                        ProjectId = g.Key.Lcode,
                        ProjectName = g.Key.Lname,
                        Startdate = (from k in _context.Ncmlocbos where k.Lcode == g.Key.Lcode select k.ProjectStartDateEst).FirstOrDefault(),
                        Enddate = (from k in _context.Ncmlocbos where k.Lcode == g.Key.Lcode select k.ProjectEndDateEst).FirstOrDefault(),
                        Oid = g.FirstOrDefault().project.Oid,
                        StartupTotal = g.Count(x => x.scope.Category == "Startup"),
                        StartupPending = g.Count(x => x.scope.Category == "Startup" && ( x.task.Status < 0)),
                        StartupInProgress = g.Count(x => x.scope.Category == "Startup" && (x.task.Status < 3 && x.task.Status >= 0)),
                        StartupCompleted = g.Count(x => x.scope.Category == "Startup" && x.task.Status == 3),

                        TrainingTotal = g.Count(x => x.scope.Category == "Training"),
                        TrainingPending = g.Count(x => x.scope.Category == "Training" && (x.task.Status < 0)),
                        TrainingInProgress = g.Count(x => x.scope.Category == "Training" && (x.task.Status <3 && x.task.Status >= 0)),
                        TrainingCompleted = g.Count(x => x.scope.Category == "Training" && x.task.Status == 3),

                        OngoingTotal = g.Count(x => x.scope.Category == "Ongoing"),
                        OngoingPending = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status < 0)),
                        OngoingInProgress = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status <3 && x.task.Status >= 0)),
                        OngoingCompleted = g.Count(x => x.scope.Category == "Ongoing" && x.task.Status == 3),

                        ProjectEndTotal = g.Count(x => x.scope.Category == "Project End"),
                        ProjectEndPending = g.Count(x => x.scope.Category == "Project End" && (x.task.Status < 0)),
                        ProjectEndInProgress = g.Count(x => x.scope.Category == "Project End" && (x.task.Status < 3 && x.task.Status >= 0)),
                        ProjectEndCompleted = g.Count(x => x.scope.Category == "Project End" && x.task.Status == 3)
                    }).ToList();
                foreach (var k in result)
                {
                    int total = k.StartupTotal + k.OngoingTotal + k.TrainingTotal + k.ProjectEndTotal;
                    int completed = k.StartupCompleted + k.OngoingCompleted + k.TrainingCompleted + k.ProjectEndCompleted;
                    int inProgress = k.StartupInProgress + k.OngoingInProgress + k.TrainingInProgress + k.ProjectEndInProgress;
                    int pending = k.StartupPending + k.OngoingPending + k.TrainingPending + k.ProjectEndPending;
                    if (total == completed)
                    { k.OverallStatus = "Completed"; }
                    if (total > completed && completed != 0)
                    { k.OverallStatus = "In Progress"; }
                    if (total > completed && completed == 0)
                    { k.OverallStatus = "Action Awaited"; }
                }
                return result;
            }
            else
            {
                var result = (
                     from task in _context.Ncbocws
                     join scope in _context.BocwScopes on task.ScopeId equals scope.ScopeId
                     join project in _context.Ncmlocs on task.Lcode equals project.Lcode
                     join mapp in _context.Ncumaps on project.Lcode equals mapp.Lcode
                     where mapp.Uno == uno
                     where scope.ScopeActive == 1 && project.Lactive == 1
                     group new { task, scope, project } by new { task.Lcode, project.Lname } into g
                     select new ProjectDashboardStatus
                     {

                         ProjectId = g.Key.Lcode,
                         ProjectName = g.Key.Lname,
                         Startdate = (from k in _context.Ncmlocbos where k.Lcode == g.Key.Lcode select k.ProjectStartDateEst).FirstOrDefault(),
                         Enddate = (from k in _context.Ncmlocbos where k.Lcode == g.Key.Lcode select k.ProjectEndDateEst).FirstOrDefault(),
                         Oid = g.FirstOrDefault().project.Oid,
                         StartupTotal = g.Count(x => x.scope.Category == "Startup"),
                         StartupPending = g.Count(x => x.scope.Category == "Startup" && (x.task.Status < 0)),
                         StartupInProgress = g.Count(x => x.scope.Category == "Startup" && (x.task.Status < 3 && x.task.Status >= 0)),
                         StartupCompleted = g.Count(x => x.scope.Category == "Startup" && x.task.Status == 3),

                         TrainingTotal = g.Count(x => x.scope.Category == "Training"),
                         TrainingPending = g.Count(x => x.scope.Category == "Training" && (x.task.Status < 0)),
                         TrainingInProgress = g.Count(x => x.scope.Category == "Training" && (x.task.Status < 3 && x.task.Status >= 0)),
                         TrainingCompleted = g.Count(x => x.scope.Category == "Training" && x.task.Status == 3),

                         OngoingTotal = g.Count(x => x.scope.Category == "Ongoing"),
                         OngoingPending = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status < 0)),
                         OngoingInProgress = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status < 3 && x.task.Status >= 0)),
                         OngoingCompleted = g.Count(x => x.scope.Category == "Ongoing" && x.task.Status == 3),

                         ProjectEndTotal = g.Count(x => x.scope.Category == "Project End"),
                         ProjectEndPending = g.Count(x => x.scope.Category == "Project End" && (x.task.Status < 0)),
                         ProjectEndInProgress = g.Count(x => x.scope.Category == "Project End" && (x.task.Status < 3 && x.task.Status >= 0)),
                         ProjectEndCompleted = g.Count(x => x.scope.Category == "Project End" && x.task.Status == 3)
                     }).ToList();
                foreach (var k in result)
                {
                    int total = k.StartupTotal + k.OngoingTotal + k.TrainingTotal + k.ProjectEndTotal;
                    int completed = k.StartupCompleted + k.OngoingCompleted + k.TrainingCompleted + k.ProjectEndCompleted;
                    int inProgress = k.StartupInProgress + k.OngoingInProgress + k.TrainingInProgress + k.ProjectEndInProgress;
                    int pending = k.StartupPending + k.OngoingPending + k.TrainingPending + k.ProjectEndPending;
                    if (total == completed)
                    { k.OverallStatus = "Completed"; }
                    if (total > completed && completed != 0)
                    { k.OverallStatus = "In Progress"; }
                    if (total > completed && completed == 0)
                    { k.OverallStatus = "Action Awaited"; }
                }
                return result;
            }
                
        }

    }
}
