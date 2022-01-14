using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Shared.Utilities.Results.ComplexTypes;

namespace BlogApp.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        ResultStatus ResultStatus { get; }
        string Message { get; }
        Exception Exception { get; }
    }
}