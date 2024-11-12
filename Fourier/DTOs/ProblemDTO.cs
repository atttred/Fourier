using System.ComponentModel.DataAnnotations;

namespace Fourier.DTOs;

public class ProblemDto
{
    [Required]
    public int Input { get; set; }
}