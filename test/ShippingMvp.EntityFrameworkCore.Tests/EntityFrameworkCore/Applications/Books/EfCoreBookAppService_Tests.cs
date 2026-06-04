using ShippingMvp.Books;
using Xunit;

namespace ShippingMvp.EntityFrameworkCore.Applications.Books;

[Collection(ShippingMvpTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<ShippingMvpEntityFrameworkCoreTestModule>
{

}