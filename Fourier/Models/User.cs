using Microsoft.AspNetCore.Identity;

namespace Fourier.Models;

public class User : IdentityUser<Guid>
{
    public string HashedPassword { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<Problem> Tasks { get; set; } = new List<Problem>();
}