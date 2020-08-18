using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityDTOFactory.Mapper
{
    /// <summary>
    /// Contains metadata used for mapping entity objects to dtos
    /// </summary>
    public class ObjectMap
    {
        public ObjectMap(Type type)
        {
            Type = type;
            CompanionType = Helpers.TypeNameResolver.ConvertType(type);

            // type could not resolve a companion type
            if (CompanionType.Name == Type.Name)
                return;

            Properties = type.GetProperties();
            Constructors = type.GetConstructors();
            Parameters = SelectIdealConstructor().GetParameters();
            ConstructorArguments = GenerateConstructorArguments();
            MapParameterNamesToPropertyNames();
            GetPropertiesExcludedFromConstructor();

        }

        public Type Type { get; set; }

        // the object this type maps to (e.g. Entity maps to DTO)
        public Type CompanionType { get; set; }
        public PropertyInfo[] Properties { get; set; }
        public ConstructorInfo[] Constructors { get; set; }
        public ParameterInfo[] Parameters { get; set; }

        // default values for constructor parameters
        public object[] ConstructorArguments { get; set; }

        // a map tying property names to those of its companion object
        public Dictionary<string, string> PropertyToCompanionProperty { get; set; } = new Dictionary<string, string>();

        // a map tying parameter names to the type's property names
        public Dictionary<string, string> ParameterToProperty { get; set; } = new Dictionary<string, string>();

        // a map tying parameter names to their default values
        public Dictionary<string, object> ParameterToObject { get; set; } = new Dictionary<string, object>();

        // a list of properties excluded from the constructor, that need to be set after object construction
        public IList<string> ExcludedProperties { get; set; } = new List<string>();

        /// <summary>
        /// Selects a constructor that initializes the most properties
        /// </summary>
        public ConstructorInfo SelectIdealConstructor()
        {
            ConstructorInfo idealConstructor = null;
            HashSet<string> propertyNames = Properties.Select(prop => prop.Name.ToLower()).ToHashSet();
            string[] _matches = new string[] { };

            foreach (ConstructorInfo constructor in Constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                HashSet<string> parameterNames = parameters.Select(param => param.Name.ToLower()).ToHashSet();
                string[] matches = parameterNames.Intersect(propertyNames).ToArray();
                if (matches.Length > _matches.Length)
                {
                    _matches = matches;
                    idealConstructor = constructor;
                }
            }

            // there is only a default constructor with no parameters
            if (idealConstructor == null)
            {
                idealConstructor = Constructors.First();
            }

            return idealConstructor;
        }

        /// <summary>
        /// Creates default instances of each parameter in the constructor
        /// </summary>
        /// <returns>Default parameter values</returns>
        public object[] GenerateConstructorArguments()
        {
            IList<object> arguments = new List<object>();

            foreach (ParameterInfo parameter in Parameters)
            {
                Type type = parameter.ParameterType;
                string name = parameter.Name;
                object value = type.IsValueType ? Activator.CreateInstance(type) : null;
                
                arguments.Add(value);

                // map parameter name to default parameter value
                ParameterToObject[name] = value;
            }

            return arguments.ToArray();
        }

        /// <summary>
        /// Creates a map between the object's parameter names and its property names
        /// </summary>
        public void MapParameterNamesToPropertyNames()
        {
            foreach (ParameterInfo parameter in Parameters)
            {
                string parameterName = parameter.Name;
                string parameterNameLowerCase = parameterName.ToLower();

                PropertyInfo match = Properties.Where(prop => prop.Name.ToLower() == parameterNameLowerCase).FirstOrDefault();

                // property exists
                if (match != null)
                {
                    string propertyName = match.Name;
                    ParameterToProperty[parameterName] = propertyName;
                }
                else
                {
                    ParameterToProperty[parameterName] = null;
                }
            }
        }

        /// <summary>
        /// Creats a list of properties that the constructor does not set
        /// </summary>
        public void GetPropertiesExcludedFromConstructor()
        {
            IList<string> propertyNames = Properties.Select(prop => prop.Name).ToList();
            IList<string> propertyNamesLowerCase = propertyNames.Select(prop => prop.ToLower()).ToList();

            if (Parameters.Length == 0)
            {
                ExcludedProperties = propertyNames;
                return;
            }

            foreach (ParameterInfo parameter in Parameters)
            {
                string parameterName = parameter.Name;
                string parameterNameLowerCase = parameterName.ToLower();

                // the property is not set via the constructor
                if (!propertyNamesLowerCase.Contains(parameterNameLowerCase))
                {
                    string propertyName = ParameterToProperty[parameterName];
                    ExcludedProperties.Add(propertyName);
                }
            }
        }
    }
}
