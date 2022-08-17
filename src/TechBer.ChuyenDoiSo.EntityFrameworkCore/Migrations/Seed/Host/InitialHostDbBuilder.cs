using TechBer.ChuyenDoiSo.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ChuyenDoiSoDbContext _context;

        public InitialHostDbBuilder(ChuyenDoiSoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
