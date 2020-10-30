using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Film : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
