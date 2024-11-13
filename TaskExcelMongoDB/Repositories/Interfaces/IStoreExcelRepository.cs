// IStoreExcelRepository Interface
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskExcelMongoDB.Models;

namespace TaskExcelMongoDB.Repositories.Interfaces
{
    public interface IStoreExcelRepository
    {
        Task<bool> InsertUsers(List<User> users);
    }
}
