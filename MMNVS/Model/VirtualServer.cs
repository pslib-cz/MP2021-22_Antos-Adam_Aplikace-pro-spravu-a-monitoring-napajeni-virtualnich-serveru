using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public class VirtualServer
    {
        [Key]
        //public int Id { get; set; }
        [Display(Name = "VM id")]
        [JsonProperty("vm")]
        public string VMId { get; set; }
        [Display(Name = "Název")]
        [JsonProperty("name")]
        public string? Name { get; set; }
        //[Display(Name = "IP Adresa")]
        //public string? IPAddress { get; set; }
        //public HostServer? PreferedHost { get; set; }
        //[ForeignKey("PreferedHost")]
        //[Display(Name = "Host")]
        //public int? PreferedHostId { get; set; }
        [Display(Name = "Pořadí při startu/vypínání")]
        public int? Order { get; set; }
        [Display(Name = "Zapnout?")]
        public bool StartServerOnStart { get; set; }
        [Display(Name = "Poznámka")]
        public string? Notes { get; set; }
        public bool IsvCenter { get; set; }
        public ICollection<LogItem>? Log { get; set; }
    }
}
