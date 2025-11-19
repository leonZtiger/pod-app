using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pod_app.Models;

namespace pod_app.DataLayer
{
    public interface IPodRepository
    {
        Task<bool> PodExistsAsync(string id);
        Task<List<string>> GetAllPodsAsync();

        // C
        Task AddNewPodAsync(PodModel pod);
        // R
        Task<PodModel?> GetPodAsync(string id);
        // U
        Task<bool> UpdatePodAsync(PodModel updatedPod);
        // D
        Task DeletePodAsync(string id);
    }
}
