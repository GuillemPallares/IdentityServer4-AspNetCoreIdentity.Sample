﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerHost.Events.Infraestructure;

namespace IdentityServerHost.Events
{
    /// <summary>
    /// Event for failed user authentication
    /// </summary>
    /// <seealso cref="IdentityServerHost.Events.Event" />
    public class UserRegisterFailureEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:IdentityServerHost.Events.UserRegisterFailureEvent" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="error">The error.</param>
        /// <param name="interactive">Specifies if login was interactive</param>
        /// <param name="clientId">The client id</param>
        public UserRegisterFailureEvent(string username, string error, bool interactive = true, string clientId = null)
            : base(EventCategories.Registration,
                  "User Register Failure",
                  EventTypes.Failure, 
                  EventIds.UserRegisterFailure,
                  error)
        {
            Username = username;
            ClientId = clientId;

            if (interactive)
            {
                Endpoint = "UI";
            }
            else
            {
                Endpoint = "Token";
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; set; }
    }
}