using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Thumbnailizer.Model;

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
        #region Private fields

        private int _conteo;
        private HashSet<string> _hash;
        private List<string> _imageExtension;
        #endregion Private fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ThumbnalizerViewModel class.
        /// </summary>
        public ThumbnalizerViewModel()
        {
            _imageExtension = new List<string>() { ".jpe", ".jpg", ".png", ".gif" };
            _prefix = "thumb_";

            _archivosSoltados = new ObservableCollection<ArchivoSoltadoModel>();
            _hash = new HashSet<string>();

            DummyInfo();

            InicializarCommands();
        }

        #endregion Constructors

        #region ViewModel Commands

        public RelayCommand<ArchivoSoltadoModel> DeleteFromListCommand { get; private set; }

        public RelayCommand<DragEventArgs> DropAnythingCommand { get; private set; }

        public RelayCommand LimpiarListaCommand { get; private set; }

        public RelayCommand LimpiarListaCompletadosCommand { get; private set; }

        public RelayCommand<System.Uri> OpenInBrowserCommand { get; private set; }

        public RelayCommand ThumbnailizerCommand { get; private set; }
        #endregion ViewModel Commands

        #region ViewModel Private Methods

        /// <summary>
        /// Llena informacion falsa, util en tiempo de diseño
        /// </summary>
        private void DummyInfo()
        {
            if (IsInDesignMode)
            {
                List<string> tempList = new List<string>
                {
                    @"F:\fotos visiometros\juno\DSC02532.JPG",
                    @"F:\fotos visiometros\juno\DSC02533.JPG",
                    @"F:\fotos visiometros\juno\DSC02534.JPG",
                };

                tempList.ForEach(path =>
                {
                    var element = new ArchivoSoltadoModel
                    {
                        Ruta = path,
                        NombreArchivo = Path.GetFileName(path),
                        NombreDirectorio = Path.GetDirectoryName(path)
                    };
                    if (tempList.IndexOf(path) % 2 == 0)
                    {
                        element.EstaProcesado = true;
                    }
                    ArchivosSoltados.Add(element);
                });
            }
        }

        /// <summary>
        /// Tomar un modelo de archivo y lo procesa
        /// redimensinandolo segun las opciones disponibles
        /// </summary>
        /// <param name="threaContext"></param>
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
                name = _prefix + name;
            }

            ImageProcess.GenerateThumbnail(
                ImageProcess.LoadImageFromStringPath(item.Ruta),
                Path.Combine(path, name), Ancho, Altura);
        }

        /// <summary>
        /// Procesa todas las imagenes listas para procesar
        /// </summary>
        private void GenerateThumbnails()
        {
            if (Altura == 0 && Ancho == 0) return;

            _conteo = 0;
            foreach (var item in ArchivosSoltados)
            {
                if (!item.EstaProcesado)
                {
                    _conteo++;

                    var tsk = Task.Factory.StartNew(GenerateThumbnail, item).ContinueWith(t =>
                    {
                        if (t.IsCompleted)
                        {
                            item.EstaProcesado = true;
                            _conteo--;
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        /// <summary>
        /// Inicializa los comandos para utilizarlos en el XAML
        /// </summary>
        private void InicializarCommands()
        {
            DropAnythingCommand = new RelayCommand<DragEventArgs>((e) =>
            {
                // Si no son archivos o carpetas no se hace nada
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                var archivos = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (var path in archivos)
                {
                    Process(path);
                }
            });

            OpenInBrowserCommand = new RelayCommand<System.Uri>(uri =>
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
            });

            ThumbnailizerCommand = new RelayCommand(GenerateThumbnails, () =>
            {
                return _conteo == 0;
            });

            LimpiarListaCommand = new RelayCommand(() =>
            {
                _archivosSoltados.Clear();
                _hash.Clear();
            });

            LimpiarListaCompletadosCommand = new RelayCommand(() =>
            {
                var no_eliminar = new ObservableCollection<ArchivoSoltadoModel>();
                foreach (var item in _archivosSoltados)
                {
                    if (item.EstaProcesado)
                    {
                        _hash.Remove(item.Ruta);
                    }
                    else
                    {
                        no_eliminar.Add(item);
                    }
                }

                ArchivosSoltados = no_eliminar;
            });

            DeleteFromListCommand = new RelayCommand<ArchivoSoltadoModel>(a =>
            {
                var item = _archivosSoltados.Where(b => a.Ruta == b.Ruta).SingleOrDefault();
                _archivosSoltados.Remove(item);
                _hash.Remove(item.Ruta);
            });
        }

        /// <summary>
        /// Determinar si la ruta pasada es una carpeta
        /// </summary>
        /// <param name="path">ruta de la carpeta</param>
        /// <returns>verdadero si es una carpeta</returns>
        private bool IsFolder(string path)
        {
            FileAttributes attrs = File.GetAttributes(path);
            return FileAttributes.Directory == (attrs & FileAttributes.Directory);
        }

        /// <summary>
        /// Discrimina archivos y carpetas
        /// </summary>
        /// <param name="path">ruta a discriminar</param>
        private void Process(string path)
        {
            if (IsFolder(path))
            {
                ProcessFolder(path);
            }
            else
            {
                ProcessFile(path);
            }
        }

        /// <summary>
        /// Toma la informacion de la ruta y lo prepara para procesar
        /// </summary>
        /// <param name="path"></param>
        private void ProcessFile(string path)
        {
            if (_hash.Contains(path)) return;
            var path_ext = Path.GetExtension(path).ToLower();

            if (null == _imageExtension.Where(a => a == path_ext).SingleOrDefault()) return;

            var element = new ArchivoSoltadoModel
            {
                Ruta = path,
                NombreArchivo = Path.GetFileName(path),
                NombreDirectorio = Path.GetDirectoryName(path),
                EstaProcesado = false
            };
            _hash.Add(path);
            ArchivosSoltados.Add(element);
        }

        /// <summary>
        /// Lee todas las entradas de la carpeta y los procesa
        /// </summary>
        /// <param name="path">ruta de la carpeta</param>
        private void ProcessFolder(string path)
        {
            var listFiles = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            foreach (var item in listFiles)
            {
                ProcessFile(item);
            }

            var listDirectories = Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            foreach (var item in listDirectories)
            {
                ProcessFolder(item);
            }
        }

        #endregion ViewModel Private Methods

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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

        #endregion ViewModel ArchivosSoltados

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

        #endregion ViewModel RutaThumbail

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

        #endregion ViewModel UsarRuta

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

        #endregion ViewModel Altura

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

        #endregion ViewModel Ancho

        #region ViewModel Prefix

        /// <summary>
        /// The <see cref="Prefix" /> property's name.
        /// </summary>
        public const string PrefixPropertyName = "Prefix";

        private string _prefix;

        /// <summary>
        /// Gets the Prefix property.
        /// </summary>
        public string Prefix
        {
            get
            {
                return _prefix;
            }

            set
            {
                if (_prefix == value)
                {
                    return;
                }
                _prefix = value;
                // Update bindings, no broadcast
                RaisePropertyChanged(PrefixPropertyName);
            }
        }

        #endregion ViewModel Prefix

        #endregion ViewModel Properties
    }
}