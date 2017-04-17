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
using System.ComponentModel;

namespace CommaOfDidymus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel = new MainViewModel();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Loaded += MeLoaded;
            Closing += MeClosing;
        }
        MainViewModel viewModel;

        private void MeLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.Init(true);
        }

        private void MeClosing(object sender, CancelEventArgs e)
        {
            viewModel.Init(false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
                Close();
        }

        private void SomethingChanged(object sender, EventArgs e)
        {
            viewModel.Update();
        }

        private void PlotMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                viewModel.TotalTime *= 1.1;
            else
                viewModel.TotalTime /= 1.1;
        }
    }
}
