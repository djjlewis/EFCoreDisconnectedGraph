using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFUpdateGraph
{
    public class CustomFloorDbSet : DbSet<Floor>
    {
        public override Floor Find(params object[] keyValues)
        {
            return base.Find(keyValues);
        }
    }
}