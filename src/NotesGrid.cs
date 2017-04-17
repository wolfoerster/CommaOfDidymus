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
using System.Windows.Controls;
using System.ComponentModel;

namespace CommaOfDidymus
{
    public class NotesGrid : Grid
    {
        public event EventHandler SomethingChanged;

        public Notes Notes
        {
            get { return (Notes)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }

        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(Notes), typeof(NotesGrid),
                new UIPropertyMetadata(null, OnNotesChanged));

        private static void OnNotesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
                throw new ApplicationException("Not supported");

            NotesGrid notesGrid = d as NotesGrid;
            notesGrid.Init();
        }

        private void Init()
        {
            for (int i = 0; i < Notes.Count; i++)
            {
                Notes[i].PropertyChanged += NotePropertyChanged;

                NoteControl noteControl = new NoteControl();
                noteControl.HandleExclusive += HandleExclusive;
                noteControl.DataContext = Notes[i];
                Grid.SetRow(noteControl, i);

                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Children.Add(noteControl);
            }
        }

        private void NotePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SomethingChanged != null)
				SomethingChanged(this, new EventArgs());
        }

        void HandleExclusive(object sender, EventArgs e)
        {
            NoteControl noteControl = sender as NoteControl;
            int row = Grid.GetRow(noteControl);

            for (int i = 0; i < Notes.Count; i++)
            {
                Notes[i].PropertyChanged -= NotePropertyChanged;
                Notes[i].IsUsed = i == row;
                Notes[i].PropertyChanged += NotePropertyChanged;
            }

            if (SomethingChanged != null)
				SomethingChanged(this, new EventArgs());
        }
    }
}
