using HotChocolate.Types;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Description("ECCSA Queries");

        }
    }
}
