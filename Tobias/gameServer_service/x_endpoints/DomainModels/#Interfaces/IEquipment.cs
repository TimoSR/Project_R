using x_endpoints.Enums;

namespace x_endpoints.Interfaces
{
    public interface IEquipment
        {
            string Id {get;}
            string Name { get; }
            int LevelRequirement { get; }
            EquipmentSlot Slot { get; }
        }
}
