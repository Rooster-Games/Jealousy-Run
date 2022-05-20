using System;
using System.Linq;


public class AssemblyInstanceCreator
{
    private readonly Type _type;
    private readonly Type[] _allTypes;

    public AssemblyInstanceCreator(Type type)
    {
        _type = type;
        _allTypes = GetAllTypes();
    }

    private Type[] FindTypes(Func<Type, bool> condition)
    {
        return _allTypes.Where(x => condition(x)).ToArray();
    }

    private Type FindSingle(Func<Type, bool> condition)
    {
        return _allTypes.Where(x => condition(x)).FirstOrDefault();
    }

    private Type FindSingleImpOfImplementation(Type type)
    {
        return FindSingle(x => type.IsAssignableFrom(x) && !x.IsInterface);
    }

    private Type[] FindAllImpOfImplementation(Type type)
    {
        return FindTypes(x => type.IsAssignableFrom(x) && !x.IsInterface);
    }

    public object FindAndCreateInstanceImpOfInterface(Type type)
    {
        var t = FindSingleImpOfImplementation(type);
        return Activator.CreateInstance(t);
    }

    public object FindAndCreateInstanceImpOfInterfaceByName(Type type, string name)
    {

        var types = FindAllImpOfImplementation(type);
        foreach (var t in types)
        {
            if (t.Name.Contains(name))
                return Activator.CreateInstance(t);
        }

        throw new System.Exception("=== Buradan Null Gelmemeli ===");
    }

    private Type[] GetAllTypes()
    {
        return _type.Assembly.GetTypes();
    }
}