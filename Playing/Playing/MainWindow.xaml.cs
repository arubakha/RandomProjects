using System;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;

namespace Playing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class CharacterCollection : ObservableCollection<Character>
    { 
    }
}
