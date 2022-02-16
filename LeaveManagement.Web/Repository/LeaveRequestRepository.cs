﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Data;
using LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Web.Repository
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<Employee> userManager;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILeaveAllocationRepository leaveAllocationRepository;

        public LeaveRequestRepository(ApplicationDbContext context, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<Employee> userManager,
            AutoMapper.IConfigurationProvider configurationProvider,
            ILeaveAllocationRepository leaveAllocationRepository) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.configurationProvider = configurationProvider;
            this.leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task Cancelleaverequest(int leaverequestid)
        {
            var leaverequest = await GetAsync(leaverequestid);
            leaverequest.Cancelled=true;

            await UpdateAsync(leaverequest);

        }

        public async Task ChangeApprovalStatus(int leaveRequestId, bool approved)
        {
            var leaveRequest=await GetAsync(leaveRequestId);
            leaveRequest.Approved= approved;

            if (approved)
            {
                var allocation = await leaveAllocationRepository.GetEmployeeAllocation(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                allocation.NumberOfDays -= (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                await leaveAllocationRepository.UpdateAsync(allocation);
            }
            await UpdateAsync(leaveRequest);
        }

        public async Task<bool> CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor?.HttpContext?.User);

            var leaveAllocation = await leaveAllocationRepository.GetEmployeeAllocation(user.Id, model.LeaveTypeId);

            if (leaveAllocation == null)return false;
         

            int daysRequested = (int)(model.EndDate.Value - model.StartDate.Value).TotalDays;

            if (daysRequested > leaveAllocation.NumberOfDays)return false;


            var leaveRequest = mapper.Map<LeaveRequest>(model);
            leaveRequest.DateRequested = DateTime.Now;
            leaveRequest.RequestingEmployeeId = user.Id;

            await AddAsync(leaveRequest);

            //await emailSender.SendEmailAsync(user.Email, "Leave Request Submitted Successfully", $"Your leave request from " +
            //    $"{leaveRequest.StartDate} to {leaveRequest.EndDate} has been submitted for approval");

            return true;

        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            var leaveRequest = await context.LeaveRequests.Include(q => q.LeaveType).ToListAsync();
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests=leaveRequest.Count,
                ApprovedRequests=leaveRequest.Count(q=>q.Approved==true),
                PendingRequests=leaveRequest.Count(q=>q.Approved==null),
                RejectedRequests=leaveRequest.Count(q=>q.Approved==false),
                LeaveRequests=mapper.Map<List<LeaveRequestVM>>(leaveRequest)

            };

            foreach (var item in model.LeaveRequests)
            {
                item.Employee = mapper.Map<EmployeeListVM>(await userManager.FindByIdAsync(item.RequestingEmployeeId));
            }
          
            return model;
        }

        public async Task<List<LeaveRequestVM>> GetAllAsync(string employeeId)
        {
            //return mapper.Map<List<LeaveRequestVM>>(await context.LeaveRequests.Where(q=>q.RequestingEmployeeId.Equals(employeeId)).ToListAsync());
            return await context.LeaveRequests.Where(q => q.RequestingEmployeeId.Equals(employeeId))
                .ProjectTo<LeaveRequestVM>(configurationProvider)
                .ToListAsync();
        }

        public async Task<LeaveRequestVM?> GetLeaveRequestAsync(int? id)
        {
            var leaveRequest = await context.LeaveRequests.Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.Id == id);

            if (leaveRequest == null) return null;

            var model = mapper.Map<LeaveRequestVM>(leaveRequest);
            model.Employee =mapper.Map<EmployeeListVM>(await userManager.FindByIdAsync(leaveRequest?.RequestingEmployeeId));

            return model;
        }

        public async Task<EmployeeLeaveRequestViewVM> GetMyLeaveDetails()
        {
            var user=await userManager.GetUserAsync(httpContextAccessor?.HttpContext?.User);

            var allocations = (await leaveAllocationRepository.GetEmployeeAllocations(user.Id)).LeaveAllocations;

            var requests = await GetAllAsync(user.Id);

            var model = new EmployeeLeaveRequestViewVM(allocations, requests);

            return model;
        }
    }
}
