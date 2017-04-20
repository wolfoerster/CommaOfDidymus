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
using NAudio.Wave;

namespace CommaOfDidymus
{
    public class WaveProvider : WaveProvider32
    {
		public WaveProvider(Notes notes)
        {
			this.notes = notes;
            SetWaveFormat(44100, 1);
		}
		Notes notes;
		uint sample;

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int n = 0; n < sampleCount; n++, sample++)
            {
                double t = sample / (double)WaveFormat.SampleRate;
                buffer[n + offset] = (float)notes.GetAmplitude(t);
            }
            return sampleCount;
        }
	}
}
