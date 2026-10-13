using UnitConversion.Api.Data;
using UnitConversion.Api.Models;

namespace UnitConversion.Api.Services;

public sealed class ConversionService
{
    private static readonly IReadOnlyDictionary<string, UnitDefinition>
        Units = new Dictionary<string, UnitDefinition>(
            StringComparer.OrdinalIgnoreCase)
        {
            // Length: base unit is meter
            ["m"] = new("Length", 1),
            ["km"] = new("Length", 1000),
            ["cm"] = new("Length", 0.01),
            ["mm"] = new("Length", 0.001),
            ["ft"] = new("Length", 0.3048),
            ["in"] = new("Length", 0.0254),
            ["mi"] = new("Length", 1609.344),

            // Mass: base unit is kilogram
            ["kg"] = new("Mass", 1),
            ["g"] = new("Mass", 0.001),
            ["mg"] = new("Mass", 0.000001),
            ["lb"] = new("Mass", 0.45359237),
            ["oz"] = new("Mass", 0.028349523125),

            // Temperature: base unit is Celsius
            ["c"] = new("Temperature", 1),
            ["f"] = new("Temperature", 5.0 / 9.0, -32.0 * 5.0 / 9.0),
            ["k"] = new("Temperature", 1, -273.15)
        };

    public ConversionResponse Convert(ConversionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!double.IsFinite(request.Value))
        {
            throw new ArgumentException(
                "Value must be a finite number.");
        }

        var fromUnit = request.FromUnit?.Trim() ?? string.Empty;
        var toUnit = request.ToUnit?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(fromUnit) ||
            string.IsNullOrWhiteSpace(toUnit))
        {
            throw new ArgumentException(
                "FromUnit and ToUnit are required.");
        }

        if (!Units.TryGetValue(fromUnit, out var source))
        {
            throw new ArgumentException(
                $"Unsupported source unit: '{fromUnit}'.");
        }

        if (!Units.TryGetValue(toUnit, out var target))
        {
            throw new ArgumentException(
                $"Unsupported target unit: '{toUnit}'.");
        }

        if (!string.Equals(
                source.Category,
                target.Category,
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Source and target units must belong to the same category.");
        }

        // Convert the input into the category's base unit.
        var baseValue = request.Value * source.Factor
                        + source.Offset;

        // Convert from the base unit into the destination unit.
        var convertedValue =
            (baseValue - target.Offset) / target.Factor;

        if (!double.IsFinite(convertedValue))
        {
            throw new ArgumentException(
                "The conversion result is outside the supported numeric range.");
        }

        return new ConversionResponse
        {
            OriginalValue = request.Value,
            FromUnit = fromUnit,
            ConvertedValue = convertedValue,
            ToUnit = toUnit,
            Category = source.Category
        };
    }
}