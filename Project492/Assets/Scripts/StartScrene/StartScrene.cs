using System.Diagnostics.Contracts;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb; // for interacting with the SDK
using System.Threading.Tasks; // for some async functionality
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartScrene : MonoBehaviour
{
    public GameObject ConnectedState;
    public GameObject DisconnectedState;
    public Button connectWalletButton;
    public Button playButton;

    [Header("Address")]
    string address = null;

    [Header("Canvas")]
    public GameObject menuCanvas;
    public TextMeshProUGUI textAddress;

    private ThirdwebSDK sdk;
    void Awake()
    {
        connectWalletButton.onClick.AddListener(ConnectWallet);
        playButton.onClick.AddListener(ConnectingGame);
        DontDestroyOnLoad(this);
    }
    void Start()
    {

        sdk = new ThirdwebSDK("optimism-goerli");

    }

    public async void ConnectWallet()
    {
        string tmpAddress =
        await sdk.wallet.Connect(new WalletConnection()
        {
            provider = WalletProvider.MetaMask,
            chainId = 5 // Switch the wallet Goerli on connection
        });

        // Disable disconnected state
        DisconnectedState.SetActive(false);

        // Enable connected state
        ConnectedState.SetActive(true);

        // Set the ConnectedStates "Address" GameObjects text to the wallet address
        textAddress.text = tmpAddress;
        address = tmpAddress;
        //float balanceFloat = FloatComparisonMode.Parse(balance);

        //string tmpBalance = await CheckBalance();
        //textBalance.text = "Balance : " + tmpBalance;
        //balance = tmpBalance;
    }

    public async Task<string> CheckBalance()
    {
        // Connect to the smart contract
        // Replace this with your own contract address
        Thirdweb.Contract contract = sdk.GetContract("0x69BC6d095517951Df17f70f38746Bc27CE1C8A62");

        // Replace this with your NFT's token ID
        string tokenId = "1";

        // Check the balance of the wallet for this NFT
        string balance = await contract.ERC1155.Balance(tokenId);
        return balance;
    }

    void ConnectingGame()
    {
        ConnectedState.SetActive(false);
        menuCanvas.SetActive(false);
    }
}
