﻿using System.Collections.ObjectModel;

namespace Squarl.Models;

public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    
    public ObservableCollection<Person> Children { get; } = new();
}