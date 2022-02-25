using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public enum UPSStateEnum
    {
        [Display(Name = "Neznámý")]
        Unknown = 0,
        [Display(Name = "Napájení ze sítě")]
        MainSupply = 1,
        [Display(Name = "Napájení z baterie")]
        Battery = 2
    }
    public class UPSLogItem
    {
        [Key]
        public int Id { get; set; }
        public UPS? UPS { get; set; }
        [ForeignKey("UPS")]
        public int? UPSId { get; set; }
        [Display (Name = "Čas a datum")]
        public DateTime DateTime { get; set; }
        [Display (Name = "Stav")]
        public UPSStateEnum State { get; set; }
        [Display (Name = "Kapacita baterie")]
        public int BatteryCapacity { get; set; }
        [Display (Name = "Zbývající čas")]   
        public int RemainingTime { get; set; }
        [Display (Name = "Zátěž")]
        public int Load { get; set; }
        public bool Error { get; set; }
    }
}
