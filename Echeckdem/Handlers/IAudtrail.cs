namespace Echeckdem.Handlers
{
    public interface IAudtrail
    {
        Task<bool> AddAuditTrailAsync(string userId, string tableName, string details);
    }
}
