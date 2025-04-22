using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;

public class ApiManage : MonoBehaviour
{
   static public  string host = "http://galaxycao.asuscomm.com:7711";

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


 
   
 
    static public IEnumerator GetUestActive(string u_id,string Active)
    {
        var req = UnityWebRequest.Get(host + "/api/Active/"+u_id+"/" + Active);
        // req.certificateHandler = new CertHandler();
        Debug.Log(req.url);
        yield return req.SendWebRequest();
        if (req.isNetworkError)
        {
            Debug.Log("Something went wrong, and returned error: " + req.error);
        }
        else
        {
            // Show results as text
            Debug.Log(req.downloadHandler.text);
        }

       

    }
  

}
public class CertHandler : CertificateHandler
{
   
    private static string PUB_KEY = "MIICtjCCAZ4CAQAwcTELMAkGA1UEBhMCSEsxGTAXBgNVBAMMEHZvY2FiZ28uZWR1aGsuaGsxDzANBgNVBAcMBlRhaSBQbzEOMAwGA1UECgwFRWRVSEsxGDAWBgNVBAgMD05ldyBUZXJyaXRvcmllczEMMAoGA1UECwwDTUlUMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxoYOs1H3boZfP4Slnyr6bczehCiLFzaOFe3fo1Jn9Mhzbuhym1dIcLMCTfJLnp2ePzjlaurIHgDR8GbrENiHj72tqWKPO2eZVvAb/Y87Z8e0v36loIu38ATYNLLUS973ywLP8crWd4Y0ligR6AHREOCJ7/6Tg8Wo+I+irFju0RxCc0L+Dv2jh/oefUHUQodSugcOamv2jTG2GfGjjnkmPbcLsrWrRqMAIa/cc0eH/tlkgUnswBVXbJjrA706I+RFAG5HLoEHnPDyqeVjZ6R/PXVBW0DWUv29Ac/X1zAXaomUorY/7YPmNPkkBW0CpWsUsE+7W6MVMXWsWVmpEVZrNQIDAQABoAAwDQYJKoZIhvcNAQELBQADggEBAA0oacpHrlH8p37++z+7P63BfMrTX51vV8em7iZLL7r1cDKtNDsPPlLEc/DW7sr6PxjvMsFqeL/azOdGx0gd7OzR+UA6nbD27Vv6GDU2UFwEI9HouZyhZaH1ErrY+GR9A7w9XLZI04NRhAAZ/MkxMXDqLx1s0kskTRsmANfj9bo9in+4KRi9MRFM204cwxc7OyaR22FhC15TmKYDvULHFvehuGLLKZfhL+8rvIeafTpOY7ThH33Pwb9yTAXdiFiyUiy9zuvuIMWvWXxyvDv1+V4lEuIxQHQLXhKAk0B7yd3rsQa0XHQnf9r5y3qv+Wy6GulN9MUnnw+WstF8uQmNya0=";
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
      //  if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            return true;
      //  return false;
    }
}
