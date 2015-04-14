﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.NetworkResourceProvider.Models;

namespace Microsoft.Azure.Commands.NetworkResourceProvider
{
    [Cmdlet(VerbsCommon.New, "AzureLoadBalancerBackendAddressPoolConfig"), OutputType(typeof(PSBackendAddressPool))]
    public class NewAzureLoadBalancerBackendAddressPoolConfigCommand : AzureLoadBalancerBackendAddressPoolConfigBase
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of the BackendAddressPool")]
        [ValidateNotNullOrEmpty]
        public override string Name { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var backendAddressPool = new PSBackendAddressPool();
            backendAddressPool.Name = this.Name;
            backendAddressPool.BackendIpConfigurations = new List<PSResourceId>();

            if (this.BackendIpConfigurationId != null)
            {
                foreach (var backendIpConfigurationId in this.BackendIpConfigurationId)
                {
                    var resourceId = new PSResourceId();
                    resourceId.Id = backendIpConfigurationId;
                    backendAddressPool.BackendIpConfigurations.Add(resourceId);
                }
            }

            backendAddressPool.Id =
                ChildResourceHelper.GetResourceNotSetId(
                    this.NetworkClient.NetworkResourceProviderClient.Credentials.SubscriptionId,
                    Microsoft.Azure.Commands.NetworkResourceProvider.Properties.Resources.LoadBalancerBackendAddressPoolName,
                    this.Name);

            WriteObject(backendAddressPool);
        }
    }
}
