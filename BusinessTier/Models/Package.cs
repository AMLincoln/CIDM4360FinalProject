using System;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public abstract class Package
    {
        public int PackageId {get; set;}
        public string PostalServiceAgency {get; set;} = string.Empty;
        public DateOnly DeliveryDate {get; set;}
    }

    public class UnknownPackage : Package
    {
        public string OwnerName {get; set;} = string.Empty;
    }

    public class ResidentPackage : Package
    {
        public string Status {get; set;} = string.Empty;
        public int ResidentId {get; set;}
        public Resident Resident {get; set;} = null!;
    }
}