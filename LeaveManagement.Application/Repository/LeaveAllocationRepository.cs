using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeaveManagement.Common.Constants;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Data;
using LeaveManagement.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application.Repository
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Employee> userManager;
        private readonly ILeaveTypeRepository leaveTypeRepository;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;

        public LeaveAllocationRepository(ApplicationDbContext context,
            UserManager<Employee> userManager,
            ILeaveTypeRepository leaveTypeRepository,
            AutoMapper.IConfigurationProvider configurationProvider,
            IEmailSender emailSender,
            IMapper mapper) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.leaveTypeRepository = leaveTypeRepository;
            this.configurationProvider = configurationProvider;
            this.emailSender = emailSender;
            this.mapper = mapper;
        }

        public async Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period)
        {
            return await context.LeaveAllocations.AnyAsync(q => q.EmployeeId == employeeId
                                                                && q.LeaveTypeId == leaveTypeId
                                                                && q.Period == period);
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string employeeId)
        {
            //var allocations = await context.LeaveAllocations.Include(q => q.LeaveType).Where(q => q.EmployeeId.Equals(employeeId))
            //    .ToListAsync();
            var allocations=await context.LeaveAllocations
                .Include(q=>q.LeaveType)
                .Where(q=>q.EmployeeId.Equals(employeeId))
                .ProjectTo< LeaveAllocationVM>(configurationProvider)
                .ToListAsync();
            var employee=await userManager.FindByIdAsync(employeeId);
            var employeeAllocationVM=mapper.Map<EmployeeAllocationVM>(employee);
            //by using mapper
            //employeeAllocationVM.LeaveAllocations = mapper.Map<List<LeaveAllocationVM>>(allocations);
            employeeAllocationVM.LeaveAllocations = allocations;
            return employeeAllocationVM;

            //var allocations2 = await context.LeaveAllocations
            // .Include(q => q.LeaveType)
            // .Where(q => q.EmployeeId.Equals(employeeId))
            // .Select(q => new LeaveAllocationVM { LeaveType = q.LeaveType })
            // .ToListAsync();
            //employeeAllocationVM.LeaveAllocations = allocations2;

        }
        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int id)
        {
            var allocation = await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .ProjectTo<LeaveAllocationEditVM>(configurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if(allocation==null) return null;


            var employee = await userManager.FindByIdAsync(allocation.EmployeeId);
            //var model=mapper.Map<LeaveAllocationEditVM>(allocation);
            var model=allocation;
            model.Employee=mapper.Map<EmployeeListVM>(employee);
            return model;
          

        }

        public async Task LeaveAllocation(int leaveTypeId)
        {
            var employees =await userManager.GetUsersInRoleAsync(Roles.User);
            var period = DateTime.Now.Year;
            var leaveType = await leaveTypeRepository.GetAsync(leaveTypeId);
            var allocations = new List<LeaveAllocation>();
            var allocatedEmployeess = new List<Employee>();

            foreach(var employee in employees)
           {
                if(!await AllocationExists(employee.Id, leaveTypeId, period))
                {
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveTypeId,
                        Period = period,
                        NumberOfDays = leaveType.DefaultDays
                    });
                    allocatedEmployeess.Add(employee);
                } 
                
            }

            await AddRangeAsync(allocations);

            foreach(var employee in allocatedEmployeess)
            {
              await emailSender.SendEmailAsync(employee.Email, $"Leave Request Posted for {period}", $"Your {leaveType.Name} " +
              $"has been posted for the period of {period}. You have been given {leaveType.DefaultDays}.");
            }
        }

        public async Task<LeaveAllocation?> GetEmployeeAllocation(string employeeId, int leaveTypeId)
        {
           return await context.LeaveAllocations.FirstOrDefaultAsync(q=>q.EmployeeId==employeeId
           && q.LeaveTypeId==leaveTypeId);
        }

        public async Task<bool> UpdateEmployeeAllocation(LeaveAllocationEditVM model)
        {
            var leaveAllocation = await GetAsync(model.Id);
            if (leaveAllocation == null) return false;

            leaveAllocation.Period = model.Period;
            leaveAllocation.NumberOfDays = model.NumberOfDays;
            await UpdateAsync(leaveAllocation);

            var leaveType=await leaveTypeRepository.GetAsync(model.LeaveTypeId);
            var user =await userManager.FindByIdAsync(leaveAllocation.EmployeeId);
            await emailSender.SendEmailAsync(user.Email, $"Your {leaveType.Name} Allocation Updated for {leaveAllocation.Period}", 
            $"Please review your leave allocation. Your {leaveType.Name} has been updated to {leaveAllocation.NumberOfDays}.");

            return true;
        }
    }
}
