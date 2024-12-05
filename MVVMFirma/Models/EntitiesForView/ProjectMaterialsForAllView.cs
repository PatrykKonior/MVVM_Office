using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ProjectMaterialsForAllView
    {
        public int ProjectMaterialID { get; set; }

        public string ProjectsName { get; set; }
        public string MaterialsName { get; set; }

        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? VATAmount { get; set; }
    }
}
