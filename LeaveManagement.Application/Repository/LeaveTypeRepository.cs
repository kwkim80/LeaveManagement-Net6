using LeaveManagement.Application.Contracts;
using LeaveManagement.Data;

namespace LeaveManagement.Application.Repository
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
