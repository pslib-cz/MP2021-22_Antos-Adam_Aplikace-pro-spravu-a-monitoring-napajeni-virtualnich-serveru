using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public enum OperationTypeEnum
    {
        [Display(Name = "Neznámý")]
        Unknown,
        [Display(Name = "Start všech VM začátek")]
        VMsStartBegin,
        [Display(Name = "Vypínání všech VM začátek")]
        VMsShutdownBegin,
        [Display(Name = "Start VM začátek")]
        VMStartBegin,
        [Display(Name = "Start VM konec")]
        VMStartEnd,
        [Display(Name = "Vypínání VM začátek")]
        VMShutdownBegin,
        [Display(Name = "Vypínání VM konec")]
        VMShutdownEnd,
        [Display(Name = "Všechny VM vypnuty")]
        VMsOff,
        [Display(Name = "Vypínání hosta začátek")]
        HostShutdownBegin,
        [Display(Name = "Vypínání hosta konec")]
        HostShutdownEnd,
        [Display(Name = "Start hosta začátek")]
        HostStartupBegin,
        [Display(Name = "Start hosta konec")]
        HostStartupEnd,
        [Display(Name = "Vypínání VSA začátek")]
        VSAShutdownBegin,
        [Display(Name = "Vypínání VSA konec")]
        VSAShutdownEnd,
        [Display(Name = "Start VSA začátek")]
        VSAStartupBegin,
        [Display(Name = "Start VSA konec")]
        VSAStartupEnd,
        [Display(Name = "Kontrola datastoru začátek")]
        DatastoreCheckBegin,
        [Display(Name = "Kontrola datastoru konec")]
        DatastoreCheckEnd,
        [Display(Name = "Start vCenter začátek")]
        vCenterStartBegin,
        [Display(Name = "Start vCenter konec")]
        vCenterStartEnd,
        [Display(Name = "Vypínání vCenter začátek")]
        vCenterShutdownBegin,
        [Display(Name = "Vypínání vCenter konec")]
        vCenterShutdownEnd,
        [Display(Name = "Změna stavu systému")]
        ChangeSystemState
    }
    public class LogItem
    {
        [Key]
        public int Id { get; set; }
        [Display (Name = "Datum a čas")]
        public DateTime DateTime { get; set; }
        [Display(Name = "Typ operace")]
        public OperationTypeEnum OperationType { get; set; }
        public UPS? UPS { get; set; }
        [ForeignKey("UPS")]
        [Display(Name = "UPS")]
        public int? UPSId { get; set; }
        [Display(Name = "Host server")]
        public HostServer? HostServer { get; set; }
        [ForeignKey("HostServer")]
        [Display(Name = "Host server")]
        public int? HostServerId { get; set; }
        [Display(Name = "Virtuální server")]
        public VirtualServer? VirtualServer { get; set; }
        [ForeignKey("VirtualServer")]
        [Display(Name = "Virtuální server")]
        public string? VirtualServerVMId { get; set; }

        [Display(Name = "Virtuální storage server")]
        public VirtualStorageServer? VirtualStorageServer { get; set; }
        [ForeignKey("VirtualStorageServer")]
        [Display(Name = "Virtuální storage server")]
        public int? VirtualStorageServerId { get; set; }
        public Datastore? Datastore { get; set; }
        [ForeignKey("Datastore")]
        [Display(Name = "Datastore")]
        public int? DatastoreId { get; set; }
        [Display(Name = "Nový stav")]
        public SystemStateEnum SystemState { get; set; }
    }
}
