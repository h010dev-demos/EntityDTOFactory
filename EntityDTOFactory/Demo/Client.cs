using EntityDTOFactory.Factory;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EntityDTOFactory.Demo
{
    public class Client
    {
        IList<Animal> entities = new List<Animal>();
        IList<AnimalDTO> dtos = new List<AnimalDTO>();

        public Client()
        {
        }

        public void MakeSomeEntityObjects()
        {
            Animal pet1 = new Pigeon("Pidgey", 100, false, 0, SpecialSkill.FETCH);
            Animal pet2 = new Rattlesnake("Ekans", 20.0, true, SpecialSkill.BITE);
            Animal pet3 = new Hawk("Fearow", 150, false, 0, SpecialSkill.FLY, (Bird)pet1);

            entities.Add(pet1);
            entities.Add(pet2);
            entities.Add(pet3);
        }

        public void ConvertEntitiesToDTOs()
        {
            Producer<Animal, AnimalDTO> dtoProducer = new Producer<Animal, AnimalDTO>();
            dtos = dtoProducer.ConvertCollection(entities);
        }

        public void ConvertDTOsToEntities()
        {
            Producer<AnimalDTO, Animal> entityProducer = new Producer<AnimalDTO, Animal>();
            entities = entityProducer.ConvertCollection(dtos);
        }

        public void PrintResults()
        {
            Console.WriteLine("Incoming entities...\n");

            foreach (Animal entity in entities)
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();

                Console.WriteLine($"\n{entity.Name}\n");

                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine($"\t{property.Name}: {property.GetValue(entity)}");
                }
            }

            Console.WriteLine("\nIncoming dtos...\n");

            foreach (AnimalDTO dto in dtos)
            {
                PropertyInfo[] properties = dto.GetType().GetProperties();

                Console.WriteLine($"\n{dto}\n");

                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine($"\t{property.Name}: {property.GetValue(dto)}");
                }
            }

            Console.ReadLine();
        }
    }
}
