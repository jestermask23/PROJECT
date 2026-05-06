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
            string extension = System.IO.Path.GetExtension(path).ToLowerInvariant();

            return extension switch
            {
                ".csv" => Task.Run(() => LoadCsv(path, takeRows)),
                ".xlsx" => Task.FromException<DataTable>(
                    new NotSupportedException("XLSX parsing will be implemented later (use CSV for now).")),
                _ => Task.FromException<DataTable>(
                    new NotSupportedException($"Unsupported file type: {extension}"))
            };
        }

        private DataTable LoadCsv(string path, int? takeRows)
        {
            var dt = new DataTable("Data");
            using var reader = new StreamReader(path, Encoding.UTF8, true);

            if (reader.EndOfStream)
                return dt;

            string? headerLine = reader.ReadLine();
            if (headerLine == null)
                return dt;

            string[] headers = SplitCsvLine(headerLine);
            foreach (string h in headers)
            {
                string colName = string.IsNullOrWhiteSpace(h) ? "Column" + (dt.Columns.Count + 1) : h.Trim();
                dt.Columns.Add(colName);
            }

            int rowCount = 0;
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null)
                    break;

                string[] values = SplitCsvLine(line);
                var row = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row[i] = i < values.Length ? values[i] : null!;
                }

                dt.Rows.Add(row);
                rowCount++;

                if (takeRows.HasValue && rowCount >= takeRows.Value)
                    break;
            }

            return dt;
        }

        private static string[] SplitCsvLine(string line)
        {
            var result = new System.Collections.Generic.List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            result.Add(sb.ToString());
            return result.ToArray();
        }
    }
}