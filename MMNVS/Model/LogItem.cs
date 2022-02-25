using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public enum OperationTypeEnum
    {
        [Display(Name = "Neznámý")]
        Unknown,
        VMsStartBegin,
        VMsShutdownBegin,
        [Display(Name = "Start VM")]
        VMStartBegin,
        VMStartEnd,
        [Display(Name = "Vypínání VM")]
        VMShutdownBegin,
        VMShutdownEnd,
        [Display(Name = "Všechny VM vypnuty")]
        VMsOff,
        HostShutdownBegin,
        HostShutdownEnd,
        HostStartupBegin,
        HostStartupEnd,
        VSAShutdownBegin,
        VSAShutdownEnd,
        VSAStartupBegin,
        VSAStartupEnd,
        DatastoreCheckBegin,
        DatastoreCheckEnd,
        vCenterStartBegin,
        vCenterStartEnd,
        vCenterShutdownBegin,
        vCenterShutdownEnd,
        ChangeSystemState
    }
    public class LogItem
    {
        [Key]
        public int Id { get; set; }
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
        public SystemStateEnum SystemState { get; set; }
    }
}
