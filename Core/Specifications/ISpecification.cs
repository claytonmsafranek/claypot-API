﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // this is what the 'where' criteria is in the linq query
        Expression<Func<T, bool>> Criteriea { get; }
        // this is the 'includes' for attaching objects in linq query
        List<Expression<Func<T, object>>> Includes { get; }
    }
}