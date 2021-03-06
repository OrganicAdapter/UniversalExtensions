﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalExtensions.Navigation
{
    public delegate void NavigationCompletedEventHandler(object sender, object param);

    public interface INavigationService
    {
        void Navigate(string project, string page);
        void Navigate(string project, string page, object param);
        void GoBack();

        event NavigationCompletedEventHandler NavigationComplatedEvent;
    }
}
