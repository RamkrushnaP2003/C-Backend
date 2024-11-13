using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using TaskExcelMongoDB.Models;
using TaskExcelMongoDB.Services.Interfaces;

namespace TaskExcelMongoDB.Services.Implementations
{
    public class FileProcessingService : IFileProcessingService
    {
        public async Task<List<User>> ProcessExcelFileAsync(IFormFile file)
        {
            var users = new List<User>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream())
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataset = reader.AsDataSet();
                if (dataset.Tables.Count > 0)
                {
                    var datatable = dataset.Tables[0];

                    for (int i = 1; i < datatable.Rows.Count; i++)
                    {
                        var row = datatable.Rows[i];
                        var user = new User
                        {
                            FullName = row[0]?.ToString() ?? string.Empty,
                            MobileNo = row[1]?.ToString() ?? string.Empty,
                            Address = row[2]?.ToString() ?? string.Empty,
                            Salary = decimal.TryParse(row[3]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var salary) ? salary : 0,
                            DateOfBirth = row[4]?.ToString() ?? string.Empty
                        };
                        users.Add(user);
                    }
                }
            }

            return await Task.FromResult(users);
        }
    }
}
