/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Thumbnailizer.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  OR (WPF only):

  xmlns:vm="clr-namespace:Thumbnailizer.ViewModel"
  DataContext="{Binding Source={x:Static vm:ViewModelLocatorTemplate.ViewModelNameStatic}}"
*/

namespace Thumbnailizer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// Use the <strong>mvvmlocatorproperty</strong> snippet to add ViewModels
    /// to this locator.
    /// </para>
    /// <para>
    /// In Silverlight and WPF, place the ViewModelLocatorTemplate in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Thumbnailizer.ViewModel"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// You can also use Blend to do all this with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// <para>
    /// In <strong>*WPF only*</strong> (and if databinding in Blend is not relevant), you can delete
    /// the Main property and bind to the ViewModelNameStatic property instead:
    /// </para>
    /// <code>
    /// xmlns:vm="clr-namespace:Thumbnailizer.ViewModel"
    /// DataContext="{Binding Source={x:Static vm:ViewModelLocatorTemplate.ViewModelNameStatic}}"
    /// </code>
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
        }

        #region ViewModel Ventana

        private static ThumbnalizerViewModel _ventanaViewModel;

        /// <summary>
        /// Gets the Ventana property.
        /// </summary>
        public static ThumbnalizerViewModel VentanaStatic
        {
            get
            {
                if (_ventanaViewModel == null)
                {
                    CreateVentana();
                }

                return _ventanaViewModel;
            }
        }

        /// <summary>
        /// Gets the Ventana property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ThumbnalizerViewModel Ventana
        {
            get
            {
                return VentanaStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Ventana property.
        /// </summary>
        public static void ClearVentana()
        {
            _ventanaViewModel.Cleanup();
            _ventanaViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the Ventana property.
        /// </summary>
        public static void CreateVentana()
        {
            if (_ventanaViewModel == null)
            {
                _ventanaViewModel = new ThumbnalizerViewModel();
            }
        }

        #endregion ViewModel Ventana

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearVentana();
        }
    }
}