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
using System.Collections.Generic;

namespace CommaOfDidymus
{
    public class Notes : List<Note>
    {
        public Notes()
        {
            for (int i = 1; i < 12; i++)
            {
				Note note = new Note(110 * i, (11 - i) * 0.1 + 0.05);
                this.Add(note);
            }
            this[0].IsUsed = true;
        }

        public double GetAmplitude(double t)
        {
            int numUsed = 0;
            double amplitude = 0;
            foreach (var note in this)
            {
                if (note.IsUsed)
                {
                    numUsed++;
					amplitude += note.Amplitude * Math.Cos(2 * Math.PI * note.Frequency * t);
                }
            }
			return numUsed > 0 ? amplitude / numUsed : 0;
        }
    }
}
