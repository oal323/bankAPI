using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace bankAPI
{
   public class ApiKeyRequirement : IAuthorizationRequirement
{
    public IReadOnlyList<string> ApiKeys { get; set; }

    public ApiKeyRequirement(IEnumerable<string> apiKeys)
    {
        ApiKeys = apiKeys?.ToList() ?? new List<string>();
    }
}
}