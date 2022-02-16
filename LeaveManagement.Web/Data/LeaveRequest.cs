﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Web.Data
{
    public class LeaveRequest :BaseEntity
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public DateTime DateRequested { get; set; }
        public string? RequestComments { get; set; }

        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        public string RequestingEmployeeId { get; set; }
    }
}
