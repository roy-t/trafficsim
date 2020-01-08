namespace TrafficSim
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new GameLoop())
            {
                game.Run();
            }
        }
    }
}
