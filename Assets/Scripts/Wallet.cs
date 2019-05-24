//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class KeyPair
//{
//    public byte[] PrivateKey;
//    public byte[] PublicKey;
//    public Address Address;
//}
//
//public class Wallet : MonoBehaviour
//{
//    public string BackendHost = "127.0.0.1";
//    public TextAsset ContractAbi;
//    public TextAsset ContractAddress;
//
//    private static readonly string PrivateKey =
//        "0x2105bfe2ea4256a01eb240fcc10fd3ef8dd116965a97b69176146997567acd84";
//    
//    public InputField TransferValueText; // VELIKO -  OVO TREBA DA ASSIGN NA NEKI FIELD U SCENI
//    public Text TranferValuePlaceholderText;
//    public Text BalanceOfText;
//
//    private Address PlayerAddress;
//    private Address EnemyAddress;
//
//    private ERC20TokenContractClient client;
//
//    #region Unity messages
//
//    private void Start()
//    {
//        // In a real game, the private key should be stored somewhere.
//        // Essentially, it represents a users' account/identity.
//        // But for this sample's simplicity, just generate a new private key each time.
//        
////        KeyPair[] accounts = new KeyPair[10];
////        for (int i = 0; i < 10; i++)
////        {
////            byte[] privateKey = CryptoUtils.GeneratePrivateKey();
////            byte[] publicKey = CryptoUtils.PublicKeyFromPrivateKey(privateKey);
////            Address address = Address.FromPublicKey(publicKey);
////
////            accounts[0].Address = address;
////            accounts[0].PublicKey = publicKey;
////            accounts[0].PrivateKey = privateKey;
////        }
//        
//        byte[] privateKeyPlayer = CryptoUtils.GeneratePrivateKey();
//        byte[] publicKeyPlayer = CryptoUtils.PublicKeyFromPrivateKey(privateKeyPlayer);
//        this.PlayerAddress = Address.FromPublicKey(publicKeyPlayer);
//
//
//        byte[] privateKeyEnemyDummy = CryptoUtils.GeneratePrivateKey();
//        byte[] publicKeyEnemyDummy = CryptoUtils.PublicKeyFromPrivateKey(privateKeyEnemyDummy);
//        this.EnemyAddress = Address.FromPublicKey(publicKeyEnemyDummy);
//        
//        
////        KeyPair filler = new KeyPair();
////        filler.PrivateKey = CryptoUtils.HexStringToBytes(PrivateKey);
////        filler.PublicKey = CryptoUtils.PublicKeyFromPrivateKey(filler.PrivateKey);
////        filler.Address = Address.FromPublicKey(filler.PublicKey);
//        
////        // fill them from private_key in dappChain (tmp contract client that will distribute tokens)
////        ERC20TokenContractClient tmpClient = new ERC20TokenContractClient(
////            this.BackendHost,
////            this.ContractAddress.text,
////            this.ContractAbi.text,
////            filler.PrivateKey,
////            filler.PublicKey,
////            NullLogger.Instance
////        );
//        
//        this.client = new ERC20TokenContractClient(
//            this.BackendHost,
//            this.ContractAddress.text,
//            this.ContractAbi.text,
//            privateKeyPlayer,
//            publicKeyPlayer,
//            NullLogger.Instance // Use Debug.unityLogger for more logs
//        );
//
//        // Subscribe to the event emitted by the contract
//        this.client.TransferEvent += ClientOnTransfer;
//        // this.client.Approval += ClientOnApprove;
//    }
//
//    private void OnDestroy()
//    {
//        this.client.TransferEvent -= ClientOnTransfer;
//        // this.client.Approval -= ClientOnApprove;
//    }
//
//    private void Update()
//    {
//        // Dispatch the events.
//        // This is done here to make sure the are dispatched on the main thread.
//        this.client.DispatchQueuedEvents();
//    }
//
//    #endregion
//
//    #region UI event handlers
//
//    public async void TransferClickHandler()
//    {
//        this.TranferValuePlaceholderText.text = "Transfering...";
//        await this.client.Transfer(this.PlayerAddress.ToString(), this.TransferValueText.text.ToString());
//        this.TranferValuePlaceholderText.text = "Transfered!";
//    }
//
//    public async void BalanceOfClickHandler()
//    {
//        this.BalanceOfText.text = "Fetching token balance...";
//        var value = await this.client.BalanceOf(this.PlayerAddress.ToString());
//        Debug.Log(value.ToString());
//        this.BalanceOfText.text =
//            String.Format("Balance of {0} is {1}", this.PlayerAddress.ToString(), value.ToString());
//    }
//
//    #endregion
//
//    #region Contract event handlers
//
//    private void ClientOnTransfer(byte[] from, byte[] to, byte[] value)
//    {
//        Debug.LogFormat("Value changed: from address: '{0}', to address: '{1}' and value: '{2}'", from, to, value);
//    }
//    
//    public static byte[] ToByteArray(string hexString)
//    {
//        byte[] retval = new byte[hexString.Length / 2];
//        for (int i = 0; i < hexString.Length; i += 2)
//            retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
//        return retval;
//    }
//
//    #endregion
//}