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
    /// The class that interfaces with the product endpoint of the api from the ui layer.
    /// Bridge between Ui and Api.
    /// </summary>
    public class ProductEndpoint : IProductEndpoint
    {
        private IAPIHelper _apiHelper;
        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        /// <summary>
        /// Get List of all Products from the API. Have avoided direct access to dataaccess from the ui
        /// by consuming this api as a messenger for our data that we need.
        /// </summary>
        public async Task<List<ProductModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
