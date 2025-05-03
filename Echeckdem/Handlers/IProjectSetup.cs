namespace Echeckdem.Handlers
{
    public interface IProjectSetup
    {
        //PE One TIme
        Task<string> CLRARCNw(string lcode,DateTime projectStartDateEst);

        Task<string> CLRA6B(string lcode, DateTime projectStartDateEst);
        Task<string> BOCWRCNw(string lcode, DateTime projectStartDateEst);

        Task<string> BOCWFr4(string lcode, DateTime projectStartDateEst);
        Task<string> CLRARCSr(string lcode, DateTime projectEndDateEst);

        Task<string> BOCWRCSr(string lcode, DateTime projectEndDateEst);

        Task<string> OnSiteDisplay(string lcode, DateTime projectEndDateEst);

        //Training
        Task<string> TrainingSiteCompliance(string lcode, DateTime projectEndDateEst);
        Task<string> TrainingVendorCompliance(string lcode, DateTime projectEndDateEst);

        Task<string> MonthlyVendorAudit(string lcode, DateTime projectEndDateEst);

        //Ongoing Returns, Cess Submission

        Task<string> BOCWFr1(string lcode, DateTime projectStartDateEst, DateTime projectEndDateEst);
        Task<string> BOCWRet(string lcode, DateTime projectStartDateEst, DateTime projectEndDateEst);

        Task<string> CLRARcRet(string lcode, DateTime projectStartDateEst, DateTime projectEndDateEst);

        Task<string> CLRALicRet(string lcode, DateTime projectStartDateEst, DateTime projectEndDateEst);

        //GC One Time
        Task<string> GcCLRALicNw(string lcode, DateTime projectStartDateEst);
        Task<string> GcCLRARCNw(string lcode, DateTime projectStartDateEst);
        Task<string> GcBOCWRCNw(string lcode, DateTime projectStartDateEst);
        Task<string> GcCLRARCSr(string lcode, DateTime projectEndDateEst);

        Task<string> GcBOCWRCSr(string lcode, DateTime projectEndDateEst);
        Task<string> GcCLRA6B(string lcode, DateTime projectStartDateEst);

        Task<string> GcBOCWFr(string lcode, DateTime projectStartDateEst);
    }
}
