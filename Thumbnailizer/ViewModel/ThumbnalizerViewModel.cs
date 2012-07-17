using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Thumbnailizer.Model;
using System.Threading;

namespace Thumbnailizer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class ThumbnalizerViewModel : ViewModelBase
    {
        //TODO: Verificar que los archivos son imagenes
        private List<string> _imageExtension = new List<string>() { ".jpe", ".jpg", ".png", ".gif" };
        /// <summary>
        /// Initializes a new instance of the ThumbnalizerViewModel class.
        /// </summary>
        public ThumbnalizerViewModel()
        {

            ArchivosSoltados = new ObservableCollection<ArchivoSoltadoModel>();

            InicializarCommands();

        }

        #region ViewModel Commands

        public RelayCommand<DragEventArgs> DropAnythingCommand { get; private set; }
        public RelayCommand ThumbnailizerCommand { get; private set; }

        #endregion

        private void InicializarCommands()
        {
            DropAnythingCommand = new RelayCommand<DragEventArgs>((e) =>
            {
                // Si no son archivos o carpetas no se hace nada
                // TODO: actualizar este comportamiento lanzar un mensaje de advertencia al usuario
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                var archivos = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (var path in archivos)
                {
                    Process(path);
                }
            });

            ThumbnailizerCommand = new RelayCommand(GenerateThumbnails);
        }

        private void Process(string path)
        {
            //TODO
            if (IsFolder(path))
            {
                ProcessFolder(path);
            }
            else
            {
                ProcessFile(path);
            }
        }

        private void ProcessFolder(string path)
        {
            throw new System.NotImplementedException();
        }

        private void ProcessFile(string path)
        {
            ArchivosSoltados.Add(new ArchivoSoltadoModel
            {
                Ruta = path
            });
        }

        private void GenerateThumbnails()
        {
            foreach (var item in ArchivosSoltados)
            {
                ThreadPool.QueueUserWorkItem(GenerateThumbnail, item);   
            }
        }

        private void GenerateThumbnail(object threaContext)
        {
            var item = (ArchivoSoltadoModel)threaContext;

            var path = Path.GetDirectoryName(item.Ruta);
            var name = Path.GetFileName(item.Ruta);
            if (UsarRuta)
            {
                path = Path.Combine(path, RutaThumbnail);
            }
            else
            {
                name = "thumb_" + name;
            }
            ImageProcess.GenerateThumbnail(
                ImageProcess.LoadImageFromStringPath(item.Ruta),
                Path.Combine(path, name));
        }

        private bool IsFolder(string path)
        {
            FileAttributes attrs = File.GetAttributes(path);
            return FileAttributes.Directory == (attrs & FileAttributes.Directory);
        }

        #region ViewModel Properties

        #region ViewModel ArchivosSoltados
        /// <summary>
        /// The <see cref="ArchivosSoltados" /> property's name.
        /// </summary>
        public const string ArchivosSoltadosPropertyName = "ArchivosSoltados";

        private ObservableCollection<ArchivoSoltadoModel> _archivosSoltados;

        /// <summary>
        /// Gets the ArchivosSoltados property.        
        /// </summary>
        public ObservableCollection<ArchivoSoltadoModel> ArchivosSoltados
        {
            get
            {
                return _archivosSoltados;
            }

            set
            {
                if (_archivosSoltados == value)
                {
                    return;
                }
                _archivosSoltados = value;
                // Update bindings, no broadcast
                RaisePropertyChanged(ArchivosSoltadosPropertyName);
            }
        }  
        #endregion

        #region ViewModel RutaThumbail
        /// <summary>
        /// The <see cref="RutaThumbnail" /> property's name.
        /// </summary>
        public const string RutaThumbnailPropertyName = "RutaThumbnail";

        private string _rutaThumbnail;

        /// <summary>
        /// Gets the RutaThumbnail property.        
        /// </summary>
        public string RutaThumbnail
        {
            get
            {
                return _rutaThumbnail;
            }

            set
            {
                if (_rutaThumbnail == value)
                {
                    return;
                }
                _rutaThumbnail = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(RutaThumbnailPropertyName);
            }
        } 
        #endregion

        #region ViewModel UsarRuta
        /// <summary>
        /// The <see cref="UsarRuta" /> property's name.
        /// </summary>
        public const string UsarRutaPropertyName = "UsarRuta";

        private bool _usarRuta;

        /// <summary>
        /// Gets the UsarRuta property.        
        /// </summary>
        public bool UsarRuta
        {
            get
            {
                return _usarRuta;
            }

            set
            {
                if (_usarRuta == value)
                {
                    return;
                }
                _usarRuta = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(UsarRutaPropertyName);
            }
        } 
        #endregion

        #region ViewModel Altura
        /// <summary>
        /// The <see cref="Altura" /> property's name.
        /// </summary>
        public const string AlturaPropertyName = "Altura";

        private int _altura;

        /// <summary>
        /// Gets the Altura property.        
        /// </summary>
        public int Altura
        {
            get
            {
                return _altura;
            }

            set
            {
                if (_altura == value)
                {
                    return;
                }
                _altura = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(AlturaPropertyName);
            }
        } 
        #endregion

        #region ViewModel Ancho
        /// <summary>
        /// The <see cref="Ancho" /> property's name.
        /// </summary>
        public const string AnchoPropertyName = "Ancho";

        private int _ancho;

        /// <summary>
        /// Gets the Ancho property.        
        /// </summary>
        public int Ancho
        {
            get
            {
                return _ancho;
            }

            set
            {
                if (_ancho == value)
                {
                    return;
                }
                _ancho = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(AnchoPropertyName);
            }
        } 
        #endregion

        #endregion


    }
}