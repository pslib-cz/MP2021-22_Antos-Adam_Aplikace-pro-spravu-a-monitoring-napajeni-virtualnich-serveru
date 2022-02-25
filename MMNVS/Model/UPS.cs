 using System.ComponentModel.DataAnnotations;

namespace MMNVS.Model
{
    public class UPS
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Název")]
        public string? Name { get; set; } //Název UPS
        [Display(Name = "Model")]
        public string? Model { get; set; } //Model
        [Display(Name = "Výrobce")]
        public string? Producer { get; set; }
        [Display(Name = "IP Adresa")]
        public string? IPAddress { get; set; }
        [Display(Name = "SNMP OID Stav")]
        public string? SNMPOIDState { get; set; } //Stav napájení ze sítě / z baterie
        [Display(Name = "SNMP OID Kapacita baterie")]
        public string? SNMPOIDBatteryCapacity { get; set; }
        [Display(Name = "SNMP OID Čas na baterii")]
        public string? SNMPOIDBatteryTime { get; set; } //Zbývající čas provozu na baterie
        [Display(Name = "SNMP OID Vytížení")]
        public string? SNMPOIDLoad { get; set; }
        [Display(Name = "Poznámky")]
        public string? Notes { get; set; }
        public ICollection<UPSLogItem>? UPSLog { get; set; }
        public ICollection<LogItem>? Log { get; set; }
        public ICollection<AppSettings>? AppSettings { get; set; }
    }
}
