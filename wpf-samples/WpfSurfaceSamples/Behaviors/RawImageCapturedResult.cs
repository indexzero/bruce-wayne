﻿//-----------------------------------------------------------------------
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

    /// <summary>
    /// The data for a raw image capture operation.
    /// </summary>
    public class RawImageCapturedResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageCapturedResult"/> class.
        /// </summary>
        /// <param name="rawImage">The raw image.</param>
        public RawImageCapturedResult(byte[] rawImage)
        {
            this.RawImage = rawImage;
            this.ImageUri = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageCapturedResult"/> class.
        /// </summary>
        /// <param name="imageUri">The image uri.</param>
        public RawImageCapturedResult(string imageUri)
        {
            this.ImageUri = imageUri;
            this.RawImage = null;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the raw image.
        /// </summary>
        /// <value>The raw image.</value>
        public byte[] RawImage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image URI.
        /// </summary>
        /// <value>The image URI.</value>
        public string ImageUri
        {
            get;
            set;
        }

        #endregion Properties 
    }
}
