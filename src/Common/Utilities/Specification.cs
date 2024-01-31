using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TesteBackend.Common.Utilities;

public abstract class Specification<T>
{
    public List<Expression<Func<T, bool>>> Criteria { get; } = new List<Expression<Func<T, bool>>>();

    protected virtual void Add(Expression<Func<T, bool>> predicate)
    {
        Criteria.Add(predicate);
    }
}