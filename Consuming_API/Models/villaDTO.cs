using System.ComponentModel.DataAnnotations;

namespace Consuming_API.Models
{
    public class villaDTO
    {
        public int VillaId { get; set; }

        [Required]
        [MaxLength(30)]
        public string VillaName { get; set; }
        public string VillaDetails { get; set; }
        [Required]
        public double VillaRate { get; set; }
        public int VillaOccupancy { get; set; }
        public int VillaSqft { get; set; }
        public string VillaImageURL { get; set; }
        public string VillaAmenity { get; set; }
    }
}

