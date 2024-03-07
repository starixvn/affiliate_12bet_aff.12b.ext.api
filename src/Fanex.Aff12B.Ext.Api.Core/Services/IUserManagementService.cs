namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password);
    }
}
