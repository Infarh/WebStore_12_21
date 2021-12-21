using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components;

//[ViewComponent(Name = "sections")]
public class SectionsViewComponent : ViewComponent
{
    private readonly IProductData _ProductData;

    public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;

    public IViewComponentResult Invoke()
    {
        var sections = _ProductData.GetSections();

        var parent_sections = sections.Where(s => s.ParentId is null);

        var parent_sections_views = parent_sections
           .Select(s => new SectionViewModel
            {
               Id = s.Id,
               Name = s.Name,
               Order = s.Order,
            })
           .ToList();

        foreach (var parent_section in parent_sections_views)
        {
            var childs = sections.Where(s => s.ParentId == parent_section.Id);

            foreach (var child_section in childs)
                parent_section.ChildSections.Add(new SectionViewModel
                {
                    Id = child_section.Id,
                    Name = child_section.Name,
                    Order = child_section.Order,
                    Parent = parent_section
                });

            parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
        }

        parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

        return View(parent_sections_views);
    }
}