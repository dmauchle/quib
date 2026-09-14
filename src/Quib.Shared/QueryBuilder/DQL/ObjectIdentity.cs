#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
#endif

/// <summary>
/// Equatable object for multi-key identities. 
/// </summary>
internal class ObjectIdentity
{
    private readonly object[] _identity;

    public ObjectIdentity(object[] identity)
    {
        _identity = identity;
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {
        if (obj is ObjectIdentity other)
        {
            if (other._identity.Length != _identity.Length)
                return false;

            for (var i = 0; i < _identity.Length; i++)
            {
                if (!Equals(_identity[i], other._identity[i]))
                    return false;
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        if (_identity.Length == 0)
            return 0;
        
        if (_identity.Length == 1)
            return _identity[0]?.GetHashCode() ?? 0;

        var hash = new HashCode();
        foreach (var item in _identity)
        {
            hash.Add(item);
        }
        return hash.ToHashCode();
    }

}