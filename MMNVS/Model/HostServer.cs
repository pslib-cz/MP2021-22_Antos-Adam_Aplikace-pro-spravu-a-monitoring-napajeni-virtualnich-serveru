using System.ComponentModel.DataAnnotations;

namespace MMNVS.Model
{
    public class HostServer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Název")]
        public string? Name { get; set; }
        [Display(Name = "Výrobce")]
        public string? Producer { get; set; }
        [Display(Name = "iLo IP adresa")]
        public string? iLoIPAddress { get; set; }
        [Display(Name = "iLo uživatel")]
        public string? iLoUser { get; set; }
        [Display(Name = "iLo heslo")]
        public string? iLoPassword { get; set; }
        [Display(Name = "IP adresa serveru")]
        public string? ESXiIPAddress { get; set; }
        [Display(Name = "Uživatel serveru")]
        public string? ESXiUser { get; set; }
        [Display(Name = "Heslo k serveru")]
        public string? ESXiPassword { get; set; }
        [Display(Name = "Poznámky")]
        public string? Notes { get; set; }
        [Display(Name = "Má operační systém Windows")]
        public bool IsOSWindows { get; set; }
        public ICollection<VirtualStorageServer>? VirtualStorageServers { get; set; }
        public ICollection<LogItem>? Log { get; set; }
    }
}