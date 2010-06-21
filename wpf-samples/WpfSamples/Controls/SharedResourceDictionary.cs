/**************************************************************
 * Copyright (c) 2008 Charlie Robbins
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

namespace Lab49.Controls.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// The shared resource dictionary is a specialized resource dictionary
    /// that loads it content only once. If a second instance with the same source
    /// is created, it only merges the resources from the cache.
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary
    {
        /// <summary>
        /// Internal cache of loaded dictionaries.
        /// </summary>
        private static Dictionary<Uri, ResourceDictionary> sharedDictionaries =
            new Dictionary<Uri, ResourceDictionary>();

        /// <summary>
        /// A value indicating whether the application is in designer mode.
        /// </summary>
        private static bool isInDesignerMode;

        /// <summary>
        /// Local member of the source uri
        /// </summary>
        private Uri sourceUri;

        /// <summary>
        /// Initializes static members of the <see cref="SharedResourceDictionary"/> class.
        /// </summary>
        static SharedResourceDictionary()
        {
            isInDesignerMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
        }

        /// <summary>
        /// Gets the internal cache of loaded dictionaries.
        /// </summary>
        public static Dictionary<Uri, ResourceDictionary> SharedDictionaries
        {
            get { return sharedDictionaries; }
        }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get 
            { 
                return this.sourceUri; 
            }

            set
            {
                this.sourceUri = value;

                // Always load the dictionary by default in designer mode.
                if (!sharedDictionaries.ContainsKey(value) || isInDesignerMode)
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    // add it to the cache if we're not in designer mode
                    if (!isInDesignerMode)
                    {
                        sharedDictionaries.Add(value, this);
                    }
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    this.MergedDictionaries.Add(sharedDictionaries[value]);
                }
            }
        }
    }
}
