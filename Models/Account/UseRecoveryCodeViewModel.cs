using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Models.Account
{
    public class UseRecoveryCodeViewModel
    {
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}
