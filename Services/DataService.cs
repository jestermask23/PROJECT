using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictiveMarketing.App.Services
{
    public class DataService
    {
        public Task<DataTable> LoadDataAsync(string path, int? takeRows = null)
        {
            var table = new DataTable("Data");
            return Task.FromResult(table);

        }
    }
}