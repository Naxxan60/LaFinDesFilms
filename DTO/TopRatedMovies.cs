using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class TopRatedMovie : BaseEntity
    {
        [MaxLength(20)]
        public string Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public int NbVote { get; set; }
    }
}
