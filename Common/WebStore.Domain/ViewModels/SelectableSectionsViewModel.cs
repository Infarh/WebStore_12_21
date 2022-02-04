using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModels
{
    public class SelectableSectionsViewModel
    {
        public IEnumerable<SectionViewModel> Sections { get; set; }

        public int? SectionId { get; set; }

        public int? ParentSectionId { get; set; }
    }
}
