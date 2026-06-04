using ShippingMvp.Samples;
using Xunit;

namespace ShippingMvp.EntityFrameworkCore.Domains;

[Collection(ShippingMvpTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ShippingMvpEntityFrameworkCoreTestModule>
{

}
