using System.ComponentModel.DataAnnotations;

namespace CloudPlatform.Tool.Configuration;

public class GeneralSettings
{

    [Required]
    [RegularExpression(@"[a-zA-Z0-9\s.\-\[\]]+", ErrorMessage = "Only letters, numbers, - and [] are allowed.")]
    [Display(Name = "Copyright")]
    [MaxLength(64)]
    public string Copyright { get; set; }


    [Required]
    [Display(Name = "OwnerName")]
    [MaxLength(32)]
    public string OwnerName { get; set; }
}
