using Grand.Domain;
using Grand.Data;
using Grand.Infrastructure.Caching;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields
        private readonly IRepository<Department> _departmentRepository;
        private readonly ICacheBase _cacheBase;
        #endregion
        
        #region Ctor
        public DepartmentService(
            IRepository<Department> departmentRepository,
            ICacheBase cacheBase)
        {
            _departmentRepository = departmentRepository;
            _cacheBase = cacheBase;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Tüm departmanları getirir
        /// </summary>
        public virtual async Task<IPagedList<Department>> GetAllDepartments(
            string storeId = "",
            bool? active = null,
            string name = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = from d in _departmentRepository.Table
                        select d;
                        
            if (!string.IsNullOrWhiteSpace(storeId))
            {
                query = query.Where(d => d.LimitedToStores);
            }
          
            if (active.HasValue)
            {
                query = query.Where(d => d.Active == active.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }
            
            query = query.OrderBy(d => d.DisplayOrder).ThenBy(d => d.Name);
            
            return await PagedList<Department>.Create(query, pageIndex, pageSize);
        }
        
        /// <summary>
        /// Departman getirir
        /// </summary>
        public virtual async Task<Department> GetDepartmentById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
                
            return await _departmentRepository.GetByIdAsync(id);
        }
        
        /// <summary>
        /// Departman ekler
        /// </summary>
        public virtual async Task InsertDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));
                
            department.CreatedOnUtc = DateTime.UtcNow;
            await _departmentRepository.InsertAsync(department);
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.DepartmentsPrefixCacheKey);
        }
        
        /// <summary>
        /// Departman günceller
        /// </summary>
        public virtual async Task UpdateDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));
                
            department.UpdatedOnUtc = DateTime.UtcNow;
            await _departmentRepository.UpdateAsync(department);
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.DepartmentsPrefixCacheKey);
        }
        
        /// <summary>
        /// Departman siler
        /// </summary>
        public virtual async Task DeleteDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));
                
            await _departmentRepository.DeleteAsync(department);
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.DepartmentsPrefixCacheKey);
        }
        #endregion
    }
} 