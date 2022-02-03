using Nethereum.Web3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MetaMask : MonoBehaviour
{
    // Start is called before the first frame update
    private Web3 web3;

    void Start()
    {
        web3 = new Web3(TestNethereum.url);
         
        if (PlayerPrefs.HasKey("address") && PlayerPrefs.HasKey("privatekey"))
        {
            Debug.Log("Auto Login");
            LoginWeb3(PlayerPrefs.GetString("address"), PlayerPrefs.GetString("privatekey"));
        }
        else
        {
            var address = "Adress";
            var priKey = "PrivateKey";
            LoginWeb3(address, priKey);
        }


    }

    private void LoginWeb3(string address,string privateKey)
    {
        if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(privateKey))
        {
            try
            {
                var address2 = Web3.GetAddressFromPrivateKey(privateKey);
                if (address2.Equals(address))
                {
                    //IsValid
                    Debug.Log("Success Login");
                    var balance = web3.Eth.GetBalance.SendRequestAsync(address);
                    if (balance.Exception != null)
                    {
                        Debug.Log(balance.Exception.Message);
                        return;
                    }
                    //All request are success!
                    Debug.Log("Balance: " + Web3.Convert.FromWei(balance.Result.Value) + " ETH");
                    PlayerPrefs.SetString("address",address);
                    PlayerPrefs.SetString("privatekey",privateKey);
                }
                else
                {
                    Debug.Log("Unvalid Account!");
                }

            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        else
        {
            Debug.Log("You should enter a address or private key"!);
        }
    }

   
}
