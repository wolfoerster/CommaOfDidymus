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
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CommaOfDidymus
{
    /// <summary>
    /// Interaction logic for RatioControl.xaml
    /// </summary>
    public partial class RatioControl : UserControl
    {
        public RatioControl()
        {
            InitializeComponent();
            ListenToTextChanges(true);
        }

        void ListenToTextChanges(bool mode)
        {
            if (mode)
            {
                numerator.TextChanged += TextChanged;
                denominator.TextChanged += TextChanged;
            }
            else
            {
                numerator.TextChanged -= TextChanged;
                denominator.TextChanged -= TextChanged;
            }
        }

        MainViewModel MainViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        List<RadioButton> GetButtons(out List<int> numerators, out List<int> denominators)
        {
            List<RadioButton> buttons = new List<RadioButton>();
            numerators = new List<int>();
            denominators = new List<int>();
            int maxi = VisualTreeHelper.GetChildrenCount(stackPanel);
            for (int i = 1; i < maxi; ++i)
            {
                RadioButton button = VisualTreeHelper.GetChild(stackPanel, i) as RadioButton;
                string[] numbers = (button.Tag as string).Split(new char[] { '/' });
                numerators.Add(int.Parse(numbers[0]));
                denominators.Add(int.Parse(numbers[1]));
                buttons.Add(button);
            }
            return buttons;
        }

        /// <summary>
        /// Whenever some text changes, uncheck all radio buttons and the select the corresponding one (if any).
        /// </summary>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            List<int> numerators, denominators;
            List<RadioButton> buttons = GetButtons(out numerators, out denominators);
            for (int i = 0; i < buttons.Count; ++i)
            {
                buttons[i].IsChecked = false;
                if (MainViewModel.Numerator == numerators[i] && MainViewModel.Denominator == denominators[i])
                {
                    buttons[i].IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Whenever the user clicks a radio button, change the numbers accordingly.
        /// </summary>
        void OnClick(object sender, RoutedEventArgs e)
        {
            List<int> numerators, denominators;
            List<RadioButton> buttons = GetButtons(out numerators, out denominators);
            for (int i = 0; i < buttons.Count; ++i)
            {
                if (buttons[i] == sender)
                {
                    ListenToTextChanges(false);
                    MainViewModel.Numerator = numerators[i];
                    MainViewModel.Denominator = denominators[i];
                    ListenToTextChanges(true);
                    break;
                }
            }
        }
    }
}
