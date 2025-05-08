namespace Echeckdem.Handlers
{
    public interface IAudtrail
    {
        Task<bool> AddAuditTrailAsync(string userId, string origin, string tableName, string recordId, string oldValue, string newValue);
    }
}
