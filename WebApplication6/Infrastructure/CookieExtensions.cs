using Microsoft.AspNetCore.Http;
using System;

namespace WebApplication6.Infrastructure
{
    public static class CookieExtensions
    {
      
        public static void SetCookie(string key,string value,int? expire_time,HttpResponse Response,bool HttpOnly,bool Secure)
        {

            var options = new CookieOptions();
            if (expire_time.HasValue)
            {
                options.Expires = DateTime.Now.AddMinutes(expire_time.Value);
            }
            options.IsEssential = true;
            options.HttpOnly = HttpOnly;
            options.Secure = Secure;

            Response.Cookies.Append(key,value,options);
        }
        public static void Remove(string key,HttpResponse Response)
        {
            Response.Cookies.Delete(key);
        }

    }
}
