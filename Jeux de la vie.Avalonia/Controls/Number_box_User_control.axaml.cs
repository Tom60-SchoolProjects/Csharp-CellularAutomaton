using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Jeux_de_la_vie.Avalonia.Controls
{
    public partial class Number_box_User_control : UserControl
    {
        public Number_box_User_control()
        {
            InitializeComponent();
            DataContext = this;

            suffix = string.Empty;
        }

        public string Title
        {
            get => Title_text.Text;
            set => Title_text.Text = value;
        }

        private int number;
        public int Number
        {
            get => number;
            set
            {
                number = value;
                Number_text.Text = value.ToString() + Suffix;
            }
        }

        private string suffix;
        public string Suffix
        {
            get => suffix;
            set
            {
                suffix = value;
                Number_text.Text = Number.ToString() + value;
            }
        }

        public int SmallChange { get; set; } = 1;
        public int MaximumValue { get; set; } = int.MaxValue;
        public int MinimumValue { get; set; } = int.MinValue;

        public event EventHandler<int>? ValueChanged;

        public void On_Click_up_Button_Click(object sender, RoutedEventArgs e)
        {
            Number += SmallChange;

            if (Number > MaximumValue)
                Number = MaximumValue;

            ValueChanged?.Invoke(this, Number);
        }

        public void On_Click_down_Button_Click(object sender, RoutedEventArgs e)
        {
            Number -= SmallChange;

            if (MinimumValue > Number)
                Number = MinimumValue;

            ValueChanged?.Invoke(this, Number);
        }

        private void Number_text_On_lost_focus(object sender, RoutedEventArgs e)
        {
            Je_ne();
        }

        private void Number_text_Key_up(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
                Je_ne();
        }

        private void Je_ne()
        {
            string number;

            if (string.IsNullOrEmpty(suffix))
                number = Number_text.Text;
            else
                number = Number_text.Text.Replace(suffix, string.Empty);

            try
            {
                Number = Convert.ToInt32(number);

                if (MinimumValue > Number)
                    Number = MinimumValue;

                if (Number > MaximumValue)
                    Number = MaximumValue;

                ValueChanged?.Invoke(this, Number);
            }
            catch
            {
                Number_text.Text = Number.ToString() + Suffix;
            }
        }
    }
}
