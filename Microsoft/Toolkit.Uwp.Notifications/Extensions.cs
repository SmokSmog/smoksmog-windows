#if !WINDOWS_UWP

using Windows.Data.Xml.Dom;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    public static class Extensions
    {
        /// <summary>
        /// Retrieves the notification XML content as a WinRT XmlDocument, so that it can be used
        /// with a local Toast notification's constructor on either
        /// </summary>
        /// <returns>The notification XML content as a WinRT XmlDocument.</returns>
        public static XmlDocument GetXml(this ToastContent toastContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(toastContent.GetContent());
            return doc;
        }

        /// <summary>
        /// Retrieves the notification XML content as a WinRT XmlDocument, so that it can be used
        /// with a local Tile notification's constructor on either.
        /// </summary>
        /// <returns>The notification XML content as a WinRT XmlDocument.</returns>
        public static XmlDocument GetXml(this TileContent tileContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tileContent.GetContent());
            return doc;
        }
    }
}

#endif