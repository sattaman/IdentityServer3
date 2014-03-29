﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Connect;
using Thinktecture.IdentityServer.Core.Services;
using UnitTests.Plumbing;

namespace UnitTests.AuthorizeRequest_Validation
{
    [TestClass]
    public class Authorize_ClientValidation_Token
    {
        ILogger _logger = new DebugLogger();
        ICoreSettings _settings = new TestSettings();

        [TestMethod]
        [TestCategory("AuthorizeRequest Client Validation - Token")]
        public async Task Mixed_Token_Request_Without_OpenId_Scope()
        {
            var parameters = new NameValueCollection();
            parameters.Add(Constants.AuthorizeRequest.ClientId, "implicitclient");
            parameters.Add(Constants.AuthorizeRequest.Scope, "resource profile");
            parameters.Add(Constants.AuthorizeRequest.RedirectUri, "oob://implicit/cb");
            parameters.Add(Constants.AuthorizeRequest.ResponseType, Constants.ResponseTypes.Token);

            var validator = ValidatorFactory.CreateAuthorizeValidator();
            var protocolResult = validator.ValidateProtocol(parameters);
            Assert.AreEqual(false, protocolResult.IsError);

            var clientResult = await validator.ValidateClientAsync();
            Assert.IsTrue(clientResult.IsError);
            Assert.AreEqual(ErrorTypes.Client, clientResult.ErrorType);
            Assert.AreEqual(Constants.AuthorizeErrors.InvalidScope, clientResult.Error);
        }
    }
}
