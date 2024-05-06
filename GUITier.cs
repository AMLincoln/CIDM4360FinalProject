using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class GUITier
    {
        public User DisplayLogin()
        {
            User user = new User();
            Console.WriteLine("\nPlease enter your username:");
            user.Username = Console.ReadLine()!;
            Console.WriteLine("Please enter your password:");
            user.Password = Console.ReadLine()!;
            return user;
        } 

        public int DisplayDashboard()
        {
            while (true)
            {
                Console.WriteLine("\n---------------Dashboard-------------------\n");
                Console.WriteLine("Choose one of the following options:\n");
                Console.WriteLine("1 - Process New Package");
                Console.WriteLine("2 - View Packages in Pending Area");
                Console.WriteLine("3 - View Packages in Postal Return Area");
                Console.WriteLine("4 - View a Resident's Package History");
                Console.WriteLine("5 - Resend Email Notification");
                Console.WriteLine("6 - Log Out");
                Console.WriteLine("\n-----------------------------------------------");
                string? input =  Console.ReadLine();
                bool isParsedSuccessfully = Int32.TryParse(input, out int number);
                if (isParsedSuccessfully == true)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid selection");
                }
            }  
        }
        public int DisplayPostalReturnSubMenu()
        {
            while (true)
            {
                Console.WriteLine("\nChoose from the following options:");
                Console.WriteLine("\n1 - Update the record for an abandoned package");
                Console.WriteLine("2 - Remove an unknown package");
                Console.WriteLine("3 - Return to Main Menu");
                string? input =  Console.ReadLine();
                bool isParsedSuccessfully = Int32.TryParse(input, out int number);
                if (isParsedSuccessfully == true)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid selection");
                }
            }  
        }

        public Resident QueryForResidentInfo(int switchInput)
        {
            switch (switchInput)
            {
                case 1:
                    while (true)
                    {
                        Resident resident = new Resident();
                        Console.WriteLine("Please enter the name on the package:");
                        string? inpName = Console.ReadLine();
                        if (string.IsNullOrEmpty(inpName))
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Please enter the unit number on the package:");
                            while(true)
                            {
                                    string? inpUnitNumber = Console.ReadLine();
                                    bool isParsedSuccessfully = Int32.TryParse(inpUnitNumber, out int number);  
                                    if (string.IsNullOrEmpty(inpUnitNumber))
                                    {
                                        resident.ResidentName = inpName;
                                        return resident;
                                    }
                                    else if (isParsedSuccessfully == true)
                                    {
                                        resident.ResidentName = inpName;
                                        resident.UnitNumber = number;
                                        return resident;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please re-enter the unit number on the package:");
                                    }
                            }
                        }
                    }
                default:
                    while (true)
                    {
                        Resident resident = new Resident();
                        Console.WriteLine("Please enter the resident's name:");
                        string? inpName = Console.ReadLine();
                        if (string.IsNullOrEmpty(inpName))
                        {
                            continue;
                        }
                        else
                        {
                            while(true)
                            {
                                    Console.WriteLine("Please enter resident's unit number:");
                                    string? inpUnitNumber = Console.ReadLine();
                                    bool isParsedSuccessfully = Int32.TryParse(inpUnitNumber, out int number);  
                                    if (isParsedSuccessfully == true)
                                    {
                                        resident.ResidentName = inpName;
                                        resident.UnitNumber = number;
                                        return resident;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                            }
                        }
                    }
            }       
        }
        public UnknownPackage QueryForUnknownPackageInfo(string residentName)
        {
            UnknownPackage unknownPackage = new UnknownPackage();
            unknownPackage.OwnerName = residentName;
            
            while (true)
            {
                Console.WriteLine("Please enter the postal service agency associated with the package:");
                string? inpPSA = Console.ReadLine();
                if (string.IsNullOrEmpty(inpPSA))
                {
                    continue;
                }
                else
                {
                    unknownPackage.PostalServiceAgency = inpPSA;
                    while(true)
                    {
                            Console.WriteLine("Please enter the date that the package arrived in the following format (MM/DD/YYYY):");
                            string? inpDeliveryDate = Console.ReadLine();
                            bool isParsedSuccessfully = DateOnly.TryParse(inpDeliveryDate, out DateOnly deliveryDate);  
                            if (isParsedSuccessfully == true)
                            {
                                unknownPackage.DeliveryDate = deliveryDate;
                                return unknownPackage;
                            }
                            else
                            {
                                continue;
                            }
                    }
                }
            }
        }
        // Overloaded method for searches that resulted in only one resident
        public ResidentPackage QueryForResidentPackageInfo(string residentName)
        { 
            ResidentPackage residentPackage = new ResidentPackage();
            while (true)
            {
                Console.WriteLine("Please enter the postal service agency associated with the package:");
                string? inpPSA = Console.ReadLine();
                if (string.IsNullOrEmpty(inpPSA))
                {
                    continue;
                }
                else
                {
                    residentPackage.PostalServiceAgency = inpPSA;
                    while(true)
                    {
                            Console.WriteLine("Please enter the date that the package arrived in the following format (MM/DD/YYYY):");
                            string? inpDeliveryDate = Console.ReadLine();
                            bool isParsedSuccessfully = DateOnly.TryParse(inpDeliveryDate, out DateOnly deliveryDate);  
                            if (isParsedSuccessfully == true)
                            {
                                residentPackage.DeliveryDate = deliveryDate;
                                while(true)
                                {
                                    Console.WriteLine("Please enter the status of the package. Acceptable statuses include Pending, Abandoned, Lost/Stolen, Damaged/Destroyed, Returned To Postal Service, or Picked Up");
                                    string? inpStatus = Console.ReadLine();
                                    if (string.IsNullOrEmpty(inpStatus))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        residentPackage.Status = inpStatus;
                                        Console.WriteLine("Please review the following for accuracy:");
                                        Console.WriteLine($"\nResident's Name: {residentName}\t\tPostal Service Agency: {residentPackage.PostalServiceAgency}\tDelivery Date: {residentPackage.DeliveryDate}\t\tStatus: {residentPackage.Status}");
                                        Console.WriteLine("\nIs the information correct? Y/N");
                                        string? confirm = Console.ReadLine();
                                        if (confirm == "Y" || confirm == "y")
                                        {
                                            return residentPackage;
                                        }
                                        else if (confirm == "N" || confirm == "n")
                                        {
                                            break;
                                        }
                                        else 
                                        {
                                        Console.WriteLine("Invalid input. Please input Y or N:");
                                        }
                                        
                                    }
                                }
                                break;
                            }
                            else
                            {
                                continue;
                            }
                    }
                }
            }
        }
        // Overloaded method for searches that resulted in multiple residents
        public ResidentPackage QueryForResidentPackageInfo(out int residentID)
        { 
            ResidentPackage residentPackage = new ResidentPackage();
            while (true)
            {
                Console.WriteLine("Please input the Resident ID for the resident associated with the package:");
                string? inpResidentID = Console.ReadLine();
                bool isParsedSuccessfully = Int32.TryParse(inpResidentID, out int number);
                if(isParsedSuccessfully)
                {
                    while (true)
                    {
                        Console.WriteLine("Please enter the postal service agency associated with the package:");
                        string? inpPSA = Console.ReadLine();
                        if (string.IsNullOrEmpty(inpPSA))
                        {
                            continue;
                        }
                        else
                        {
                            residentPackage.PostalServiceAgency = inpPSA;
                            while(true)
                            {
                                    Console.WriteLine("Please enter the date that the package arrived in the following format (MM/DD/YYYY):");
                                    string? inpDeliveryDate = Console.ReadLine();
                                    bool isParsedSuccessfully2 = DateOnly.TryParse(inpDeliveryDate, out DateOnly deliveryDate);  
                                    if (isParsedSuccessfully2 == true)
                                    {
                                        residentPackage.DeliveryDate = deliveryDate;
                                        while(true)
                                        {
                                            Console.WriteLine("Please enter the status of the package. Acceptable statuses include Pending, Abandoned, Lost/Stolen, Damaged/Destroyed, Returned To Postal Service, or Picked Up");
                                            string? inpStatus = Console.ReadLine();
                                            if (string.IsNullOrEmpty(inpStatus))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                residentPackage.Status = inpStatus;
                                                Console.WriteLine("Please review the following for accuracy:");
                                                Console.WriteLine($"\nResident's ID: {inpResidentID}\tPostal Service Agency {residentPackage.PostalServiceAgency}\tDelivery Date: {residentPackage.DeliveryDate}\t\tStatus: {residentPackage.Status}");
                                                Console.WriteLine("\nIs the information correct? Y/N");
                                                string? confirm = Console.ReadLine();
                                                if (confirm == "Y" || confirm == "y")
                                                {
                                                    residentID = number;
                                                    return residentPackage;
                                                }
                                                else if (confirm == "N" || confirm == "n")
                                                {
                                                    break;
                                                }
                                                else 
                                                {
                                                Console.WriteLine("Invalid input. Please input Y or N:");
                                                }
                                                
                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }        

        public void QueryForResidentPackageUpdate(out int packageID, out string status)
        { 
            while(true)
            {
                Console.WriteLine("Please enter the ID for the package you wish to update:");
                string? inpPackageID = Console.ReadLine();
                bool isParsedSuccessfully = Int32.TryParse(inpPackageID, out int number);  
                if (isParsedSuccessfully == true)
                {
                    while(true)
                    {
                        Console.WriteLine("Please enter the status of the package. Acceptable statuses include Pending, Abandoned, Lost/Stolen, Damaged/Destroyed, Returned To Postal Service, or Picked Up");
                        string? inpStatus = Console.ReadLine();
                        if (string.IsNullOrEmpty(inpStatus))
                        {
                            continue;
                        }
                        else
                        {
                            packageID = number;
                            status = inpStatus;  
                            break;       
                        }
                    }
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        // Chose to place ResidentEmailNotification within GUI due to presence of email formatting
        public async void ResidentEmailNotification()
        {
            var sender = "DoNotReply@4d75c6e2-2e84-4129-9253-1808b92909f6.azurecomm.net";
            string serviceConnectionString =  "endpoint=https://amlincoln1communicationservice.unitedstates.communication.azure.com/;accesskey=Vrt+4UCemkeOMWZrj2BDjF0ZFE1xEXeFwllMy5cOqk65DkCCfaxa3nF7CL6knkzXaT0ycEXvgM68WW63RdIj3Q==";
            
            EmailClient emailClient = new EmailClient(serviceConnectionString);
            var subject = "Package Notification";
            var htmlContent = @"
                            <html>
                                <body>
                                    <p>Hello,</p>
                                    <p>We have received a package in the office for you. Due to limited storage space, you will have 5 days to pick up your package, or it will be sent back to the post office.</p>
                                    <p>Thank you!</p>
                                </body>
                            </html>";
            while (true)
            {
                Console.WriteLine("Please input recipient email address: ");
                string? recipient = Console.ReadLine();
                if (String.IsNullOrEmpty(recipient))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Sending email...");
                        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                        Azure.WaitUntil.Started,
                        sender,
                        recipient,
                        subject,
                        htmlContent);

                    try
                    {
                        while (true)
                        {
                            await emailSendOperation.UpdateStatusAsync();
                            if (emailSendOperation.HasCompleted)
                            {
                                break;
                            }
                            await Task.Delay(2000);
                        }

                        if (emailSendOperation.HasValue)
                        {
                            Console.WriteLine($"Email Status = {emailSendOperation.Value.Status}");
                        }
                    }
                    catch (RequestFailedException ex)
                    {
                        Console.WriteLine($"Email send failed with Code = {ex.ErrorCode} and Message = {ex.Message}");
                    }
                    break;
                }
            }  
        }
    }    
}
