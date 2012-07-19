using MahApps.Metro.Controls;
using Thumbnailizer.ViewModel;

namespace Thumbnailizer.Windows
{
    /// <summary>
    /// Lógica de interacción para VentanaPrincipal.xaml
    /// </summary>
    public partial class VentanaPrincipal : MetroWindow
    {
        public VentanaPrincipal()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}
