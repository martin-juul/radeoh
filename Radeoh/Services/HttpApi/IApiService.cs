using System;
using Fusillade;

namespace Radeoh.Services.HttpApi
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}