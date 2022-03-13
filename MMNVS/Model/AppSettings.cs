using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public enum SystemStateEnum
    {
        [Display(Name = "Neznámý")]
        Unknown,
        [Display(Name = "Spuštěno")]
        Running,
        [Display(Name = "Spouštění")]
        Startup,
        [Display(Name = "Vypínání")]
        Shutdown,
        [Display(Name = "Přerušen start/vypínání")]
        Interrupt,
        [Display(Name = "Vypnuto")]
        Off
    }

    public enum vCenterVersionEnum
    {
        [Display(Name = "Verze 6.6 a nižší")]
        v65,
        [Display(Name = "Verze 6.7 a vyšší")]
        v67
    }
    public class AppSettings
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Název")]
        public string? Name { get; set; }
        [Display(Name = "Email administrátora")]
        public string? AdministratorEmail { get; set; }
        [Display(Name = "SMTP server")]
        public string? SmtpServer { get; set; }
        [Display(Name = "SMTP uživatel")]
        public string? SmtpUser { get; set; }
        [Display(Name = "SMTP heslo")]
        public string? SmtpPassword { get; set; }
        [Display(Name = "SMTP port")]
        public int? SmtpPort { get; set; }
        [Display(Name = "Zabezpečený přenos SMTP")]
        public bool SmtpIsSecure { get; set; }
        [Display(Name = "Primární UPS")]
        public int? PrimaryUPSId { get; set; }
        [ForeignKey("PrimaryUPSId")]
        public UPS? PrimaryUPS { get; set; }
        [Display(Name = "Název vCenter")]
        public string? vCenterIP{ get; set; }
        [Display(Name = "Uživatel vCenter")]
        public string? vCenterUsername { get; set; }
        [Display(Name = "Heslo k vCenter")]
        public string? vCenterPassword { get; set; }
        [Display(Name = "API url vCenter")]
        public string? vCenterApiUrl { get; set; }
        [Display(Name = "Verze vCenter")]
        public vCenterVersionEnum vCenterVersion { get; set; }
        [Display(Name = "Čas čekání pro kontrolu stavu u VM")]
        public int DelayTime { get; set; }
        [Display(Name = "Čas čekání pro kontrolu stavu u hostů (s)")]
        public int DelayTimeHosts { get; set; }
        [Display(Name = "Čas čekání pro kontrolu stavu u datastorů (s)")]
        public int DelayTimeDatastores { get; set; }
        [Display(Name = "Čas čekání při spouštění VM (s)")]
        public int DelayTimeVMStart { get; set; }
        [Display(Name = "Minimální výdrž baterie pro start (s)")]
        public int MinBatteryTimeForStart { get; set; } //Minimální výdrž baterie pro bezpečný start v sec
        [Display(Name = "Výdrž baterie pro zahájení vypínání (s)")]
        public int MinBatteryTimeForShutdown { get; set; } //Minimální výdrž baterie pro bezpečné vypnutí v sec
        [Display(Name = "Výdrž baterie pro zahájení vypínání hostů (s)")]
        public int BatteryTimeForShutdownHosts { get; set; }
        public SystemStateEnum SystemState { get; set; }
    }
}
