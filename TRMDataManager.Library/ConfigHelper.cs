using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library
{
    public static class ConfigHelper
    {
        /// <summary>
        /// This method replicates reading the taxRate from the conifg file. This time from web.config.
        /// </summary>
        /// <returns></returns>
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidtaxRate = decimal.TryParse(rateText, out decimal output);

            return !isValidtaxRate ? throw new ConfigurationErrorsException("The tax rate is not setup properly!") : output;
        }

    }
}
