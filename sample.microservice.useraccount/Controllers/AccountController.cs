using System;
using System.Threading.Tasks;
using Dapr.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.useraccount.Models;

namespace sample.microservice.useraccount.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddUser(Account account, [FromServices] DaprClient client)
        {
            try
            {
                account.Id = Guid.NewGuid();

                Console.WriteLine("New Account");
                Console.WriteLine($"Account Name: {account.Name}");
                Console.WriteLine($"Account ID: {account.Id}");

                await client.SaveStateAsync("statestore", account.Id.ToString(), account.Name);

                int usersOnline = await client.GetStateAsync<int>("statestore", "UsersOnline");
                await client.SaveStateAsync("statestore", "UsersOnline", usersOnline += 1);

                return account.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return account.Id;
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<Guid>> DeleteUser(Account account, [FromServices] DaprClient client)
        {    
            try
            {   
                account.Name = await client.GetStateAsync<string>("statestore", account.Id.ToString());

                if(account.Name != null)
                {
                    Console.WriteLine("Delete Account");
                    Console.WriteLine($"Account Name: {account.Name}");
                    Console.WriteLine($"Account ID: {account.Id}");

                    await client.DeleteStateAsync("statestore", account.Id.ToString());

                    int usersOnline = await client.GetStateAsync<int>("statestore", "UsersOnline");
                    await client.SaveStateAsync("statestore", "UsersOnline", usersOnline -= 1);
                }
                else
                {
                    Console.WriteLine("This account is not on our server. ");
                }

                return account.Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return account.Id;
            }
        }
        
        [HttpPost("get")]
        public async Task<ActionResult<Guid>> GetUser(Account account, [FromServices] DaprClient client)
        {  
            try
            {   
                account.Name = await client.GetStateAsync<string>("statestore", account.Id.ToString());

                if (account.Name != null)
                {
                    Console.WriteLine("Get Account");
                    Console.WriteLine($"Account Name: {account.Name}");
                    Console.WriteLine($"Account ID: {account.Id}");
                }
                else
                {
                    Console.WriteLine("This account is not on our server. ");
                }

                return account.Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return account.Id;
            }
        }

        [HttpPost("check")]
        public async Task<ActionResult<int>> CheckUser([FromServices] DaprClient client)
        {  
            try
            {                   
                int usersOnline = await client.GetStateAsync<int>("statestore", "UsersOnline");

                Console.WriteLine($"Users Online: {usersOnline} "); 

                return usersOnline;
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return 0;
            }
        }
    }
}
