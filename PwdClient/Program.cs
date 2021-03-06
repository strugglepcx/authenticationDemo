﻿using System;
using System.Net.Http;
using IdentityModel.Client;

namespace PwdClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var diso = DiscoveryClient.GetAsync("http://localhost:5000").Result;

            if (diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }
            var tokenClient = new TokenClient(diso.TokenEndpoint, "pwdClient", "secret");

            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("strugglepcx", "123456").Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }
            else
            {
                Console.WriteLine(tokenResponse.Json);
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(tokenResponse.AccessToken);
                var response = httpClient.GetAsync("http://localhost:5001/api/values").Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
            Console.ReadLine();
        }
    }
}
