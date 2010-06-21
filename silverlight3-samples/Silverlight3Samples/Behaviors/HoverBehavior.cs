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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity;

namespace Silverlight3Samples.Behaviors
{
    public class HoverBehavior : Behavior<UIElement>
    {
        #region HoverProperty

        public static readonly DependencyProperty HoverProperty = DependencyProperty.Register(
            "Hover",
            typeof(Brush),
            typeof(HoverBehavior),
            new PropertyMetadata(null));

        public Brush Hover
        {
            get { return (Brush)GetValue(HoverProperty); }
            set { SetValue(HoverProperty, value); }
        }

        #endregion

        private Brush NotHover
        {
            get;
            set; 
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseEnter += SetHoverBackground;
            this.AssociatedObject.MouseLeave += SetNotHoverBackground;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.MouseEnter -= SetHoverBackground;
            this.AssociatedObject.MouseLeave -= SetNotHoverBackground;
        }

        private void SetHoverBackground(object sender, MouseEventArgs args)
        {
            Border border = sender as Border;
            if (border != null)
            {
                this.NotHover = border.Background;
                border.Background = this.Hover;
            }
        }

        private void SetNotHoverBackground(object sender, MouseEventArgs args)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.Background = this.NotHover;
            }
        }

        
    }
}
