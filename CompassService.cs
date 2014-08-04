using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace UniversalExtensions
{
    public delegate void CompassValueChangedEventHandler(object sender, double value);
    public delegate void CompassCalibrationStatusChangedEventHandler(object sender, bool isCalibrating);

    public class CompassService
    {
        #region Events

        public event CompassValueChangedEventHandler CompassValueChangedEvent;

        #endregion //Events

        #region Properties

        private Compass Compass { get; set; }
        private double TrueHeading { get; set; }

        #endregion //Properties

        #region Constructor

        public CompassService()
        {
            Compass = Compass.GetDefault();
        }

        #endregion //Constructor

        #region Methods

        /// <summary>
        /// Checks if compass is supported on your device.
        /// </summary>
        /// <returns>Compass support</returns>
        public bool CheckCompassSupport()
        {
            return (Compass == null) ? false : true;
        }

        /// <summary>
        /// Starts the compass.
        /// </summary>
        /// <param name="timeSpan">The refresh rate of the compass</param>
        /// <exception cref="NotSupportedException">Thrown when compass is not supported on the device.</exception>
        public void StartCompass(int timeSpan)
        {
            try
            {
                if (Compass == null)
                {
                    throw new NotSupportedException("Compass is not supported on your device.");
                }

                Compass.ReportInterval = (uint)timeSpan;
                Compass.ReadingChanged += ReadingChanged;
            }

            catch
            { 
            
            }
        }

        private void ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            TrueHeading = args.Reading.HeadingTrueNorth.Value;
            CompassValueChangedEvent(this, TrueHeading);
        }

        #endregion //Methods
    }
}
