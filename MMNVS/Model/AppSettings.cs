using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMNVS.Model
{
    public enum SystemStateEnum
    {
        Unknown,
        Running,
        Startup,
        Shutdown,
        Interrupt,
        Off
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
        [Display(Name = "Čas čekání pro kontrolu stavu u VM")]
        public int DelayTime { get; set; }
        [Display(Name = "Čas čekání pro kontrolu stavu u hostů (s)")]
        public int DelayTimeHosts { get; set; }
        [Display(Name = "Čas čekání pro kontrolu stavu u datastorů (s)")]
        public int DelayTimeDatastores { get; set; }
        [Display(Name = "Čas čekání při spouštění VM (s)")]
        public int DelayTimeVMStart { get; set; }
        [Display(Name = "Minimální výdrž baterie pro start (s)")]
        public int MinBatteryTimeForStart { get; set; } //Minimální výdrž baterie pro bezpečný start VM v sec
        public SystemStateEnum SystemState { get; set; }
    }
}
