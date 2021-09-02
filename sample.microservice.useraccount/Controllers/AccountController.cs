using System;
using System.Threading.Tasks;
using Dapr.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.useraccount.Models;
using System.Collections.Generic;
using sample.microservice.useraccount;

namespace sample.microservice.useraccount.Controllers
{
    //dapr run --app-id "account-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --urls="http://+:5001"

    [ApiController]
    public class AccountController : ControllerBase
    { 

        List<string> userIdList = new List<string>();

        [HttpGet("add")]
        public async void AddUser(Account account, [FromServices] DaprClient client)
        {
            try
            {
                account.Id = Guid.NewGuid();

                Console.WriteLine("New Account");
                Console.WriteLine($"User Name: {account.Name}");
                Console.WriteLine($"User Age:  {account.age}");
                Console.WriteLine($"User ID:   {account.Id}");

                await client.SaveStateAsync("statestore", account.Id.ToString(), account);

                userIdList = await client.GetStateAsync<List<string>>("statestore", "userId"); 
                if(userIdList == null){userIdList = new List<string>();}
                userIdList.Add(account.Id.ToString());
                await client.SaveStateAsync("statestore", "userId", userIdList);
            }
            catch (Exception error){Console.WriteLine("{0} Exception caught.", error);}
        }

        [HttpGet("delete")]
        public async void DeleteUser(Account account, [FromServices] DaprClient client)
        {    
            try
            {   
                account = await client.GetStateAsync<Account>("statestore", account.Id.ToString());
                if(account != null)
                {
                    Console.WriteLine("Delete Account");
                    Console.WriteLine($"User Name: {account.Name}");
                    Console.WriteLine($"User Age:  {account.age}");
                    Console.WriteLine($"User ID:   {account.Id}");

                    await client.DeleteStateAsync("statestore", account.Id.ToString());

                    userIdList = await client.GetStateAsync<List<string>>("statestore", "userId"); 
                    userIdList.Remove(account.Id.ToString());
                    await client.SaveStateAsync("statestore", "userId", userIdList);
                }
                else{Console.WriteLine("This account is not on our server. ");}
            }
            catch(Exception error){Console.WriteLine("{0} Exception caught.", error);}
        }
        
        [HttpGet("get")]
        public async void GetUser(Account account, [FromServices] DaprClient client)
        {  
            try
            {   
                account = await client.GetStateAsync<Account>("statestore", account.Id.ToString());
                if (account != null)
                {
                    Console.WriteLine("Get Account");
                    Console.WriteLine($"User Name: {account.Name}");
                    Console.WriteLine($"User Age:  {account.age}");
                    Console.WriteLine($"User ID:   {account.Id}");
                }
                else{Console.WriteLine("This account is not on our server. ");}
            }
            catch(Exception error){Console.WriteLine("{0} Exception caught.", error);}
        }

        [HttpGet("check")]
        public async void CheckUser([FromServices] DaprClient client)
        {  
            try
            {          
                userIdList = await client.GetStateAsync<List<string>>("statestore", "userId");         
                Console.WriteLine($"Users Online: {userIdList.Count} "); 
            }
            catch(Exception error){Console.WriteLine("{0} Exception caught.", error);}
        }

        [HttpGet("list")]
        public async void UserIDList([FromServices] DaprClient client)
        {  
            try
            {        
                userIdList = await client.GetStateAsync<List<string>>("statestore", "userId");       
                if(userIdList != null){foreach(string item in userIdList){Console.WriteLine(item);}}
            }
            catch(Exception error){Console.WriteLine("{0} Exception caught.", error);}
        }
    }
}
