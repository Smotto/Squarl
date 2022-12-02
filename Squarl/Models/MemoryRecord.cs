using System.Collections.ObjectModel;

namespace Squarl.Models;

public class MemoryRecord
{
    public string? address { get; set; }
    public string? value { get; set; }
    public string? previousValue { get; set; }
    public string? label { get; set; }

    public ObservableCollection<MemoryRecord> MemoryRecords { get; } = new();
}