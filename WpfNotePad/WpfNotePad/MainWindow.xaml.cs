using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfNotePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool textEditing = false;

        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            status_text.Text = "100%";
        }
        
        private void Btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_paste_click(object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = Clipboard.GetText();
        }

        private void btn_cut_click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox.SelectedText);
            textBox.SelectedText = "";
        }

        private void btn_copy_click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox.SelectedText);
        }

        private void btn_del_click(object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = "";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if(textBox.SelectedText == "")
            {
                textEditing = false;
            }
            else
            {
                textEditing = true;
            }

            textEdit(textEditing);
        }

        private void textEdit(bool te)
        {
            btn_copy.IsEnabled = te;
            btn_cut.IsEnabled = te;
            btn_del.IsEnabled = te;

            btn_copy1.IsEnabled = te;
            btn_cut1.IsEnabled = te;
            btn_del1.IsEnabled = te;
        }

    }
}
