namespace UnitConversion.Api.Data;

public sealed record UnitDefinition(
    string Category,
    double Factor,
    double Offset = 0);