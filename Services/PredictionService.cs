using System.Data;
using System.Threading.Tasks;

namespace PredictiveMarketing.App.Services
{

    public class PredictionService
    {
        public Task TrainAsync(DataTable data, string targetColumnName)
        {

            return Task.CompletedTask;
        }

        public Task<(double Rmse, double Mae, double R2)> EvaluateAsync()
        {

            return Task.FromResult((double.NaN, double.NaN, double.NaN));
        }
    }
}