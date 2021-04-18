using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Models.Manage
{
    public class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }

    }
}
