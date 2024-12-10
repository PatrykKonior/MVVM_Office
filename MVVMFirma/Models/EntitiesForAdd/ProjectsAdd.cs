using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class ProjectsAdd : DatabaseClass
    {
        public ProjectsAdd(DesignOfficeEntities db) : base(db)
        {
        }

        public IQueryable<KeyAndValue> GetProjectsKeyAndValueItems()
        {
            return db.Projects.Select(p => new KeyAndValue
            {
                Key = p.ProjectID,
                Value = p.ProjectName
            }).ToList().AsQueryable();
        }
    }
}
