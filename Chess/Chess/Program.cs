namespace Chess
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (Chess game = new Chess())
            {
                game.Run();
            }
        }
    }
}

