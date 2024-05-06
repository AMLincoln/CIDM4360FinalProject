using System;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class DataTier
    {
        GUITier app = new GUITier();
        public void SeedDatabase()
        {
            using (var db = new AppDbContext())
            {
                List<Resident> residents = new List<Resident>{
                    new Resident {ResidentName = "Kittie Mousdall", UnitNumber = 101, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Claudette Rait", UnitNumber = 102, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Eliza Himsworth", UnitNumber = 103, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Emitt Gann", UnitNumber = 104, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Aurlie Pedycan", UnitNumber = 105, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Keriann Kettlesting", UnitNumber = 106, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Fiorenze Iacovuzzi", UnitNumber = 107, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Darlene Gravie", UnitNumber = 108, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Tomasine Challener", UnitNumber = 109, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Dotti Marple", UnitNumber = 110, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Gabriel Tofanelli", UnitNumber = 201, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Aldo Welldrake", UnitNumber = 202, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Ezmeralda Laydon", UnitNumber = 203, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Kale Lendrem", UnitNumber = 204, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Lenard Cubbit", UnitNumber = 205, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Dedie Caddies", UnitNumber = 206, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Nancy Janosevic", UnitNumber = 207, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Layne Whiterod", UnitNumber = 208, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Averell Labusch", UnitNumber = 209, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Gordan Raddon", UnitNumber = 210, Email ="amlincoln1@buffs.wtamu.edu"},
                    new Resident {ResidentName = "Claudette Rait", UnitNumber = 301, Email ="amlincoln1@buffs.wtamu.edu"},
                };

                User user = new User {Username = "alice", Password = "alice123"};

                UnknownPackage unknownPackage = new UnknownPackage {PostalServiceAgency = "UPS", DeliveryDate = DateOnly.FromDateTime(DateTime.Now), OwnerName = "Amanda Lincoln"};

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Add(user);
                db.Add(unknownPackage);
                db.AddRange(residents);
                db.SaveChanges();
            }
        }
        public bool AuthenticateUser(User user)
        {
            using (var db = new AppDbContext())
            {
                    try
                    {
                        var check = db.Users.Where(u => u.Username == user.Username).Where(u => u.Password == user.Password).Single();
                        Console.WriteLine("\nLog-In Successful!");
                        return true;
                    }
                    catch
                    {
                        return false;
                    }     
            }   
        }
        // Overloaded method for searches involving both the resident's name and unit number
        public int ResidentsCount(Resident resident)
        {
            using (var db = new AppDbContext())
            {
                int count = db.Residents.Where(r => r.ResidentName == resident.ResidentName).Where(r => r.UnitNumber == resident.UnitNumber).Count();
                return count;
            }
        }
        // Overloaded method for searches involving just the resident's name
        public int ResidentsCount(string residentName)
        {
            using (var db = new AppDbContext())
            {
                int count = db.Residents.Where(r => r.ResidentName == residentName).Count();
                return count;
            }
        }
        // Overloaded method for searches involving both the resident's name and unit number
        public void PullResidentInfo(Resident resident)
        {
            using (var db = new AppDbContext())
            {
                var residents = db.Residents.Where(r => r.ResidentName == resident.ResidentName).Where(r => r.UnitNumber == resident.UnitNumber);
                foreach (Resident r in residents)
                {
                    Console.WriteLine($"\nResident ID: {r.ResidentId}\t Name: {r.ResidentName}\t Email: {r.Email}\t Unit Number {r.UnitNumber}");
                }
            }
        }
        // Overloaded method for searches involving just the resident's name
        public void PullResidentInfo(string residentName)
        {
            using (var db = new AppDbContext())
            {
                var residents = db.Residents.Where(r => r.ResidentName == residentName);
                foreach (Resident r in residents)
                {
                    Console.WriteLine($"\nResident ID: {r.ResidentId}\tName: {r.ResidentName}\tEmail: {r.Email}\tUnit Number {r.UnitNumber}");
                }
            }
        }
        public void EnterUnknownPackageIntoDatabase(UnknownPackage unknownPackage)
        {
            using (var db = new AppDbContext())
            {
                db.Add(unknownPackage);
                db.SaveChanges();
            }
        }
        // Overloaded method for entering resident packages into the database, following searches involving both the resident's name and unit number
        public void EnterResidentPackageIntoDatabase(string residentName, int unitNumber, ResidentPackage residentPackage)
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    residentPackage.Resident = db.Residents.Where(r => r.ResidentName == residentName).Where(r => r.UnitNumber == unitNumber).Single();
                    db.Add(residentPackage);
                    db.SaveChanges();
                    app.ResidentEmailNotification();
                }
                catch
                {
                    Console.WriteLine("\nUnable to process your request at this time. Please try again.");
                }
                
            }
        } 
        // Overloaded method for entering resident packages into the database, following searches involving only the resident's name
        public void EnterResidentPackageIntoDatabase(int residentID, ResidentPackage residentPackage)
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    residentPackage.Resident = db.Residents.Where(r => r.ResidentId == residentID).Single();
                    db.Add(residentPackage);
                    db.SaveChanges();
                    app.ResidentEmailNotification();
                }
                catch
                {
                    Console.WriteLine("\nInput resident ID does not match an existing resident.");
                }
                
            }
        } 
        public void PullPendingPackageList()
        {
            using (var db = new AppDbContext())
            {
                var residentPackages = db.ResidentPackages.Where(p => p.Status == "Pending").Include(r => r.Resident);
                foreach (ResidentPackage p in residentPackages)
                {
                    Console.WriteLine($"\nPackage ID: {p.PackageId}\tResident Name: {p.Resident.ResidentName}\tPostal Service Agency: {p.PostalServiceAgency}\tDelivery Date {p.DeliveryDate}");
                }
            }
        }  
        public void PullPostalReturnPackageList()
        {
            using (var db = new AppDbContext())
            {
                foreach (UnknownPackage p in db.UnknownPackages)
                {
                    Console.WriteLine($"\nPackage ID: {p.PackageId}\tOwner's Name: {p.OwnerName}\tPostal Service Agency: {p.PostalServiceAgency}\tDelivery Date {p.DeliveryDate}");
                }
            }

            using (var db = new AppDbContext())
            {
                var residentPackages = db.ResidentPackages.Where(p => p.Status == "Abandoned").Include(r => r.Resident);
                foreach (ResidentPackage p in residentPackages)
                {
                    Console.WriteLine($"\nPackage ID: {p.PackageId}\tResident Name: {p.Resident.ResidentName}\tPostal Service Agency: {p.PostalServiceAgency}\tDelivery Date {p.DeliveryDate} ");
                }
            }
        } 

        public void PullResidentPackageHistory(Resident resident)
        {
            using (var db = new AppDbContext())
            {
                var listPackages = db.Residents.Where(r => r.ResidentName == resident.ResidentName).Where(r => r.UnitNumber == resident.UnitNumber).Include(p => p.ResidentPackages);
                if (listPackages.Count() > 0)
                {
                    foreach (Resident res in listPackages)
                    {
                        Console.WriteLine($"\n{res.ResidentName}:");
                        foreach(ResidentPackage pk in res.ResidentPackages)
                        {
                            Console.WriteLine($"\nPackage ID: {pk.PackageId} \t Postal Service Agency: {pk.PostalServiceAgency} \t Delivery Date {pk.DeliveryDate} \t Status: {pk.Status}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No residents were found that matched the search criteria.");
                }
            }
        } 
        public void UpdateResidentPackage(int packageID, string status)
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    ResidentPackage residentPackageToUpdate = db.ResidentPackages.Where(p => p.PackageId == packageID).First();
                    residentPackageToUpdate.Status = status;
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("\nInput package ID does not match an existing resident's package.");
                }
            }
        } 
        public void RemoveUnknownPackage(int packageID)
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    UnknownPackage unknownPackageToRemove = db.UnknownPackages.Where(p => p.PackageId == packageID).First();
                    db.Remove(unknownPackageToRemove);
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("\nInput package ID does not match an existing unknown package.");
                }
            }
        } 
    }
}

