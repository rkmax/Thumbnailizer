using System.Windows.Media;

namespace Thumbnailizer.Model
{
    public class ArchivoSoltadoModel : BaseModel
    {
        #region Model Private Properties
        private string _ruta;
        private ImageSource _thumbnail;
        private int _width;
        private int _height; 
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
