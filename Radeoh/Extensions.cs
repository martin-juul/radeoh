using System;

namespace Radeoh
{
    public static class Extensions
    {
        /// <summary>
        /// Dumps object as json to console
        /// </summary>
        /// <param name="o"></param>
        public static void Json(this object o)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(o));
        }
    }
}