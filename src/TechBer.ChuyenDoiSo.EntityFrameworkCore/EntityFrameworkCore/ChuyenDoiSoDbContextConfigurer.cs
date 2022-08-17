using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.EntityFrameworkCore
{
    public static class ChuyenDoiSoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ChuyenDoiSoDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ChuyenDoiSoDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}