using SIS.MvcFramework;
using System;

namespace Panda.Web
{
    public class Program
    {
        public static void Main()
        {
            WebHost.Start(new StartUp());
        }
    }
}
