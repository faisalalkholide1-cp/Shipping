using ShippingMvp.Samples;
using Xunit;

namespace ShippingMvp.EntityFrameworkCore.Applications;

[Collection(ShippingMvpTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ShippingMvpEntityFrameworkCoreTestModule>
{

}
