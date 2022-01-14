using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Shared.Utilities.Results.Abstract
{
    public interface IDataResult<out T>:IResult
    {
        T Data { get; }
    }
}