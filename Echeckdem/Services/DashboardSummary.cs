using Echeckdem.Handlers;
using Echeckdem.Models;
using Echeckdem.ViewModel;

namespace Echeckdem.Services
{
    public class DashboardSummary: IDashboardSummary
        

    {
        private readonly DbEcheckContext _context;
        public DashboardSummary(DbEcheckContext context)
        {
            _context = context;
        }
        public  List<ProjectDashboardStatus> GetDashboardSummary()
        {
            var result =  (
                from task in _context.Ncbocws
                join scope in _context.BocwScopes on task.ScopeId equals scope.ScopeId
                join project in _context.Ncmlocs on task.Lcode equals project.Lcode
                where scope.ScopeActive == 1
                group new { task, scope, project } by new { task.Lcode, project.Lname } into g
                select new ProjectDashboardStatus
                {
                    ProjectId = g.Key.Lcode,
                    ProjectName = g.Key.Lname,

                    StartupTotal = g.Count(x => x.scope.Category == "Startup"),
                    StartupPending = g.Count(x => x.scope.Category == "Startup" && (x.task.Status == -2 || (x.task.Status >= -1 && x.task.Status < 1))),
                    StartupInProgress = g.Count(x => x.scope.Category == "Startup" && (x.task.Status==1 || x.task.Status == 2)),
                    StartupCompleted = g.Count(x => x.scope.Category == "Startup" && x.task.Status == 3),

                    TrainingTotal = g.Count(x => x.scope.Category == "Training"),
                    TrainingPending = g.Count(x => x.scope.Category == "Training" && (x.task.Status == -2 || (x.task.Status >= -1 && x.task.Status < 1))),
                    TrainingInProgress = g.Count(x => x.scope.Category == "Training" && (x.task.Status == 1 || x.task.Status == 2)),
                    TrainingCompleted = g.Count(x => x.scope.Category == "Training" && x.task.Status == 3),

                    OngoingTotal = g.Count(x => x.scope.Category == "Ongoing"),
                    OngoingPending = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status == -2 || (x.task.Status >= -1 && x.task.Status < 1))),
                    OngoingInProgress = g.Count(x => x.scope.Category == "Ongoing" && (x.task.Status == 1 || x.task.Status == 2)),
                    OngoingCompleted = g.Count(x => x.scope.Category == "Ongoing" && x.task.Status == 3),

                    ProjectEndTotal = g.Count(x => x.scope.Category == "Project End"),
                    ProjectEndPending = g.Count(x => x.scope.Category == "Project End" && (x.task.Status == -2 || (x.task.Status >= -1 && x.task.Status < 1))),
                    ProjectEndInProgress = g.Count(x => x.scope.Category == "Project End" && (x.task.Status == 1 || x.task.Status == 2)),
                    ProjectEndCompleted = g.Count(x => x.scope.Category == "Project End" && x.task.Status == 3)
                }).ToList();

            return result;
        }

    }
}
