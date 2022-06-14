namespace GrpcLibrary.Protos.CustomTypes;

public partial class DecimalValue
{
    private const decimal NanoFactor = 1_000_000_000;

    public DecimalValue(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator decimal(DecimalValue decimalValue)
    {
        return ToDecimal(decimalValue);
    }

    public static implicit operator DecimalValue(decimal value)
    {
        return FromDecimal(value);
    }

    private static decimal ToDecimal(DecimalValue decimalValue)
    {
        return decimal.Round(decimalValue.Units + decimalValue.Nanos / NanoFactor, 2);
    }

    public static DecimalValue FromDecimal(decimal value)
    {
        var units = decimal.ToInt64(value);
        var nanos = decimal.ToInt32((value - units) * NanoFactor);
        return new DecimalValue(units, nanos);
    }
}