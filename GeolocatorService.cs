using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace UniversalExtensions
{
    public delegate void LocationChangedEventHandler(object sender, double latitude, double longitude);
    public delegate void StatusChangedEventHandler(object sender, int status);

    /// <summary>
    /// Windows Phone 8 GeoCoordinateWatcher service for PCL.
    /// </summary>
    public class GeolocatorService
    {
        #region Events
        /// <summary>
        /// Raised when user location changed.
        /// </summary>
        public event LocationChangedEventHandler PositionChangedEvent;
        /// <summary>
        /// Raised when GeoCoordinateWatcherStatus has changed.
        /// 0: Initializing
        /// 1: Disabled
        /// 2: Not supported
        /// 3: No data
        /// 4: Ready
        /// </summary>
        public event StatusChangedEventHandler StatusChangedEvent;

        #endregion //Events

        #region Properties

        private Geolocator Locator { get; set; }
        private bool IsRunning { get; set; }

        #endregion //Properties

        #region Constructor

        public GeolocatorService()
        {
            Locator = new Geolocator();
            Locator.MovementThreshold = 20;

            Locator.PositionChanged += PositionChanged;
            Locator.StatusChanged += StatusChanged;
        }

        #endregion //Constructor

        #region Methods

        #region Public

        /// <summary>
        /// Starts receiving data from GeoCoordinateWatcher.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when location is not supported on the device.</exception>
        public void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            GetPosition();
        }

        /// <summary>
        /// Stops checking location.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Returns the distance of two points in kilometres.
        /// </summary>
        /// <param name="fromLat">Latitude of starting point</param>
        /// <param name="fromLong">Longitude of starting point</param>
        /// <param name="toLat">Latitude of end point</param>
        /// <param name="toLong">Longitude of end point</param>
        /// <returns></returns>
        public double GetDistance(double fromLat, double fromLong, double toLat, double toLong)
        {
            const double degreesToRadians = (Math.PI / 180.0);
            const double earthRadius = 6371; // kilometers

            // convert latitude and longitude values to radians
            var prevRadLat = fromLat * degreesToRadians;
            var prevRadLong = fromLong * degreesToRadians;
            var currRadLat = toLat * degreesToRadians;
            var currRadLong = toLong * degreesToRadians;

            // calculate radian delta between each position.
            var radDeltaLat = currRadLat - prevRadLat;
            var radDeltaLong = currRadLong - prevRadLong;

            // calculate distance
            var expr1 = (Math.Sin(radDeltaLat / 2.0) *
                         Math.Sin(radDeltaLat / 2.0)) +

                        (Math.Cos(prevRadLat) *
                         Math.Cos(currRadLat) *
                         Math.Sin(radDeltaLong / 2.0) *
                         Math.Sin(radDeltaLong / 2.0));

            var expr2 = 2.0 * Math.Atan2(Math.Sqrt(expr1),
                                         Math.Sqrt(1 - expr1));

            var distance = (earthRadius * expr2);
            return distance;  // return results as meters
        }

        #endregion //Public

        #region Private

        private async void GetPosition()
        {
            while (IsRunning)
            {
                await Locator.GetGeopositionAsync();
            }
        }

        private void PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (PositionChangedEvent == null || !IsRunning) return;

            PositionChangedEvent(this, args.Position.Coordinate.Point.Position.Latitude, args.Position.Coordinate.Point.Position.Longitude);
        }

        private void StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            if (!IsRunning) return;

            switch (args.Status)
            {
                case PositionStatus.Initializing:
                    StatusChangedEvent(this, 0);
                    break;

                case PositionStatus.Disabled:
                    StatusChangedEvent(this, 1);
                    break;

                case PositionStatus.NoData:
                    StatusChangedEvent(this, 3);
                    break;

                case PositionStatus.Ready:
                    StatusChangedEvent(this, 4);
                    break;
            }
        }

        #endregion //Private

        #endregion //Methods
    }
}
