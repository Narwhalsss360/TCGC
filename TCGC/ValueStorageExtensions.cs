using TruckConnect;

namespace TCGC
{
    public static class ValueStorageExtensions
    {
        public static T ValueOrDefault<T>(this ValueStorage<T> value, T @default) =>
            value.Initialized ? value.Value : @default;
    }
}
