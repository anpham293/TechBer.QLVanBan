using Abp.Dependency;

namespace TechBer.ChuyenDoiSo.Storage
{
    public interface ITempFileCacheManager: ITransientDependency
    {
        void SetFile(string token, byte[] content);

        byte[] GetFile(string token);
    }
}
