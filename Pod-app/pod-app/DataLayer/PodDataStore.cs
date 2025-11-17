using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace pod_app.DataLayer
{
    internal class PodDataStore
        

{
        private readonly string _filePath = "savedPods.json";
        public List<PodModel> LoadSavedPods()
        {
            if (!File.Exists(_filePath))
                return new List<PodModel>();
            
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<PodModel>>(json) ?? new List<PodModel>();
        
        }

        public void SavePods(List<PodModel> pods)

        {
            string json = JsonSerializer.Serialize(
                pods,
                new JsonSerializerOptions { WriteIndented = true }
            );

            File.WriteAllText(_filePath, json);
        }



    }
}

