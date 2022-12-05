using System.Diagnostics;
using System.Threading.Tasks;

namespace Squarl.Models;

public class CustomProcess
{
    public CustomProcess(string? name, nint handle)
    {
        Name = name;
        Handle = handle;
    }
    private string? Name { get; set; }
    private nint Handle { get; set; }
    
    
}