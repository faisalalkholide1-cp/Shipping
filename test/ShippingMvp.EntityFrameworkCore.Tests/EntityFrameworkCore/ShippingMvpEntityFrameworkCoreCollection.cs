using Xunit;

namespace ShippingMvp.EntityFrameworkCore;

[CollectionDefinition(ShippingMvpTestConsts.CollectionDefinitionName)]
public class ShippingMvpEntityFrameworkCoreCollection : ICollectionFixture<ShippingMvpEntityFrameworkCoreFixture>
{

}
