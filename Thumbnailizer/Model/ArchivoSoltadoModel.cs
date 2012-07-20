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
        
        #endregion

    }
}
