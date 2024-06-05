using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleClient.Models
{
    public class TaskModel
    {
        public string Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string ServiceId { get; set; }
        public bool IsCompleted { get; set; }
        public string AssignedTo { get; set; }
    }
}
