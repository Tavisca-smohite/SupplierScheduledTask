

using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public interface IUpdateFaresourcesConfig
    {
        bool DisableSupplier(int supplierId);
        bool EnableSupplier(int supplierId);
    }
}
