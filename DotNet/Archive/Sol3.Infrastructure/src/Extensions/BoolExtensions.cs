namespace Sol3.Infrastructure.Extensions
{
    public static class BoolExtensions
    {
        public static int ToInt(this bool src)
        {
            return src ? 1 : 0;
        }
        public static int ToInt(this bool? src)
        {
            return src?.ToInt() ?? 0;
        }
    }
}