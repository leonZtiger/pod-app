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

        Task<List<Podcast>> GetAllPodFlowsAsync();

        // C
        Task AddNewPodAsync(Podcast pod);
        // R
        Task<Podcast?> GetPodFlowAsync(string id);
        // U
        Task<bool> UpdatePodFlowAsync(Podcast updatedFlow);
        // D
        Task DeletePodFlowAsync(string id);
    }
}
