// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using IdentityServer4.EntityFramework.Storage;
using Serilog;
using Microsoft.AspNetCore.Identity;
using IdentityServerHost.Models;
using System.Security.Claims;
using System.Collections.Generic;
using IdentityModel;
using IdentityServerHost.Data;
using IdentityServerHost;

namespace IdentityServerHost
{
    public class SeedData
    {
        public async static void EnsureSeedData(string connectionString, UserManager<ApplicationUser> _userManager)
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

            services.AddOperationalDbContext(options =>
            {
                Log.Information("Migrating OperationalDbContext");
                options.ConfigureDbContext = db => db.UseSqlite(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });
            services.AddConfigurationDbContext(options =>
            {
                Log.Information("Migrating ConfigurationDbContext");
                options.ConfigureDbContext = db => db.UseSqlite(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });
       
            var serviceProvider = services.BuildServiceProvider(); 

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();

                if (_userManager.FindByNameAsync("Admin").Result == null)
                    {
                        Log.Debug("No Admin Find");

                        ApplicationUser user = new ApplicationUser();
                        user.UserName = "Admin";
                        user.Email = "admin@localhost.com";

                        IdentityResult result = await _userManager.CreateAsync(user, "Prueba1234$");

                        
                        if (result.Succeeded)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, user.Email),
                                new Claim(JwtClaimTypes.Name, user.UserName)
                            };

                            await _userManager.AddClaimsAsync(user, claims);

                            Log.Debug("Admin Created");
                        }
                    }

                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var configContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                configContext.Database.Migrate();
                EnsureSeedData(configContext);
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext configContext)
        {
            if (!configContext.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Config.Clients.ToList())
                {
                    configContext.Clients.Add(client.ToEntity());
                }
                configContext.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }

            if (!configContext.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    configContext.IdentityResources.Add(resource.ToEntity());
                }
                configContext.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }

            if (!configContext.ApiResources.Any())
            {
                Log.Debug("ApiReosurces being populated");
                foreach (var resource in Config.ApiResources.ToList())
                {
                    configContext.ApiResources.Add(resource.ToEntity());
                }
                configContext.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }
            
            if (!configContext.ApiScopes.Any())
            {
                Log.Debug("ApiScopes being populated");
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    configContext.ApiScopes.Add(resource.ToEntity());
                }
                configContext.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }
        }
    }
}
