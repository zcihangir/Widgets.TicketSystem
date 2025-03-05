using Grand.Domain;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Tüm departmanları getirir
        /// </summary>
        Task<IPagedList<Department>> GetAllDepartments(
            string storeId = "",
            bool? active = null,
            string name = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue);
            
        /// <summary>
        /// Departman getirir
        /// </summary>
        Task<Department> GetDepartmentById(string id);
        
        /// <summary>
        /// Departman ekler
        /// </summary>
        Task InsertDepartment(Department department);
        
        /// <summary>
        /// Departman günceller
        /// </summary>
        Task UpdateDepartment(Department department);
        
        /// <summary>
        /// Departman siler
        /// </summary>
        Task DeleteDepartment(Department department);
    }
} 