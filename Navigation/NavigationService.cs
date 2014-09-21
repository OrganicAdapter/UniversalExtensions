using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalExtensions.Navigation
{
    public class NavigationService : INavigationService
    {
        #region Events

        public event NavigationCompletedEventHandler NavigationComplatedEvent;

        #endregion //Events

        #region Fields

        private readonly ITypeService _typeService;

        #endregion //Fields

        #region Properties

        private Frame RootFrame { get; set; }

        #endregion //Properties

        #region Constructor

        public NavigationService(ITypeService typeService)
        {
            _typeService = typeService;

            RootFrame = Window.Current.Content as Frame;
        }

        #endregion //Constructor

        #region Methods

        public void Navigate(string project, string page)
        {
            var type = _typeService.GetType(project + "." + page);
            RootFrame.Navigate(type);
            RootFrame.Navigated += Navigated;
        }

        private void Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (NavigationComplatedEvent != null)
                NavigationComplatedEvent(this, e.Parameter);
        }

        public void Navigate(string project, string page, object param)
        {
            var type = _typeService.GetType(project + "." + page);
            RootFrame.Navigate(type, param);
        }

        public void GoBack()
        {
            RootFrame.GoBack();
        }

        #endregion //Methods
    }
}
