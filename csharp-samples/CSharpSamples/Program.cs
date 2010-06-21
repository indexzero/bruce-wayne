
namespace CSharpSamples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// The main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for Program.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            DynamicDispatcher dispatcher = new DynamicDispatcher();

            dispatcher.Register<string>("Foo", str => new DispatchResult() { Result = str });
            dispatcher.Register<int>("Foo", num => new DispatchResult() { Result = num });

            DispatchResult result = dispatcher.Dispatch<string>("Foo", "Bar");

            Debug.WriteLine(dispatcher.Dispatch<string>("Foo", "Bar").Result.ToString());
            Debug.WriteLine(dispatcher.Dispatch<int>("Foo", 123).Result.ToString());

            int x = 0;
            x++;
        }
    }
}
