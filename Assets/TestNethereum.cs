using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Web3;
using System.Numerics;
using System;
using Nethereum.JsonRpc.UnityClient;

public class TestNethereum : MonoBehaviour
{
    // Start is called before the first frame update
    private Web3 web3;
    //public static string url = "HTTP://127.0.0.1:7545";
    public static string url = "https://mainnet.infura.io/v3/f3092718f2f146169c2b1f7d53b99b7a";
    void Start()
    {

        web3 = new Web3(url);
        

        //BigInteger value = GetBalanceFromAddress("0x3a2cF9b278854dB8fbdE18BDEaa6b4E7d4441c7e");
        StartCoroutine(SendEther("0x3a2cF9b278854dB8fbdE18BDEaa6b4E7d4441c7e", "06f3e63691609ea25e027fc2f534e80a6ad5d406bfb1c9af7ae4dafabdd1c68d",
            "0x546206872043462B42e7332170E6Bf6c44cC20ee", 10));

    }

    private BigInteger GetBalanceFromAddress(string address)
    {
        var response = web3.Eth.GetBalance.SendRequestAsync(address);
        BigInteger balance;
        try
        {
            if (!response.IsCanceled)
            {
                if (response.Result != null)
                {
                    balance = response.Result.Value;
                }

            }
        }
        catch (Exception)
        {
            balance = -1;
        }

        return balance;
    }


    private IEnumerator SendEther(string fromAddress, string fromPrivateKey, string toAddress, decimal ether)
    {
        SmartContract smartContract = new SmartContract();
        var chainId = web3.Eth.ChainId.SendRequestAsync();
        var gasPriceRequest = web3.Eth.GasPrice.SendRequestAsync();
        Debug.Log(chainId.Result.Value);

        var gasPrice = Web3.Convert.FromWei(gasPriceRequest.Result.Value);
        Debug.Log(gasPrice);

        var transaction = new EthTransferUnityRequest(url, fromPrivateKey, chainId.Result.Value);
        Debug.Log("Sending ETH...");
        yield return transaction.TransferEther(toAddress, ether, gasPrice);

        if (transaction.Exception != null)
        {
            Debug.Log(transaction.Exception.Message);
            yield break;
        }
        var transactionHash = transaction.Result; 
        Debug.Log("Transaction Success! Transaction Hash: " + transactionHash);
        
        smartContract.GetTransactionInfo(transactionHash,web3);
       

        var fromBalance = GetBalanceFromAddress(fromAddress);
        if (fromBalance >= 0)
        {
            decimal fromWeiAmount = Web3.Convert.FromWei(fromBalance);
            Debug.Log(fromWeiAmount + " ETH");

        }

    }




    // Update is called once per frame
    void Update()
    {

    }





}
