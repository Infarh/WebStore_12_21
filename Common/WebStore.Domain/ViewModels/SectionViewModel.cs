﻿namespace WebStore.ViewModels;

public class SectionViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Order { get; set; }

    public SectionViewModel? Parent { get; set; }

    public List<SectionViewModel> ChildSections { get; set; } = new();
}