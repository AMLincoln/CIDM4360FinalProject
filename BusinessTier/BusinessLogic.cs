/* 
Amanda M. Lincoln
CIDM 4360-70 Final Project

This is a mailroom management application intended to improve the mailroom service for Amarillo Apartments.

The following functions have been implemented as of 6 May 2024:

1. New Package Processing
2. View a list of packages in the pending area
3. View a list of packages in the postal return area
4. Update records for packages associated with a current resident
5. Remove records for unknown packages
6. Email notification upon input of a resident's package into the database, or as needed
7. Pull a complete package history for current residents using their name and unit number

*/

using FinalProject;

class BusinessLogic
{
    static void Main(string[] args)
    {
        DataTier database = new DataTier();
        database.SeedDatabase();

        GUITier app = new GUITier();

        // Title included here instead of in GUITier to prevent it from appearing more than once
        Console.WriteLine("\n------Welcome to the Mailroom Management Application------");

        // Login Process
        User user;
        bool authentication = true;

        while (authentication)
        {
            user = app.DisplayLogin();
            if (database.AuthenticateUser(user))
            {
                break;
            }
            else
            {
                Console.WriteLine("\nIncorrect username and/or password.");
            }
        }

        // Dashboard
        Resident resident;
        bool _continue = true;
        while (_continue)
        {
            int choice = app.DisplayDashboard();
            switch (choice)
            {
                // Process a new package
                case 1:
                    int count;
                    resident = app.QueryForResidentInfo(1);
                    if (resident.UnitNumber == 0)
                    {
                        count = database.ResidentsCount(resident.ResidentName);
                    }
                    else 
                    {
                        count = database.ResidentsCount(resident); 
                    }
                    
                    switch(count)
                    {
                        // Create record for an unknown package
                        case 0: 
                            Console.WriteLine("\nThere are no residents that match this criteria");
                            Console.WriteLine("\nWould you like to create an unknown package record? Y/N");
                            while (true)
                            {
                                string? input = Console.ReadLine(); 
                                if (input == "Y" || input == "y")
                                {
                                    UnknownPackage unknownPackage = app.QueryForUnknownPackageInfo(resident.ResidentName);
                                    database.EnterUnknownPackageIntoDatabase(unknownPackage);
                                    break;
                                }
                                else if (input == "N" || input == "n")
                                {
                                    break;
                                }
                                else 
                                {
                                   Console.WriteLine("Invalid input. Please input Y or N:");
                                }
                            }
                            break; 
                        // Create a record for a resident's package if only one resident resulted from query
                        case 1:
                            if (resident.UnitNumber == 0)
                            {
                                database.PullResidentInfo(resident.ResidentName);
                            }
                            else
                            {
                                database.PullResidentInfo(resident);
                            }

                            Console.WriteLine("\nWould you like to create an new resident package record? Y/N");
                            while (true)
                            {
                                string? input = Console.ReadLine(); 
                                if (input == "Y" || input == "y")
                                {
                                    ResidentPackage residentPackage = app.QueryForResidentPackageInfo(resident.ResidentName);
                                    database.EnterResidentPackageIntoDatabase(resident.ResidentName, resident.UnitNumber, residentPackage);
                                    break;
                                }
                                else if (input == "N" || input == "n")
                                {
                                    break;
                                }
                                else 
                                {
                                   Console.WriteLine("Invalid input. Please input Y or N:");
                                }
                            }
                            break;
                        // Create a record for a resident's package if more than one resident resulted from query. 
                        // Intended to follow searches involving only the resident's name    
                        default: 
                            database.PullResidentInfo(resident.ResidentName);
                            Console.WriteLine("\nWould you like to create an new resident package record? Y/N");
                            while (true)
                            {
                                string? input = Console.ReadLine(); 
                                if (input == "Y" || input == "y")
                                {
                                    int residentID;
                                    ResidentPackage residentPackage = app.QueryForResidentPackageInfo(out residentID);
                                    database.EnterResidentPackageIntoDatabase(residentID, residentPackage);
                                    break;
                                }
                                else if (input == "N" || input == "n")
                                {
                                    break;
                                }
                                else 
                                {
                                   Console.WriteLine("Invalid input. Please input Y or N:");
                                }
                            }
                            break;    
                    }
                    break;
                // View packages in pending area
                case 2:
                    database.PullPendingPackageList();
                    // Update a record for a package in the pending area
                    Console.WriteLine("\nWould you like to update the status of a package record? Y/N");
                    while (true)
                    {
                        string? input = Console.ReadLine(); 
                        if (input == "Y" || input == "y")
                        {
                            int packageID;
                            string status;
                            app.QueryForResidentPackageUpdate(out packageID, out status);
                            database.UpdateResidentPackage(packageID, status);
                            break;
                        }
                        else if (input == "N" || input == "n")
                        {
                            break;
                        }
                        else 
                        {
                            Console.WriteLine("Invalid input. Please input Y or N:");
                        }
                    }
                    break;
                // View packages in postal return area
                case 3:
                    database.PullPostalReturnPackageList();
                    bool _innerContinue = true;
                    while (_innerContinue)
                    {
                        int innerChoice = app.DisplayPostalReturnSubMenu();
                        switch (innerChoice)
                        {
                            // Update the status on a record for a resident's package that has been previously moved from the pending to the postal return area
                            case 1:
                                int packageID;
                                string status;
                                app.QueryForResidentPackageUpdate(out packageID, out status);
                                database.UpdateResidentPackage(packageID, status);
                                _innerContinue = false;
                                break;
                            // Remove record for an unknown package once it has been removed from the postal return area
                            case 2:
                                while(true)
                                {
                                    Console.WriteLine("Please enter the ID for the package you wish to remove:");
                                    string? inpPackageID = Console.ReadLine();
                                    bool isParsedSuccessfully = Int32.TryParse(inpPackageID, out int number);  
                                    if (isParsedSuccessfully == true)
                                    {
                                        database.RemoveUnknownPackage(number);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                _innerContinue = false;
                                break;
                            // Return to main menu functionality
                            case 3:
                                _innerContinue = false;
                                break;    
                            default:
                                Console.WriteLine("Invalid selection");
                                break;
                        }
                    }
                    break;
                // View a resident's package history
                case 4:
                    Resident residentHistory = app.QueryForResidentInfo(2);
                    database.PullResidentPackageHistory(residentHistory);
                    break;
                // Resend email functionality in case the user receives a message that the original notification email was not successful.
                case 5:
                    app.ResidentEmailNotification();
                    break;
                // Log Out
                case 6:
                    _continue = false;
                    Console.WriteLine("\nGoodbye!\n");
                    break;
                default:
                    Console.WriteLine("\nInvalid selection");
                    break;
            }
        }
    }
}




