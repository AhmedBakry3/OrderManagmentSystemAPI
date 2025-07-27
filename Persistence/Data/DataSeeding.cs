using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class DataSeeding(StoredIdentityDbContext _identityDbContext
                           , RoleManager<IdentityRole> _roleManager
                           , UserManager<IdentityUser> _userManager
                           , OrderManagementDbContext _dbContext) : IDataSeeding
    {
        public async Task SeedInvoicesAsync()
        {
            if (!_dbContext.Invoices.Any())
            {
                var orders = await _dbContext.Orders.ToListAsync();

                foreach (var order in orders)
                {
                    _dbContext.Invoices.Add(new Invoice
                    {
                        OrderId = order.Id,
                        InvoiceDate = DateTime.UtcNow,
                        TotalAmount = 200 + order.Id * 10
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new IdentityUser
                    {
                        Email = "Ahmed@gmail.com",
                        PhoneNumber = "01155325382",
                        UserName = "AhmedBakry",
                    };

                    var User02 = new IdentityUser
                    {
                        Email = "Mohamed@gmail.com",
                        PhoneNumber = "0111963282",
                        UserName = "MohamedBakry",
                    };

                    await _userManager.CreateAsync(User01, "P@ssW0rd");
                    await _userManager.CreateAsync(User02, "P@ssW0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "Customer");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
