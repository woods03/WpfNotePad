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
        private string title;
        private string path;
        private bool textEditing;
        private bool statusBarBool;
        private bool isSaved;
        private bool isWrap;
 
        public MainWindow()
        {
            InitializeComponent();
            Update();
        }

        private void Update()
        {
            title = "Untitled";
            path = "";
            textEditing = false;
            statusBarBool = false;
            isSaved = true;
            isWrap = true;
            textBox.Text = "";

            this.Title = title + " - WPFNP";
        }

        #region File
        private void btn_Create_click(object sender, RoutedEventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBox.Show("Save file?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //no
                    Update();
                }
                else
                {
                    //yes
                    SaveFile();
                    Update();
                }
            }
            else { Update(); }
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
                    SaveFile();
                    OpenFileDialog();
                }
            }
            else { OpenFileDialog(); }
        }

        private void btn_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void btn_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog();
        }

        private void Btn_exit_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBox.Show("Save file?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //no
                    this.Close();
                }
                else
                {
                    //yes
                    SaveFile();
                    this.Close();
                }
            }
            else { this.Close(); }
        }
        #endregion

        #region editing
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
        
        private void btn_SelectAll_click(object sender, RoutedEventArgs e)
        {
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }
        #endregion

        private void btn_status_click(object sender, RoutedEventArgs e)
        {
            statusBarBool = !statusBarBool;
            statusBarEnab(statusBarBool);
        }

        private void textBox_textChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;

            label_FontInfo.Text = textBox.FontFamily.ToString() + ", " + textBox.FontSize + "    " ;

            progressBar_CharInfo.Value = textBox.Text.Length / 10 ;
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

            if (isWrap == true)
            {
                int curr_line = this.textBox.GetLineIndexFromCharacterIndex(this.textBox.SelectionStart) + 1;
                int firstChar = this.textBox.GetCharacterIndexFromLineIndex(curr_line - 1);
                int curr_column = this.textBox.CaretIndex - firstChar;

                textBlock_LineandStroke.Text = $"Line: {curr_line}, Stroke: {curr_column}";
            }
            else
            {
                textBlock_LineandStroke.Text = "Wrap!";
            }
        }
        
        private void btn_WordWrap_Click(object sender, RoutedEventArgs e)
        {
            isWrap = !isWrap;

            if(isWrap)
            {
                textBox.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                textBox.TextWrapping = TextWrapping.NoWrap;
            }
        }
        
        private void form_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBox.Show("Save file?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }   
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|HTML Files(*.html)|*.html|Rich Text Files(*.rtf)|*.rtf"
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                isSaved = true;
                title = openFileDialog.FileNames.First().Split('\\').Last().Split('.').First();
                path = openFileDialog.FileNames.First();
                this.Title = title + " - WPFNP";
            }
        }

        private void SaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|HTML Files(*.html)|*.html|Rich Text Files(*.rtf)|*.rtf",
                FileName = title
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, textBox.Text);
                isSaved = true;
                title = dialog.FileNames.First().Split('\\').Last().Split('.').First();
                path = dialog.FileNames.First();
                this.Title = title + " - WPFNP";
            }
        }

        private void SaveFile()
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, textBox.Text);
                isSaved = true;
            }
            else
            {
                SaveFileDialog();
            }
        }

        private void statusBarEnab(bool sbb)
        {
            if (sbb)
            {
                main_Grid.RowDefinitions[2].Height = new GridLength(0);
                main_Grid.RowDefinitions[1].Height = new GridLength(20 , GridUnitType.Star);
            }
            else
            {
                main_Grid.RowDefinitions[2].Height = new GridLength(20, GridUnitType.Pixel);
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

        private void btn_undo_click(object sender, RoutedEventArgs e)
        {
            textBox.Undo();
        }
    }
}
