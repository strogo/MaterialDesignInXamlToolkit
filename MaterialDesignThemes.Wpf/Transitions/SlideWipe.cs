﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaterialDesignThemes.Wpf.Transitions
{
    public class SlideWipe : ITransitionWipe
    {
        public enum SlideDirection { Left, Right, Up, Down }

        private readonly SineEase _sineEase = new SineEase();

        /// <summary>
        /// Direction of the slide wipe
        /// </summary>
        public SlideDirection Direction { get; set; }

        /// <summary>
        /// Duration of the animation
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0.5);

        public void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController)
        {
            if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
            if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            // Set up time points
            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var endKeyTime = KeyTime.FromTimeSpan(Duration);

            // Set up coordinates
            double fromStartX, fromEndX, toStartX, toEndX;
            double fromStartY, fromEndY, toStartY, toEndY;

            if (Direction == SlideDirection.Left)
            {
                fromStartX = 0;
                fromStartY = 0;
                fromEndX = -fromSlide.ActualWidth;
                fromEndY = 0;
                toStartX = toSlide.ActualWidth;
                toStartY = 0;
                toEndX = 0;
                toEndY = 0;
            }
            else if (Direction == SlideDirection.Right)
            {
                fromStartX = 0;
                fromStartY = 0;
                fromEndX = fromSlide.ActualWidth;
                fromEndY = 0;
                toStartX = -toSlide.ActualWidth;
                toStartY = 0;
                toEndX = 0;
                toEndY = 0;
            }
            else if (Direction == SlideDirection.Up)
            {
                fromStartX = 0;
                fromStartY = 0;
                fromEndX = 0;
                fromEndY = -fromSlide.ActualHeight;
                toStartX = 0;
                toStartY = toSlide.ActualHeight;
                toEndX = 0;
                toEndY = 0;
            }
            else // if (Direction == SlideDirection.Down)
            {
                fromStartX = 0;
                fromStartY = 0;
                fromEndX = 0;
                fromEndY = fromSlide.ActualHeight;
                toStartX = 0;
                toStartY = -toSlide.ActualHeight;
                toEndX = 0;
                toEndY = 0;
            }

            // Old
            fromSlide.RenderTransform = new TranslateTransform(fromStartX, fromStartY);
            var fromXAnimation = new DoubleAnimationUsingKeyFrames();
            fromXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartX, zeroKeyTime));
            fromXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndX, endKeyTime, _sineEase));
            var fromYAnimation = new DoubleAnimationUsingKeyFrames();
            fromYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(fromStartY, zeroKeyTime));
            fromYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(fromEndY, endKeyTime, _sineEase));

            // New
            toSlide.RenderTransform = new TranslateTransform(toStartX, toStartY);
            var toXAnimation = new DoubleAnimationUsingKeyFrames();
            toXAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartX, zeroKeyTime));
            toXAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndX, endKeyTime, _sineEase));
            var toYAnimation = new DoubleAnimationUsingKeyFrames();
            toYAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(toStartY, zeroKeyTime));
            toYAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(toEndY, endKeyTime, _sineEase));

            // Animate
            fromSlide.RenderTransform.BeginAnimation(TranslateTransform.XProperty, fromXAnimation);
            fromSlide.RenderTransform.BeginAnimation(TranslateTransform.YProperty, fromYAnimation);
            toSlide.RenderTransform.BeginAnimation(TranslateTransform.XProperty, toXAnimation);
            toSlide.RenderTransform.BeginAnimation(TranslateTransform.YProperty, toYAnimation);

            zIndexController.Stack(toSlide, fromSlide);
        }
    }
}