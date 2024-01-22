using System.ComponentModel.DataAnnotations;

namespace HealthObserver.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


    }
}
