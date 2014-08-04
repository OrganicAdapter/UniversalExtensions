using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniversalExtensions
{
    public class HttpClientService
    {
        #region Fields



        #endregion //Fields

        #region Properties

        public HttpClient Client { get; set; }
        public string UriBase { get; set; }

        #endregion //Properties

        #region Constructor

        public HttpClientService(string uriBase)
        {
            Client = new HttpClient();
            UriBase = uriBase;
        }

        #endregion //Constructor

        #region Methods

        public async Task<string> GetRequest(string uri)
        {
            var str = await Client.GetStringAsync(new Uri(UriBase + uri, UriKind.RelativeOrAbsolute));
            return str;
        }

        #endregion //Methods
                
    }
}
