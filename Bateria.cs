using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAM_M07_ACT_03_Alberto_Perez_del_Rio
{
    public partial class Bateria : UserControl
    {
        #region Variables
        private Color _lowBatteryColor = Color.Red;
        private Color _goodBatteryColor = Color.LimeGreen;
        private int _chargeLevel = 100;
        private bool _isCharging = false;
        private bool _isDepleted = false;
        private bool _isLow = false;
        private bool _isDragging = false;
        private string _message = "";

        #endregion

        #region Constructor
        public Bateria()
        {
            InitializeComponent();

        }
        #endregion

        #region Propiedades

        // Propiedad para controlar valores de carga entre 0 y 100
        public int ChargeLevel
        {
            get { return _chargeLevel; }
            set
            {
                _chargeLevel = value;
                if (_chargeLevel < 0) _chargeLevel = 0;
                if (_chargeLevel > 100) _chargeLevel = 100;
                // Cambia el color de la batería si la carga es menor o igual al 10%
                if (_chargeLevel <= 10) _isLow = true;
                Invalidate();
            }
        }

        // Propiedad para controlar el color de la batería cuando está agotada, si no se establece ningún color, se usará el color rojo, le afecta cuando esta por debajo del 10%
        public bool IsDepleted
        {
            get { return _isDepleted; }
            set
            {
                _isDepleted = value;
                Invalidate();
            }
        }

        // Propiedad para controlar el color de la batería cuando está cargando
        public bool IsCharging
        {
            get { return _isCharging; }
            set
            {
                _isCharging = value;
                Invalidate();
            }
        }
        #endregion

        #region Eventos
        public event EventHandler LevelChanged;

        // Evento para controlar el nivel de carga
        protected virtual void OnLevelChanged(EventArgs e)
        {
            LevelChanged?.Invoke(this, e); // Controla que el evento no sea nulo
        }

        #region "Dibuja bateria"
        // Evento que pintará la batería
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DoubleBuffered = true; // Evita el parpadeo

            // Pinta la batería en el centro ocupando el 80% del ancho y el 80% del alto
            float left = this.Width * 0.1f;
            float top = this.Height * 0.1f;
            float width = this.Width * 0.8f;
            float height = this.Height * 0.7f;

            // Pinta el fondo de la batería
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.DarkGray, left, top, width, height);

            // Pinta el borde de la batería
            g.DrawRectangle(Pens.White, left, top, width, height);

            // Pinta la carga
            float chargeHeight = height * (_chargeLevel / 100f);
            g.FillRectangle(new SolidBrush(_isDepleted ? _lowBatteryColor : _goodBatteryColor), left, top + height - chargeHeight, width, chargeHeight);

            // Pinta la base de la batería
            g.DrawImage(DAM_M07_ACT_03_Alberto_Perez_del_Rio.Resource1._base, 0, 0, 150, 300); // Pinta la base de la batería

            // Controla el mensaje que se mostrará en la batería y la imagen que se pintará
            if (_isCharging)
            {
                _isDepleted = false;
                _isLow = false;
                // mensaje de carga
                _message = "Cargando...";
                g.DrawImage(DAM_M07_ACT_03_Alberto_Perez_del_Rio.Resource1.carga, 0, 0, 150, 300); // Pinta la base de la batería

            }

            if (_isLow)
            {
                _isDepleted = false;
                _isCharging = false;
                // Cambia el color de la batería a rojo sólo en la parte que corresponda al nivel de carga

                g.FillRectangle(new SolidBrush(_lowBatteryColor), left, top + height - chargeHeight, width, chargeHeight);
            }

            if (_isDepleted) // Si la batería está agotada
            {
                _isCharging = false;
                _chargeLevel = 0;
                // rellena la batería de rojo
                g.FillRectangle(new SolidBrush(_lowBatteryColor), left, top, width, height);
                _message = "Agotada";
                g.DrawImage(DAM_M07_ACT_03_Alberto_Perez_del_Rio.Resource1.agotada, 0, 0, 150, 300); // Pinta la base de la batería
                                                                                                     // impide que se cambie el porcentaje con el raton
                this.Enabled = false;
            }

            if (!_isCharging && !_isDepleted) // En caso de que no este cargando ni agotada
            {
                _message = "";
                _isLow = false;
                g.DrawImage(DAM_M07_ACT_03_Alberto_Perez_del_Rio.Resource1._base, 0, 0, 150, 300); // Pinta la base de la batería

                // Habilita el control para que se pueda cambiar el porcentaje con el raton
                this.Enabled = true;
            }

            // Crear una nueva fuente en negrita con un tamaño personalizado
            Font boldFont = new Font(this.Font.FontFamily, 12, FontStyle.Bold);

            // Pintar el porcentaje de carga
            string percentage = _chargeLevel + "%";
            SizeF percentageSize = g.MeasureString(percentage, boldFont);
            Brush percentageBrush = Brushes.Black;

            // Pintar el porcentaje de carga
            g.DrawString(percentage, boldFont, percentageBrush, (this.Width - percentageSize.Width) / 2, this.Height - percentageSize.Height - 30);

            // Crear una nueva fuente en negrita con un tamaño personalizado
            SizeF messageSize = g.MeasureString(_message, boldFont);
            Brush messageBrush = Brushes.Black;

            // Pintar el mensaje de carga
            g.DrawString(_message, boldFont, messageBrush, (this.Width - messageSize.Width) / 2, (this.Height - messageSize.Height - 5));

        }

        #endregion

        // Evento que controla el doble click
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            _isCharging = !_isCharging; // Controla el estado de carga
        }

        // Evento que controla el click
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            // Controla que no se esté cargando
            if (!_isCharging)
            {
                _isDragging = true;
                int newLevel = 100 - (e.Y * 100 / this.Height);
                ChargeLevel = newLevel;
            }
        }

        // Evento que controla el click cuando se suelta
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isDragging = false;
        }

        // Evento que controla el movimiento del ratón
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                // Actualiza el nivel de carga
                int newLevel = 100 - (e.Y * 100 / this.Height);
                ChargeLevel = newLevel;
            }
        }
    }
    #endregion
}