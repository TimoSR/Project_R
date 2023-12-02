using MongoDB.Driver;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.Enums;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class ArmorService : BaseService<Armor>
{
    public ArmorService(MongoDbService dbService) : base(dbService, "Armors")
    {
    }
    public virtual async Task<List<Armor>> GetAllLegsAsync()
    {
        return await _collection.Find(p => p.Slot == EquipmentSlot.Legs).ToListAsync();
    }
   
    public virtual async Task<List<Armor>> GetAllHeadsAsync()
    {
        return await _collection.Find(p => p.Slot == EquipmentSlot.Head).ToListAsync();
    }
    public virtual async Task<List<Armor>> GetAllChestAsync()
    {
        return await _collection.Find(p => p.Slot == EquipmentSlot.Chest).ToListAsync();
    }
  
}  