using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashShopAPI.Tests
{
    public class AllowAnonymousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var req in context.PendingRequirements.ToList())
            {
                context.Succeed(req);
            }

            return Task.CompletedTask;
        }
    }
}
