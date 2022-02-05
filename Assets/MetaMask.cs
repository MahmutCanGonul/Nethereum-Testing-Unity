using Nethereum.Web3;
using NBitcoin;
using Nethereum.HdWallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;

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
            var address = "Address";
            var priKey = "PrivateKey";
            LoginWeb3(address, priKey);
        }

        //CreateWallet("Test123456");




    }

    private void LoginWeb3(string address, string privateKey)
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
                    PlayerPrefs.SetString("address", address);
                    PlayerPrefs.SetString("privatekey", privateKey);
                }
                else
                {
                    Debug.Log("Unvalid Account!");
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        else
        {
            Debug.Log("You should enter a address or private key"!);
        }
    }

    private void CreateWallet(string password)
    {
        if (password.Length > 6)
        {
            Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
            var secretWords = "";
            for (int i = 0; i < mnemo.Words.Length; i++)
            {
                Debug.Log(mnemo.Words[i]);
                secretWords += mnemo.Words[i] + " ";
            }
            var wallet = new Wallet(secretWords, password);
            if (wallet != null)
            {
                var account = wallet.GetAccount(0);
                Debug.Log("Account is created!");
                Debug.Log("Adress: " + account.Address);
                Debug.Log("PrivateKey: " + account.PrivateKey);
                
                //Save the all keys on local device
                PlayerPrefs.SetString("address",account.Address);
                PlayerPrefs.SetString("privatekey", account.PrivateKey);
                PlayerPrefs.SetString("secretwords", secretWords);
            }
            else
            {
                Debug.Log("Something get issue!");
            }

        }
        else
        {
            Debug.Log("Password is too weak!");
        }


    }




}
