using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    /// <summary>
    /// The endpoint for sending sale data to our consuming api.
    /// </summary>
    public class SaleEndpoint : ISaleEndpoint
    {
        private IAPIHelper _apiHelper;
        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        /// <summary>
        /// Sends the sale data to the api on a post command.
        /// </summary>
        /// <param name="sale">The sale data we want to post on the api.</param>
        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Log success call?
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
