namespace Echeckdem.ViewModel
{
    public class ProjectDashboardStatus
    {
        public string Oid { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        
        public DateOnly? Startdate { get; set; }

        public DateOnly? Enddate { get; set; }
        public int StartupTotal { get; set; }

        
        public int StartupPending { get; set; }
        public int StartupInProgress { get; set; }
        public int StartupCompleted { get; set; }
        public int TrainingTotal { get; set; }
        
        public int TrainingPending { get; set; }
        public int TrainingInProgress { get; set; }
        public int TrainingCompleted { get; set; }
        public int OngoingTotal { get; set; }
        public int OngoingPending { get; set; }
        public int OngoingInProgress { get; set; }
        public int OngoingCompleted { get; set; }
        
        public int ProjectEndTotal { get; set; }
        public int ProjectEndPending { get; set; }
        public int ProjectEndInProgress { get; set; }
        public int ProjectEndCompleted { get; set; }
        public string OverallStatus { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
