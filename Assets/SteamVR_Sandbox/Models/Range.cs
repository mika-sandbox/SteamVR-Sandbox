namespace SteamVR_Sandbox.Models
{
    public struct Range<T>
    {
        public T Min { get; set; }

        public T Max { get; set; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}