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

namespace CommaOfDidymus
{
    public class Note : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public Note(double frq, double amp)
        {
            Frequency = frq;
            Amplitude = amp;
        }

        public double Frequency
        {
            get { return frequency; }
            set
            {
                if (frequency != value)
                {
                    frequency = Math.Max(value, 1);
					//--- time dependent calculations need the frequency multiplied by 2PI (which is then called omega):
                    Omega = 2 * Math.PI * Math.Round(value);//--- non-integral values end up with a click in NAudio!
                    FirePropertyChanged("Frequency");
                }
            }
        }
        double frequency;

        public double Omega { get; private set; }

        public double Amplitude
        {
            get { return amplitude; }
            set
            {
                if (amplitude != value)
                {
                    amplitude = Math.Min(value, 1);
                    FirePropertyChanged("Amplitude");
                }
            }
        }
        double amplitude;

		public bool IsUsed
		{
			get { return isUsed; }
			set
			{
				if (isUsed != value)
				{
					isUsed = value;
					FirePropertyChanged("IsUsed");
				}
			}
		}
		bool isUsed;
	}
}
