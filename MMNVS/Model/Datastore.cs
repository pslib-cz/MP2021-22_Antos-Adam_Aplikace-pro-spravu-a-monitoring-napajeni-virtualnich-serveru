using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public class Datastore
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Unikátní název")]
        public string? Name { get; set; }
        [Display(Name = "Poznámka")]
        public VirtualStorageServer? VirtualStorageServer { get; set; }
        [ForeignKey("VirtualStorageServer")]
        public int? VirtualStorageServerId { get; set; }
        public string? Notes { get; set; }
        public ICollection<LogItem>? Log { get; set; }
    }
}
