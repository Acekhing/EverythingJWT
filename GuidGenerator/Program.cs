// See https://aka.ms/new-console-template for more information
public class Program
{
    static void Main(string[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine(Guid.NewGuid());
        }
    }
}
