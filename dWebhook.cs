using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class dWebHook
{
    private readonly WebClient dWebClient;

    public string UserName = "Photon Boye";
    public string ProfilePicture = "https://cdn.discordapp.com/avatars/698110005941108757/0f2bfde0df6d79740380ffa70005420c.png";

    public dWebHook()
    {
        dWebClient = new WebClient();
    }


    public void SendMessage(string msgSend)
    {

        NameValueCollection discordValues = new NameValueCollection();

        discordValues.Add("content", msgSend);

        discordValues.Add("username", UserName);

        discordValues.Add("avatar_url", ProfilePicture);

        dWebClient.UploadValues("https://canary.discordapp.com/api/webhooks/698709639684030494/91V5hIenry_NToiofUgc0lqsMiSrnmwRgdhF0NT0AFEvumdKtRktI2u5NOXH4FewJkUb", discordValues);
    }
}