using System.Collections.Concurrent;
using App.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Infrastructure.Configuration.DataAccess;

public class StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
    : ValueConverterSelector(dependencies)
{
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters = new();

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
        {
            var isTypedIdValue = typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType);
            if (isTypedIdValue)
            {
                var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType!);

                yield return _converters.GetOrAdd((underlyingModelType, typeof(Guid))!, _ =>
                {
                    return new ValueConverterInfo(
                        modelClrType: modelClrType,
                        providerClrType: typeof(Guid),
                        factory: valueConverterInfo => (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints)!);
                });
            }
        }
    }

    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}
