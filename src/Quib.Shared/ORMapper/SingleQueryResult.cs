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

public readonly struct SingleQueryResult<T>
{

    private readonly bool _hasResult;
    private readonly T? _result;

    public SingleQueryResult(T result)
    {
        _hasResult = true;
        _result = result;
    }

    public bool HasResult => _hasResult;

    public T Result => _result!;

}