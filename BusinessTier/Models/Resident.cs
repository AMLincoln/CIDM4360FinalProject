using System;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class Resident
    {
        public int ResidentId {get; set;}
        public string ResidentName {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public int UnitNumber {get; set;}
        public List<ResidentPackage> ResidentPackages {get; set;} = new List<ResidentPackage>();
    }
}