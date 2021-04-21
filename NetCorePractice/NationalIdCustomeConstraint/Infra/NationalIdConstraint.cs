using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalIdCustomeConstraint.Infra
{
    public class NationalIdConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            var nationalId = values[routeKey].ToString();
            if (string.IsNullOrWhiteSpace(nationalId) ||
                nationalId.Length<10 ||
                !long.TryParse(nationalId, out long lngNationalId))
            {
                return false;
            }

            if (lngNationalId % 1111111111 == 0) return false;

            int intSum = 0;

            for (var i = 0; i < 9; i++)
                intSum += (nationalId[i] - '0') * (10 - i);

            var intMod = intSum % 11;

            if (intMod < 2) return nationalId[9] - '0' == intMod;

            if (intMod >= 2) return 11 - intMod == nationalId[9] - '0';

            return false;
        }
    }
}
