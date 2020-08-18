using EntityDTOFactory.Demo;

namespace EntityDTOFactory
{
    class App
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            client.MakeSomeEntityObjects();
            client.ConvertEntitiesToDTOs();
            client.ConvertDTOsToEntities();

            client.PrintResults();
        }
    }
}
