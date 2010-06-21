/**************************************************************
 * Copyright (c) 2009 Charlie Robbins
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
**************************************************************/

namespace CSharpSamples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Dispatches strongly typed actions based on a named key.
    /// </summary>
    public class DynamicDispatcher
    {
        /// <summary>
        /// The lookup registry for dispatching operations by name and type
        /// </summary>
        private Dictionary<Type, Dictionary<string, DispatchRegistration<object>>> dispatchRegistry = new Dictionary<Type, Dictionary<string, DispatchRegistration<object>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDispatcher"/> class.
        /// </summary>
        public DynamicDispatcher()
        {
        }

        /// <summary>
        /// Registers the specified name with a trivial null action.
        /// </summary>
        /// <param name="name">The name to register for dispatch.</param>
        /// <returns>The registration now associated with name.</returns>
        public DispatchRegistration<object> Register(string name)
        {
            return this.Register<object>(name, obj => null); 
        }

        /// <summary>
        /// Registers the specified name with the specificed dispatch action.
        /// </summary>
        /// <typeparam name="T">The type of the payload of the dispatch action.</typeparam>
        /// <param name="name">The name to register.</param>
        /// <param name="dispatchAction">The dispatch action.</param>
        /// <returns>The registration now associated with name.</returns>
        public DispatchRegistration<object> Register<T>(string name, Func<T, DispatchResult> dispatchAction)
        {
            // Check to see if we've encountered typeof(T) before
            Dictionary<string, DispatchRegistration<object>> typeOfTRegistry;
            if (!this.dispatchRegistry.TryGetValue(typeof(T), out typeOfTRegistry))
            {
                typeOfTRegistry = new Dictionary<string, DispatchRegistration<object>>();
                this.dispatchRegistry[typeof(T)] = typeOfTRegistry;
            }

            DispatchRegistration<object> registration = new DispatchRegistration<object>()
            {
                Name = name,
                DispatchAction = obj => dispatchAction((T)obj)
            };

            typeOfTRegistry[name] = registration;
            return registration;
        }

        /// <summary>
        /// Attempts to dispatch name and T to a registered action.
        /// </summary>
        /// <typeparam name="T">The payload of the </typeparam>
        /// <param name="name">The name to dispatch on.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The result of the dispatch</returns>
        public DispatchResult Dispatch<T>(string name, T parameter)
        {
            Dictionary<string, DispatchRegistration<object>> typeOfTRegistry;
            if (this.dispatchRegistry.TryGetValue(typeof(T), out typeOfTRegistry))
            {
                if (typeOfTRegistry.ContainsKey(name))
                {
                    return typeOfTRegistry[name].DispatchAction(parameter);
                }
            }

            return null;
        }
    }
}
