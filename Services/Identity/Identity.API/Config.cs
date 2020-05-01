// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace wwf.Services.Identity.API
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                 new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {   new ApiResource("organizer","Organizer Service"),
                new ApiResource ("catalog","Catalog Service"),
                 new ApiResource("bi","BI Service"),
                new ApiResource("userinfo","User Information Service"),
                new ApiResource("recording","Recording Service"),
                new ApiResource("reward","Reward Service"),
                new ApiResource("coin","Coin Service")
                };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {  new Client{
                ClientId="publisher-js.wwf.com",
                ClientName="PubliherWebClient",
                AllowedGrantTypes= GrantTypes.Code,
                RequireClientSecret=false,
                RequirePkce=true,
                RedirectUris = { "http://localhost:5003/callback.html" },
                PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                AllowedCorsOrigins =     { "http://localhost:5003" },
                AllowedScopes = {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "organizer"
                                    }
                }
                 };

    }
}