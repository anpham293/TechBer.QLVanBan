using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TechBer.ChuyenDoiSo.Configuration;
using TechBer.ChuyenDoiSo.Web;

namespace TechBer.ChuyenDoiSo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ChuyenDoiSoDbContextFactory : IDesignTimeDbContextFactory<ChuyenDoiSoDbContext>
    {
        public ChuyenDoiSoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ChuyenDoiSoDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            ChuyenDoiSoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ChuyenDoiSoConsts.ConnectionStringName));

            return new ChuyenDoiSoDbContext(builder.Options);
        }
    }
}