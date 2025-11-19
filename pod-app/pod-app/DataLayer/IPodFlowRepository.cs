using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pod_app.Models;

namespace pod_app.DataLayer
{
    public interface IPodFlowRepository
    {
        Task<bool> PodExistsAsync(string id);

        Task<List<PodFlow>> GetAllPodFlowsAsync();

        // C
        Task AddNewPodAsync(PodFlow pod);
        // R
        Task<PodFlow?> GetPodFlowAsync(string id);
        // U
        Task<bool> UpdatePodFlowAsync(PodFlow updatedFlow);
        // D
        Task DeletePodFlowAsync(string id);
    }
}
