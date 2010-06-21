//-----------------------------------------------------------------------
// <copyright file="ContactMultiTapEventArgs.cs" company="Charlie Robbins">
//     Copyright (c) Charlie Robbins.  All rights reserved.
// </copyright>
// <license>
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </license>
// <summary>Contains the DeleteMe class.</summary>
//-----------------------------------------------------------------------

namespace WpfSurfaceSamples.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Surface.Presentation;
    using System.Windows;
    using System.Windows.Input;

    public delegate void ContactMultiTapEventHandler(object sender, ContactMultiTapEventArgs args);

    public class ContactMultiTapEventArgs : RoutedEventArgs
    {
        public ContactMultiTapEventArgs(RoutedEvent routedEvent, ContactEventArgs args, int multiTapCount)
        {
            this.RoutedEvent = routedEvent;
            this.ContactEventArgs = args;
            this.MultiTapCount = multiTapCount;
            this.Handled = args.Handled;
            this.Source = args.Source;
        }

        private ContactEventArgs ContactEventArgs
        {
            get;
            set;
        }

        public int MultiTapCount
        {
            get;
            private set;
        }

        public Point GetPosition(UIElement relativeTo)
        {
            return this.ContactEventArgs.GetPosition(relativeTo);
        }

        public object ContactOriginalSource
        {
            get { return this.ContactEventArgs.OriginalSource; }
        }

        public Contact Contact
        {
            get { return this.ContactEventArgs.Contact; }
        }

        public InputDevice Device
        {
            get { return this.ContactEventArgs.Device; }
        }
    }
}
