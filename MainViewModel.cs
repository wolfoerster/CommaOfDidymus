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
using System.ComponentModel;
using System.Threading.Tasks;
using NAudio.Wave;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CommaOfDidymus
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

		WaveProvider waveProvider;
		WaveOut waveOut = new WaveOut();
		LineSeries series = new LineSeries() { StrokeThickness = 1, Color = OxyColors.Blue };
		LinearAxis xAxis = new LinearAxis { Position = AxisPosition.Bottom, IsZoomEnabled = false, IsPanEnabled = false, Minimum = 0, Maximum = 0.02727, Title = "Time [s]" };
		LinearAxis yAxis = new LinearAxis { Position = AxisPosition.Left, IsZoomEnabled = false, IsPanEnabled = false, Title = "Amplitude" };

		public MainViewModel()
        {
			Notes = new Notes();
			waveProvider = new WaveProvider(Notes);

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(xAxis);
            PlotModel.Axes.Add(yAxis);
            PlotModel.Series.Add(series);
			PlotModel.PlotMargins = new OxyThickness(40, 0, 0, 34);
		}

        public Notes Notes { get; private set; }

        public PlotModel PlotModel { get; private set; }

        public void Init(bool mode)
        {
            if (mode)
            {
                waveOut.Init(waveProvider);
                waveOut.Play();
                Update();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
        }

        public int Numerator
        {
            get { return numerator; }
            set
            {
                if (numerator != value)
                {
                    numerator = value;
                    FirePropertyChanged("Numerator");
                }
            }
        }
        private int numerator = 1;

        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (denominator != value)
                {
                    denominator = value;
                    FirePropertyChanged("Denominator");
                }
            }
        }
        private int denominator;

        public double TotalTime
        {
            get { return xAxis.Maximum; }
            set
            {
                if (xAxis.Maximum != value)
                {
                    xAxis.Maximum = value;
                    Update();
                }
            }
        }

        public void Update()
        {
            AsyncUpdate();
        }

        async void AsyncUpdate()
        {
            if (isBusy) return;
            isBusy = true;

            series.Points.Clear();
            double minY = 0, maxY = 0;

            await Task.Run(() =>
            {
                double t = 0;
                double dt = 1.0 / waveProvider.WaveFormat.SampleRate;
                for (int i = 0; t <= TotalTime; ++i, t = i * dt)
                {
                    double y = Notes.GetAmplitude(t);
                    series.Points.Add(new DataPoint(t, y));
                    minY = Math.Min(minY, y);
                    maxY = Math.Max(maxY, y);
                }
            });

            if (maxY > 1e-6)
            {
                yAxis.Minimum = minY * 1.1;
                yAxis.Maximum = maxY * 1.1;
            }

            PlotModel.InvalidatePlot(true);
            isBusy = false;
        }
        bool isBusy;
    }
}
