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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CommaOfDidymus
{
    /// <summary>
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }

        private void KnobRightButtonDown(object sender, MouseButtonEventArgs e)
        {
			if (HandleExclusive != null)
				HandleExclusive(this, new EventArgs());
        }

        public event EventHandler HandleExclusive;

        void OnScroll(object sender, ScrollEventArgs e)
        {
            Note note = DataContext as Note;
            MainViewModel vm = (Parent as FrameworkElement).DataContext as MainViewModel;

            bool down = e.ScrollEventType == ScrollEventType.SmallIncrement;
			if (vm.Numerator <= 0 || vm.Denominator <= 0)
			{
				note.Frequency += down ? -1 : 1;
			}
			else
			{
				double ratio = vm.Numerator / (double)vm.Denominator;
				note.Frequency *= down ? 1 / ratio : ratio;
			}
        }
    }
}
