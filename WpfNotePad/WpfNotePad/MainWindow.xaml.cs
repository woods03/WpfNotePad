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
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace WpfNotePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string title = "Untitled";
        private bool textEditing = false;
        private bool statusBarBool = false;
        private bool isSaved = true;
        
        public MainWindow()
        {
            InitializeComponent();

            this.Title = title + " - WPFNP";
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
        
        private void btn_status_click(object sender, RoutedEventArgs e)
        {
            statusBarBool = !statusBarBool;
            statusBarEnab(statusBarBool);
        }

        private void textBox_textChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;
        }

        private void textBox_SelectChanged(object sender, RoutedEventArgs e)
        {
            if (textBox.SelectedText == "")
            {
                textEditing = false;
            }
            else
            {
                textEditing = true;
            }

            textEdit(textEditing);
        }

        private void btn_OpenFile_click(object sender, RoutedEventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBox.Show("Save file?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //no
                    OpenFileDialog();
                }
                else
                {
                    //yes
                    SaveFileDialog();
                    OpenFileDialog();
                }
            }
            else
            {
                OpenFileDialog();
            }

        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text file file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                title = openFileDialog.FileNames.First().Split('\\').Last().Split('.').First();
                this.Title = title + " - WPFNP";
            }
        }

        private void SaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt",
                FileName = title
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, textBox.Text);
            }
        }

        private void statusBarEnab(bool sbb)
        {
            if (sbb)
            {
                main_Grid_TextBox.Height = new GridLength(1, GridUnitType.Star);
                main_Grid_StatusRow.Height = GridLength.Auto;
            }
            else
            {
                main_Grid_StatusRow.Height = new GridLength(20, GridUnitType.Pixel);
            }
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
