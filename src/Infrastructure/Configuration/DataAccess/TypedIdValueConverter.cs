using App.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Infrastructure.Configuration.DataAccess;

public class TypedIdValueConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null)
    : ValueConverter<TTypedIdValue, Guid>(id => id.Value, value => Create(value), mappingHints)
    where TTypedIdValue : TypedIdValueBase
{
    private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue ?? throw new InvalidOperationException();
}
