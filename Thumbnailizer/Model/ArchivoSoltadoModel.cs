using System.Windows.Media;
using System.IO;
namespace Thumbnailizer.Model
{
    public class ArchivoSoltadoModel : BaseModel
    {
        #region Model Private Properties
        private string _ruta;
        private string _nombreArchivo;
        private string _nombreDirectorio;
        private ImageSource _thumbnail;
        private int _width;
        private int _height;
        private bool _estaProcesado;

        #endregion

        #region Model Public Properties
        public string Ruta
        {
            get { return _ruta; }

            set
            {
                if (value == _ruta) return;
                _ruta = value;
                OnPropertyChanged("Ruta");
            }
        }

        public string NombreArchivo
        {
            get { return _nombreArchivo; }

            set
            {
                if (value == _nombreArchivo) return;
                _nombreArchivo = value;
                OnPropertyChanged("NombreArchivo");
            }

        }

        public string NombreDirectorio
        {
            get { return _nombreDirectorio; }

            set
            {
                if (value == _nombreDirectorio) return;
                _nombreDirectorio = value;
                OnPropertyChanged("NombreDirectorio");
            }

        }

        public int Height
        {
            get { return _height; }

            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        public int Width
        {
            get { return _width; }

            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        public bool EstaProcesado
        {
            get { return _estaProcesado; }

            set
            {
                if (value == _estaProcesado) return;
                _estaProcesado = value;
                OnPropertyChanged("EstaProcesado");
            }

        }

        public ImageSource Thumbnail
        {
            get { return _thumbnail; }

            set
            {
                if (value == _thumbnail) return;
                _thumbnail = value;
                OnPropertyChanged("Thumbnail");
            }

        } 
        #endregion

    }
}
