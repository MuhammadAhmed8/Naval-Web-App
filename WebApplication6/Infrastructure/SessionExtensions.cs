using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApplication6.Infrastructure
{
    public static class SessionExtensions
    {
        public static T GetJson<T>(this ISession session,string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null
            ?
                default(T):JsonConvert.DeserializeObject<T>(sessionData);
            
        }

        public static void SetJson<T>(this ISession session,string key,T value)
        {
            session.SetString(key,JsonConvert.SerializeObject(value));
        }
    }

}
