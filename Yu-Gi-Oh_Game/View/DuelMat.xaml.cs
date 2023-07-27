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
using Yu_Gi_Oh_Game.ViewModel;

namespace Yu_Gi_Oh_Game.View
{
    /// <summary>
    /// Interaction logic for DuelMat.xaml
    /// </summary>
    public partial class DuelMat : UserControl
    {
        public DuelMat()
        {
            InitializeComponent();
            DuelMatViewModel viewModel = new DuelMatViewModel();
            DataContext = viewModel;
        }
    }
}
