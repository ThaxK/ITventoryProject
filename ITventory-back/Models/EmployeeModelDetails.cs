﻿using Itventory.web.Entidades;

namespace WebApplication1.Models
{
    public class EmployeeModelDetails
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public Status Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

    public string FullName => $"{Name} {LastName} {DocumentNumber}";

    public int WorkAreaId { get; set; }
}
}
