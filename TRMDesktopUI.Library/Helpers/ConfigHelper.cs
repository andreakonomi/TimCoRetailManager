using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Helpers
{
    /// <summary>
    /// Handles the interactions with the configurations file of the application.
    /// </summary>
    public class ConfigHelper : IConfigHelper
    {
        /// <summary>
        /// Gets the tax rate from the confiugrations of the application.
        /// </summary>
        /// <returns>the tax rate from the settings, or throws exception if value
        /// is not setup correctly.</returns>
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidtaxRate = decimal.TryParse(rateText, out decimal output);

            return !isValidtaxRate ? throw new ConfigurationErrorsException("The tax rate is not setup properly!") : output;
        }
    }
}
