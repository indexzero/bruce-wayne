//-----------------------------------------------------------------------
// <copyright file="ContactMultiTap.cs" company="Charlie Robbins">
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
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Surface.Presentation;

    /// <summary>
    /// A set of attached events for double and triple contact taps in the Surface SDK
    /// </summary>
    public class ContactMultiTap : UIElement
    {
        /// <summary>
        /// Attached event for double tap
        /// </summary>
        public static readonly RoutedEvent ContactMultiTapGestureEvent = EventManager.RegisterRoutedEvent(
            "ContactMultiTapGesture",
            RoutingStrategy.Bubble,
            typeof(ContactMultiTapEventHandler),
            typeof(ContactMultiTap));
        
        /// <summary>
        /// Backing store for the multi tap interval property
        /// </summary>
        public static readonly DependencyProperty MultiTapIntervalProperty = DependencyProperty.RegisterAttached(
            "MultiTapInterval",
            typeof(TimeSpan),
            typeof(ContactMultiTap),
            new FrameworkPropertyMetadata(TimeSpan.FromSeconds(0), OnMultiTapIntervalPropertyChanged));

        /// <summary>
        /// Backing store for the tap count attached property
        /// </summary>
        private static readonly DependencyProperty TapCountProperty = DependencyProperty.RegisterAttached(
            "TapCount",
            typeof(int),
            typeof(ContactMultiTap),
            new FrameworkPropertyMetadata(0));

        /// <summary>
        /// A mapping of weak reference to point for location aware tap counting.
        /// </summary>
        private static Dictionary<WeakReference, Point> multiTapLocations = new Dictionary<WeakReference, Point>();
        
        /// <summary>
        /// A mapping of weak reference to timer for UIElement tap counting.
        /// </summary>
        private static Dictionary<WeakReference, DispatcherTimer> multiTapTimers = new Dictionary<WeakReference, DispatcherTimer>();
        
        /// <summary>
        /// The pixel tolerance between taps. If an subsequent tap is within this
        /// amount (both X and Y) it will be considered the next tap in a set  
        /// of multi tap events.
        /// </summary>
        private static Point interTapTolerance = new Point(20, 20);

        /// <summary>
        /// Adds a contact multi tap handler to a UIElement.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="handler">The handler.</param>
        public static void AddContactMultiTapGestureHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            UIElement element = dependencyObject as UIElement;
            if (element != null)
            {
                element.AddHandler(ContactMultiTapGestureEvent, handler);
            }
        }

        /// <summary>
        /// Gets the multi tap interval for a specific dependency object
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <returns>The value for the MultiTapInterval property, if any.</returns>
        public static TimeSpan GetMultiTapInterval(DependencyObject obj)
        {
            return (TimeSpan)obj.GetValue(MultiTapIntervalProperty);
        }

        /// <summary>
        /// Removes a ContactMultiTapGesture event handler.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="handler">The event handler.</param>
        public static void RemoveContactMultiTapGestureHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
        {
            UIElement element = dependencyObject as UIElement;
            if (element != null)
            {
                element.RemoveHandler(ContactMultiTapGestureEvent, handler);
            }
        }

        /// <summary>
        /// Sets the multi tap interval.
        /// </summary>
        /// <param name="obj">The objet to set the value on.</param>
        /// <param name="value">The value.</param>
        public static void SetMultiTapInterval(DependencyObject obj, TimeSpan value)
        {
            obj.SetValue(MultiTapIntervalProperty, value);
        }

        /// <summary>
        /// Gets the tap count.
        /// </summary>
        /// <param name="obj">The object to get the value from.</param>
        /// <returns>The tap count associated with the object</returns>
        private static int GetTapCount(DependencyObject obj)
        {
            return (int)obj.GetValue(TapCountProperty);
        }

        /// <summary>
        /// Increments the tap count on the sender and raises the 
        /// ContactMultiTapGestureEvent if appropriate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Microsoft.Surface.Presentation.ContactEventArgs"/> instance containing the event data.</param>
        private static void OnContactTapGesture(object sender, ContactEventArgs args)
        {
            // Attempt to find the weak reference for the element
            WeakReference elementReference = multiTapTimers
                .Where(pair => object.ReferenceEquals(pair.Key.Target, sender))
                .Select(pair => pair.Key)
                .FirstOrDefault();

            // Stop the timer so we don't reset the tap count
            multiTapTimers[elementReference].Stop();

            if (!multiTapLocations.ContainsKey(elementReference))
            {
                multiTapLocations.Add(elementReference, args.GetPosition(sender as UIElement));
                
                // Increment the tap count on the element
                int tapCount = GetTapCount(sender as DependencyObject);
                SetTapCount(sender as DependencyObject, ++tapCount);
                
                // Start the timer to reset the tap count if we don't 
                // get a tap in the appropriate time
                multiTapTimers[elementReference].Start();
                return;
            }

            // Get the last location so that we can ensure that this tap is 
            // from the same location
            Point lastLocation = multiTapLocations[elementReference];
            Point newLocation = args.GetPosition(sender as UIElement);
            Vector locationDelta = new Vector(0, 0);
            locationDelta = newLocation - lastLocation;
            locationDelta = new Vector(Math.Abs(locationDelta.X), Math.Abs(locationDelta.Y));

            if (locationDelta.X <= interTapTolerance.X && locationDelta.Y <= interTapTolerance.Y)
            {
                // Increment the tap count on the sender.
                int tapCount = GetTapCount(sender as DependencyObject);
                SetTapCount(sender as DependencyObject, ++tapCount);

                // If the tap count is more than two, raise the double tap event
                if (tapCount >= 2)
                {
                    ((UIElement)sender).RaiseEvent(new ContactMultiTapEventArgs(
                        ContactMultiTapGestureEvent,
                        args,
                        tapCount));
                }
            }
            else
            {
                // If the location is outside the threshold, reset the tap count
                ((DependencyObject)sender).SetValue(TapCountProperty, 0);
            }

            // Start the timer to reset the tap count if we don't get a tap in the appropriate time
            multiTapTimers[elementReference].Start();
        }

        /// <summary>
        /// Called when MultiTapIntervalProperty changes; updates multi-tap listening events for this element
        /// </summary>
        /// <param name="obj">The object whose property changes.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMultiTapIntervalPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // Attempt to find the weak reference for the element
            WeakReference elementReference = multiTapTimers
                .Where(pair => object.ReferenceEquals(pair.Key.Target, obj))
                .Select(pair => pair.Key)
                .FirstOrDefault();

            // If the element doesn't exist
            if (elementReference == null)
            {
                // Create a weak reference to the element
                elementReference = new WeakReference(obj);

                // Create a new timer for counting taps on the element
                DispatcherTimer elementTimer = new DispatcherTimer();
                elementTimer.Interval = (TimeSpan)args.NewValue;
                elementTimer.Tick += OnMultiTapTimerTick;
                multiTapTimers.Add(elementReference, elementTimer);

                // Listen for single tap events for multi tap counting
                Contacts.AddContactTapGestureHandler(obj, OnContactTapGesture);
            }
        }

        /// <summary>
        /// Called when a multi-tap expiration time has passed; expires the multi-tap gesture if nessary
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void OnMultiTapTimerTick(object sender, EventArgs args)
        {
            // Find the weak reference to the element
            WeakReference elementReference = multiTapTimers
                .Where(pair => object.ReferenceEquals(pair.Value, sender))
                .Select(pair => pair.Key)
                .FirstOrDefault();

            // Remove the last location of the element reference since
            // we are no longer tracking it
            multiTapLocations.Remove(elementReference);

            // Reset the tap count since we are no longer tracking it
            ((DependencyObject)elementReference.Target).SetValue(TapCountProperty, 0);
        }

        /// <summary>
        /// Sets the tap count.
        /// </summary>
        /// <param name="obj">The object to set the value on.</param>
        /// <param name="value">The value.</param>
        private static void SetTapCount(DependencyObject obj, int value)
        {
            obj.SetValue(TapCountProperty, value);
        }
    }
}
