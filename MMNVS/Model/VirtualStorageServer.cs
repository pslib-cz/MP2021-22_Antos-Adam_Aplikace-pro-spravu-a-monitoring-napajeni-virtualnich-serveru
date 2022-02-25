using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public class VirtualStorageServer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Unikátní název")]
        public string? Name { get; set; }
        //[Display(Name = "IP adresa")]
        //public string? IPAddress { get; set; }
        [Display(Name = "Host")]
        public HostServer? Host { get; set; }
        [ForeignKey("Host")]
        public int? HostId { get; set; }
        [Display(Name = "Pořadí při startu/vypínání")]
        public int? Order { get; set; }
        [Display(Name = "Poznámka")]
        public string? Notes { get; set; }
        public ICollection<Datastore> Datastores { get; set; }
        public ICollection<LogItem>? Log { get; set; }
    }
}
