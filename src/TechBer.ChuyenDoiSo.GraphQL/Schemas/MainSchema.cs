using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using TechBer.ChuyenDoiSo.Queries.Container;

namespace TechBer.ChuyenDoiSo.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}