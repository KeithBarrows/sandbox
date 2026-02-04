namespace Touch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                Console.WriteLine("Provide [FileName] [+/-N] [T/f] [T/f] [T/f]");
                Console.WriteLine("Touch [filename] [+/-days] [creation] [lastaccess] [lastwrite]");
                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                return;
            }

            var file = args[0];
            var days = 0;

            if (args.Length > 1)
                int.TryParse(args[1], out days);
            if (args.Length > 2 && args[2].ToUpper() == "T")
                File.SetCreationTime(file, DateTime.Now.AddDays(days));
            if (args.Length > 3 && args[3].ToUpper() == "T")
                File.SetLastAccessTime(file, DateTime.Now.AddDays(days));
            if (args.Length > 4 && args[4].ToUpper() == "T")
                File.SetLastWriteTime(file, DateTime.Now.AddDays(days));
        }
    }
}