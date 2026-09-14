using System.Diagnostics.CodeAnalysis;

#if MYSQL56
namespace Quib.MySql56.ORMapper;
#elif MYSQL57
namespace Quib.MySql57.ORMapper;
#elif MYSQL80
namespace Quib.MySql80.ORMapper;
#elif MYSQL84
namespace Quib.MySql84.ORMapper;
#elif POSTGRES18
namespace Quib.Postgres18.ORMapper;
#endif


internal class QueryMappingCache
{

    private readonly Dictionary<ObjectIdentity, List<object>> _store = [];

    public bool TryGet<T>(ObjectIdentity key, [NotNullWhen(true)] out T? obj)
    {
        if (_store.TryGetValue(key, out var store))
        {
            foreach (var item in store)
            {
                if (item is T casted)
                {
                    obj = casted;
                    return true;
                }
            }
        }

        obj = default;
        return false;
    }

    public void Add<T>(ObjectIdentity key, T obj)
        where T : notnull
    {
        if (_store.TryGetValue(key, out var store))
            store.Add(obj);
        else
            _store.Add(key, [obj]);
    }

}