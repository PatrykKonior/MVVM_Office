using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Linq;

namespace MVVMFirma.Models.EntitiesForAdd
{
    public class MaterialsAdd : DatabaseClass
    {
        public MaterialsAdd(DesignOfficeEntities db) : base(db)
        {
        }

        public IQueryable<KeyAndValue> GetMaterialsKeyAndValueItems()
        {
            return db.Materials.Select(m => new KeyAndValue
            {
                Key = m.MaterialID,
                Value = m.MaterialName
            }).ToList().AsQueryable();
        }
    }
}
