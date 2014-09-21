using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace UniversalExtensions.PushNotification
{
    public delegate void PushNotificationReceavedEventHandler(PushNotificationChannel sender, PushNotificationReceivedEventArgs args);

    public class PushNotificationService
    {
        public event PushNotificationReceavedEventHandler PushNotificationReceivedEvent;

        private Notifier Notifier { get; set; }
        private PushNotificationChannel Channel { get; set; }
        private string ChannelUri { get; set; }


        public PushNotificationService(string channelUri)
        {
            ChannelUri = channelUri;

            Notifier = new Notifier();
        }


        public async void OpenChannel()
        {
            try
            {
                ChannelAndWebResponse channelAndWebResponse = await Notifier.OpenChannelAndUploadAsync(ChannelUri);
                Channel = channelAndWebResponse.Channel;
                Channel.PushNotificationReceived += PushNotificationReceived;
            }
            catch
            {
                
            }
        }

        private void PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            if (PushNotificationReceivedEvent != null)
                PushNotificationReceivedEvent(sender, args);
        }

        public void CloseChannel()
        {
            if (Channel != null)
            {
                Channel.PushNotificationReceived -= PushNotificationReceived;
                Channel.Close();
                Channel = null;
            }
            else
            {
            }
        }
    }
}
