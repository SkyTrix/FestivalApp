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
using System.Windows.Shapes;

namespace FestivalApp.View
{
    /// <summary>
    /// Interaction logic for EditGenreWindow.xaml
    /// </summary>
    public partial class EditGenreWindow : Window
    {
        public EditGenreWindow()
        {
            InitializeComponent();
            this.Loaded += EditGenreWindow_Loaded;
        }

        void EditGenreWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
            txtName.SelectAll();
        }
    }
}
