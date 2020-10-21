using System;

namespace Radeoh.Support
{
    public static class ConsoleDumper
    {
        public static void Json(this object obj)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}