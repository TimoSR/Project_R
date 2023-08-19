
using x_endpoints.Enums;
using x_endpoints.Interfaces;

namespace x_endpoints.Models
{


    public class Armor : IEquipment
    {
        public string Id { get; }
        public string Name { get; set; }
        public int LevelRequirement { get; set; }

        public EquipmentSlot Slot { get; set; }

        public int ArmorValue { get; set; }

    }
}
 