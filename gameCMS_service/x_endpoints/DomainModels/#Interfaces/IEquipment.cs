using x_endpoints.Enums;

namespace x_endpoints.DomainModels._Interfaces
{
    public interface IEquipment
        {
            int LevelRequirement { get; set; }
            EquipmentSlot Slot { get; }
        }
}
