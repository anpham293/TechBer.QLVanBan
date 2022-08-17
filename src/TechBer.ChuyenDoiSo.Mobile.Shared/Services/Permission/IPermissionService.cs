namespace TechBer.ChuyenDoiSo.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}