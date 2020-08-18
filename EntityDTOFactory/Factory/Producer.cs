using EntityDTOFactory.Mapper;
using System;
using System.Collections.Generic;

namespace EntityDTOFactory.Factory
{
    public class Producer<TSource, TDestination>
    {
        private readonly Factory<TDestination> factory = new Factory<TDestination>();
        private readonly Mapper<TSource, TDestination> mapper = new Mapper<TSource, TDestination>();

        public IList<TDestination> ConvertCollection(IList<TSource> collection)
        {
            IList<TDestination> result = new List<TDestination>();
            foreach (TSource item in collection)
            {
                var convertedItem = Produce(item);
                if (convertedItem != null)
                    result.Add(convertedItem);
            }

            return result;
        }

        public TDestination Produce(TSource source)
        {
            Type sourceType = source.GetType();
            Type modelType = mapper.objectMaps[sourceType].CompanionType;

            object[] args = mapper.GenerateConstructorArguments(source);

            // create an instance of the object and update properties
            TDestination destination = factory.CreateObject(modelType.Name, args);
            destination = mapper.Map(source, destination);

            return destination;
        }
    }
}
