using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nito.ConnectedProperties;
using Nito.ConnectedProperties.Explicit;

namespace IntegrationTest
{
    internal static class Program
    {
        static void Main(string[] args)
        {
        }

        public static string PreconditionViolationExceptionType()
        {
            try
            {
                var propertyDefinition = new PropertyConnector<object, dynamic>();
                propertyDefinition.GetProperty(null);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.GetType().ToString();
            }
        }
    }
}
