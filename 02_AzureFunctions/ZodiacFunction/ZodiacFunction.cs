using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ZodiacFunction
{
    public static class ZodiacFunction
    {
        [FunctionName("GetSign")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string dateOfBirthQuery = req.Query["dateOfBirth"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            DateTime dateOfBirth = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateOfBirthQuery) &&
                !DateTime.TryParse(dateOfBirthQuery, out dateOfBirth))
            {
                return new BadRequestObjectResult("Invalid date of birth");
            }

            if (!string.IsNullOrEmpty(requestBody) &&
                !DateTime.TryParse(requestBody, out dateOfBirth))
            {
                return new BadRequestObjectResult("Invalid date of birth");
            }

            log.LogInformation($"date of birth: {dateOfBirth}");

            ZodiacSign sign = GetSign(dateOfBirth);
            return new OkObjectResult(sign.ToString());
        }

        private static ZodiacSign GetSign(DateTime dateOfBirth)
        {
            if (dateOfBirth.Day <= 20 && dateOfBirth.Month <= 1)
            {
                return ZodiacSign.Capricornus;
            }
            if (dateOfBirth.Month <= 1 || dateOfBirth.Day <= 20 && dateOfBirth.Month <= 2)
            {
                return ZodiacSign.Aquarius;
            }
            if (dateOfBirth.Month <= 2 || dateOfBirth.Day <= 20 && dateOfBirth.Month <= 3)
            {
                return ZodiacSign.Pisces;
            }
            if (dateOfBirth.Month <= 3 || dateOfBirth.Day <= 20 && dateOfBirth.Month <= 4)
            {
                return ZodiacSign.Aries;
            }
            if (dateOfBirth.Month <= 4 || dateOfBirth.Day <= 20 && dateOfBirth.Month <= 5)
            {
                return ZodiacSign.Taurus;
            }
            if (dateOfBirth.Month <= 5 || dateOfBirth.Day <= 21 && dateOfBirth.Month <= 6)
            {
                return ZodiacSign.Gemini;
            }
            if (dateOfBirth.Month <= 6 || dateOfBirth.Day <= 22 && dateOfBirth.Month <= 7)
            {
                return ZodiacSign.Cancer;
            }
            if (dateOfBirth.Month <= 7 || dateOfBirth.Day <= 22 && dateOfBirth.Month <= 8)
            {
                return ZodiacSign.Leo;
            }
            if (dateOfBirth.Month <= 8 || dateOfBirth.Day <= 22 && dateOfBirth.Month <= 9)
            {
                return ZodiacSign.Virgo;
            }
            if (dateOfBirth.Month <= 9 || dateOfBirth.Day <= 23 && dateOfBirth.Month <= 10)
            {
                return ZodiacSign.Libra;
            }
            if (dateOfBirth.Month <= 10 || dateOfBirth.Day <= 22 && dateOfBirth.Month <= 11)
            {
                return ZodiacSign.Scorpius;
            }
            if (dateOfBirth.Month <= 11 || dateOfBirth.Day <= 21 && dateOfBirth.Month <= 12)
            {
                return ZodiacSign.Sagittarius;
            }

            return ZodiacSign.Capricornus;
        }

        private enum ZodiacSign
        {
            Aries,
            Taurus,
            Gemini,
            Cancer,
            Leo,
            Virgo,
            Libra,
            Scorpius,
            Sagittarius,
            Capricornus,
            Aquarius,
            Pisces
        }

        public class ZodiacSignResponse
        {
            public string? Sign { get; init; }
        }
    }
}
