using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using ShippingMvp.Books;

namespace ShippingMvp;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ShippingMvpBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ShippingMvpCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}
