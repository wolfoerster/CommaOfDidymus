//******************************************************************************************
// Copyright © 2017 Wolfgang Foerster (wolfoerster@gmx.de)
//
// This file is part of the CommaOfDidymus project which can be found on github.com
//
// CommaOfDidymus is free software: you can redistribute it and/or modify it under the terms 
// of the GNU General Public License as published by the Free Software Foundation, 
// either version 3 of the License, or (at your option) any later version.
// 
// CommaOfDidymus is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//******************************************************************************************
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CommaOfDidymus
{
    public class Knob : FrameworkElement
    {
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Knob),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(Knob), 
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            isMouseInside = true;
            InvalidateVisual();
        }
        bool isMouseInside;

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            isMouseInside = false;
            InvalidateVisual();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            double value = Value + 0.05 * Math.Sign(e.Delta);
            Value = Math.Min(Math.Max(value, 0), 1);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            IsChecked ^= true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Opacity = IsChecked ? 1 : 0.5;

            double radiusX = RenderSize.Width * 0.5;
            double radiusY = RenderSize.Height * 0.5;
            Point center = new Point(radiusX, radiusY);
            dc.DrawEllipse(isMouseInside ? Brushes.White : Brushes.LightGray, pen, center, radiusX, radiusY);

            Geometry arc = CreateArcGeometry(center, radiusX, radiusY);
            dc.DrawGeometry(isMouseInside ? Brushes.Red : Brushes.Firebrick, pen, arc);
        }
        Pen pen = new Pen(Brushes.Black, 1) { LineJoin = PenLineJoin.Bevel };

        Geometry CreateArcGeometry(Point center, double radiusX, double radiusY)
        {
            StreamGeometry streamGeometry = new StreamGeometry();

            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                double phi = Value * Math.PI * 2;
                double arc = Math.Abs(phi * (radiusX + radiusY) * 0.5);

                int iSteps = (int)(arc / 2.0 + 1.5);
                double dPhi = phi / iSteps;

                ctx.BeginFigure(center, true, true);

                for (int i = 0; i <= iSteps; i++)
                {
                    double angle = i * dPhi;
                    double x = radiusX * Math.Sin(angle);
                    double y = radiusY * Math.Cos(angle);
                    Point pt = new Point(center.X - x, center.Y + y);
                    ctx.LineTo(pt, true, false);
                }
            }

            return streamGeometry;
        }
    }
}
