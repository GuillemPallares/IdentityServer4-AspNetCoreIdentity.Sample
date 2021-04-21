// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerHost.Models;
using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerHost
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1"),
                // Prototype for AdminScope
                // new ApiScope("admin")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
                {
                    // Prototype for Admin API
                    /* new ApiResource("admin", "Admin API")
                    {
                        ApiSecrets = { new Secret("secret".Sha256()) },
                        
                        Scopes = { "admin" }
                    }, */

                    new ApiResource("api1", "Demo 1")
                    {
                        ApiSecrets = { new Secret("secret".Sha256()) },
                        
                        Scopes = { "api1" }
                    }
                };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1" }
                },

                // interactive client using code flow + pkce
                // Prototype for AdminUI
                /* new Client
                {
                    ClientId = "Admin",
                    ClientName ="Admin UI",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:4200/" },
                    FrontChannelLogoutUri = "https://localhost:5001/",
                    PostLogoutRedirectUris = { "https://localhost:4200/" },
                    AllowedCorsOrigins = { "https://localhost:4200" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "admin" },

                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                }, */
            };
    }
}
