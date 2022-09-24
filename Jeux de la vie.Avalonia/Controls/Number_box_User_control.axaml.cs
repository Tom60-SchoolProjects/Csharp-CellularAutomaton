using Avalonia;
using Avalonia.Controls;
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

        public event EventHandler<int> ValueChanged;

        public void On_Click_up_Button_Click(object sender, RoutedEventArgs e)
        {
            Number++;
            ValueChanged.Invoke(this, Number);
        }

        public void On_Click_down_Button_Click(object sender, RoutedEventArgs e)
        {
            Number--;
            ValueChanged.Invoke(this, Number);
        }
    }
}
