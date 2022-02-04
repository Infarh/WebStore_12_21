using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

//[ViewComponent(Name = "sections")]
public class SectionsViewComponent : ViewComponent
{
    private readonly IProductData _ProductData;

    public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;

    public IViewComponentResult Invoke(string SectionId)
    {
        //SectionId = HttpContext.Request.Query["SectionId"];

        var section_id = int.TryParse(SectionId, out var id) ? id : (int?)null;

        var sections = GetSections(section_id, out var parent_section_id);

        return View(new SelectableSectionsViewModel
        {
            Sections = sections,
            SectionId = section_id,
            ParentSectionId = parent_section_id,
        });
    }

    private IEnumerable<SectionViewModel> GetSections(int? SectionId, out int? ParentSectionId)
    {
        ParentSectionId = null;

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
            {
                if (child_section.Id == SectionId)
                    ParentSectionId = child_section.ParentId;

                parent_section.ChildSections.Add(new SectionViewModel
                {
                    Id = child_section.Id,
                    Name = child_section.Name,
                    Order = child_section.Order,
                    Parent = parent_section
                });
            }

            parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
        }

        parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

        return parent_sections_views;
    }
}