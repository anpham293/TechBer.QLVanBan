using System.Data.SqlClient;
using Shouldly;
using Xunit;

namespace TechBer.ChuyenDoiSo.Tests.General
{
    // ReSharper disable once InconsistentNaming
    public class ConnectionString_Tests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Test()
        {
            var csb = new SqlConnectionStringBuilder("Server=localhost; Database=ChuyenDoiSo; Trusted_Connection=True;");
            csb["Database"].ShouldBe("ChuyenDoiSo");
        }
    }
}
