﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Covid19Radar.Api.Services
{
    public class ValidationServerService : IValidationServerService
    {
        private readonly ILogger<ValidationServerService> Logger;
        private readonly IConfiguration Config;
        private readonly bool AzureFrontDoorRestrictionEnabled;
        private readonly string AzureFrontDoorId;


        public ValidationServerService(IConfiguration config, ILogger<ValidationServerService> logger)
        {
            Logger = logger;
            Config = config;
            AzureFrontDoorRestrictionEnabled = Config.AzureFrontDoorRestrictionEnabled();
            AzureFrontDoorId = Config.AzureFrontDoorId();
        }

        public IValidationServerService.ValidateResult Validate(HttpRequest req)
        {
            var fdidHeaderValue = req.Headers["X-Azure-FDID"].ToString();
            if (AzureFrontDoorRestrictionEnabled)
            {
                if (fdidHeaderValue != AzureFrontDoorId)
                {
                    return IValidationServerService.ValidateResult.InvalidAzureFrontDoorId;
                }
            }
            return IValidationServerService.ValidateResult.Success;
        }
    }
}
