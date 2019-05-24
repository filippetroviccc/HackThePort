using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.JsonRpc.UnityClient;
using UnityEngine;

public class ERC20TokenContractClient : MonoBehaviour {
    
    public string url = "http://localhost:9545";
    public string contractAddr;
    public string privateKey = "0x2105bfe2ea4256a01eb240fcc10fd3ef8dd116965a97b69176146997567acd84";
    public string filler = "0x397d8047d135caf1b4a9ad2451e83226b7feefe8";

    public string[] accounts = new string[10];
    public string[] privateKeys = new string[10];

    //Deployment contract object definition

    public partial class EIP20Deployment : EIP20DeploymentBase
    {
        public EIP20Deployment() : base(BYTECODE) { }

        public EIP20Deployment(string byteCode) : base(byteCode) { }
    }

    public class EIP20DeploymentBase : ContractDeploymentMessage
    {

        public static string BYTECODE = "608060405234801561001057600080fd5b506040516107843803806107848339810160409081528151602080840151838501516060860151336000908152808552959095208490556002849055908501805193959094919391019161006991600391860190610096565b506004805460ff191660ff8416179055805161008c906005906020840190610096565b5050505050610131565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106100d757805160ff1916838001178555610104565b82800160010185558215610104579182015b828111156101045782518255916020019190600101906100e9565b50610110929150610114565b5090565b61012e91905b80821115610110576000815560010161011a565b90565b610644806101406000396000f3006080604052600436106100ae5763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166306fdde0381146100b3578063095ea7b31461013d57806318160ddd1461017557806323b872dd1461019c57806327e235e3146101c6578063313ce567146101e75780635c6581651461021257806370a082311461023957806395d89b411461025a578063a9059cbb1461026f578063dd62ed3e14610293575b600080fd5b3480156100bf57600080fd5b506100c86102ba565b6040805160208082528351818301528351919283929083019185019080838360005b838110156101025781810151838201526020016100ea565b50505050905090810190601f16801561012f5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b34801561014957600080fd5b50610161600160a060020a0360043516602435610348565b604080519115158252519081900360200190f35b34801561018157600080fd5b5061018a6103ae565b60408051918252519081900360200190f35b3480156101a857600080fd5b50610161600160a060020a03600435811690602435166044356103b4565b3480156101d257600080fd5b5061018a600160a060020a03600435166104b7565b3480156101f357600080fd5b506101fc6104c9565b6040805160ff9092168252519081900360200190f35b34801561021e57600080fd5b5061018a600160a060020a03600435811690602435166104d2565b34801561024557600080fd5b5061018a600160a060020a03600435166104ef565b34801561026657600080fd5b506100c861050a565b34801561027b57600080fd5b50610161600160a060020a0360043516602435610565565b34801561029f57600080fd5b5061018a600160a060020a03600435811690602435166105ed565b6003805460408051602060026001851615610100026000190190941693909304601f810184900484028201840190925281815292918301828280156103405780601f1061031557610100808354040283529160200191610340565b820191906000526020600020905b81548152906001019060200180831161032357829003601f168201915b505050505081565b336000818152600160209081526040808320600160a060020a038716808552908352818420869055815186815291519394909390927f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925928290030190a350600192915050565b60025481565b600160a060020a03831660008181526001602090815260408083203384528252808320549383529082905281205490919083118015906103f45750828110155b15156103ff57600080fd5b600160a060020a038085166000908152602081905260408082208054870190559187168152208054849003905560001981101561046157600160a060020a03851660009081526001602090815260408083203384529091529020805484900390555b83600160a060020a031685600160a060020a03167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef856040518082815260200191505060405180910390a3506001949350505050565b60006020819052908152604090205481565b60045460ff1681565b600160209081526000928352604080842090915290825290205481565b600160a060020a031660009081526020819052604090205490565b6005805460408051602060026001851615610100026000190190941693909304601f810184900484028201840190925281815292918301828280156103405780601f1061031557610100808354040283529160200191610340565b3360009081526020819052604081205482111561058157600080fd5b3360008181526020818152604080832080548790039055600160a060020a03871680845292819020805487019055805186815290519293927fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef929181900390910190a350600192915050565b600160a060020a039182166000908152600160209081526040808320939094168252919091522054905600a165627a7a7230582084c618322109054a21a57e27075384a6172ab854e4b2c2d35062a964a6bf593f0029";

        public EIP20DeploymentBase() : base(BYTECODE) { }

        public EIP20DeploymentBase(string byteCode) : base(byteCode) { }

        [Parameter("uint256", "_initialAmount", 1)]
        public int InitialAmount { get; set; }
        [Parameter("string", "_tokenName", 2)]
        public string TokenName { get; set; }
        [Parameter("uint8", "_decimalUnits", 3)]
        public byte DecimalUnits { get; set; }
        [Parameter("string", "_tokenSymbol", 4)]
        public string TokenSymbol { get; set; }

    }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public string To { get; set; }
        [Parameter("uint256", "_value", 2)]
        public int Value { get; set; }
    }

    public partial class TransferFunction : TransferFunctionBase
    {

    }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }

    [FunctionOutput]
    public class BalanceOfFunctionOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public int Balance { get; set; }
    }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {

        [Parameter("address", "_from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "_to", 2, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 3, false)]
        public virtual int Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase
    {
        public static EventABI GetEventABI()
        {
            return EventExtensions.GetEventABI<TransferEventDTO>();
        }
    }

    // Use this for initialization
    void Start ()
    {
        this.accounts[0] = "0x7692678b73e7b0b0d43d97f78f5005c9877427da";
        this.accounts[1] = "0x7d839bd03f9364d738f0dca17299ed9592342480";
        this.accounts[2] = "0x465115477a5505182f87879cca9cdaa4dfb9169f";
        this.accounts[3] = "0x8b9c879e993c55354afde83fb985148eeabb6fda";
        this.accounts[4] = "0xcf4910bf817add21b136214365f33c3bdab73799";
        this.accounts[5] = "0x8ec9343e42e197ea2443d5fd0469de5a49289306";
        this.accounts[6] = "0x017443a530bf58d7dbc539234cc21b4bdfe88efe";

        this.privateKeys[0] = "0xe88530c51aee54ab0b14bd3e463ea1220387b6653702afe39967c39d4905d8da";
        this.privateKeys[1] = "0x785344aac89ae974ffcff96d7fd997368ad6d9c91fc8402d50a434bec4f8cd1b";
        this.privateKeys[2] = "0x52dc2aea009e5265114cf581c98b37f14122602ce725c2311f8b7fb85c99d7b0";
        this.privateKeys[3] = "0x9bb443f754be6781719c4d05e4d76cc3bfb7dda0f9ea1401d5a32dbc90447591";
        this.privateKeys[4] = "0xe5d44b3c9dcd296451878ede8cd9fe383c7078a0f5f4f02dd72f7b0308a";
        this.privateKeys[5] = "0xe23192f0a0daab4c9746b4ff1e4170be1af46e5ebffda9eee5ef87e5af01d8";
        this.privateKeys[6] = "0xa3a838f51497d40e89c15ab2c05f647997fdbff100cb9a5a964e5a565093ec51";
        //StartCoroutine(AAAA());
    }

    public IEnumerator AAAA()
    {
        yield return StartCoroutine(DeployAndFillAccounts());
        // yield return StartCoroutine(Transfer(this.privateKeys[1], this.accounts[1], this.accounts[2], 1));
    }

    public IEnumerator DeployAndFillAccounts()
    {
        var url = "http://localhost:9545";
        var FillerPrivateKey = "0x2105bfe2ea4256a01eb240fcc10fd3ef8dd116965a97b69176146997567acd84";
        var filler = "0x397d8047d135caf1b4a9ad2451e83226b7feefe8";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(this.url, FillerPrivateKey, filler);
        
        var deployContract = new EIP20Deployment()
        {
            InitialAmount = 999999999,
            FromAddress = filler,
            TokenName = "TST",
            TokenSymbol = "TST"
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<EIP20DeploymentBase>(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;
        Debug.Log(deploymentReceipt.ContractAddress);
        this.contractAddr = deploymentReceipt.ContractAddress;

        Debug.Log("Deployment contract address:" + deploymentReceipt.ContractAddress);

        for (int i = 0; i < 10; i++)
        { 
            //Query request using our acccount and the contracts address (no parameters needed and default values)
            var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfFunctionOutput>(url, filler);
            yield return queryRequest.Query(new BalanceOfFunction(){Owner = this.filler}, deploymentReceipt.ContractAddress);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result;
            Debug.Log(dtoResult.Balance);
            
            var transactionTransferRequest = new TransactionSignedUnityRequest(url, FillerPrivateKey, filler);

            var newAddress = accounts[i];

            var transactionMessage = new TransferFunction
            {
                FromAddress = filler,
                To = newAddress,
                Value = 9999,
            };

            yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, deploymentReceipt.ContractAddress);

            var transactionTransferHash = transactionTransferRequest.Result;

            Debug.Log("Transfer txn hash:" + transactionHash);

            transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
            yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
            var transferReceipt = transactionReceiptPolling.Result;

            var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>(); 
            Debug.Log("Transferd amount from event: " + transferEvent[0].Event.Value);
            
            var getLogsRequest = new EthGetLogsUnityRequest(url);
            
            var eventTransfer = TransferEventDTO.GetEventABI();
            yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(deploymentReceipt.ContractAddress, this.filler));

            var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

            Debug.Log("Transferd amount from get logs event: " + eventDecoded[0].Event.Value);
        }
    }

    public IEnumerator Transfer(string privateKeyFrom, string addressFrom, string to, int value)
    {
        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfFunctionOutput>(url, addressFrom);
        yield return queryRequest.Query(new BalanceOfFunction(){Owner = addressFrom}, this.contractAddr);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.Balance);

        var transactionTransferRequest = new TransactionSignedUnityRequest(this.url, privateKeyFrom, this.filler);
        Debug.Log(value);
        var transactionMessage = new TransferFunction
        {
            FromAddress = addressFrom,
            To = to,
            Value = value,
        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, this.contractAddr);
        var transactionTransferHash = transactionTransferRequest.Result;

        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
        var transferReceipt = transactionReceiptPolling.Result;

        var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>();
        Debug.Log("Transfered amount from event: " + transferEvent[0].Event.Value);

        var getLogsRequest = new EthGetLogsUnityRequest(url);

        var eventTransfer = TransferEventDTO.GetEventABI();
        yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(this.contractAddr, this.filler));

        var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

        Debug.Log("Transfered amount from get logs event: " + eventDecoded[0].Event.Value);
    }

    //Sample of new features / requests
    public IEnumerator DeployAndTransferToken()
    {
        var url = "http://localhost:9545";
        var privateKey = "0x2105bfe2ea4256a01eb240fcc10fd3ef8dd116965a97b69176146997567acd84";
        var account = "0x397d8047d135caf1b4a9ad2451e83226b7feefe8";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, account);


        var deployContract = new EIP20Deployment()
        {
            InitialAmount = 10000,
            FromAddress = account,
            TokenName = "TST",
            TokenSymbol = "TST"
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<EIP20DeploymentBase>(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;

        Debug.Log("Deployment contract address:" + deploymentReceipt.ContractAddress);

        //Query request using our acccount and the contracts address (no parameters needed and default values)
        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfFunctionOutput>(url, account);
        yield return queryRequest.Query(new BalanceOfFunction(){Owner = account}, deploymentReceipt.ContractAddress);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.Balance);


        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey, account);

        var newAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";

        var transactionMessage = new TransferFunction
        {
            FromAddress = account,
            To = newAddress,
            Value = 1000,

        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, deploymentReceipt.ContractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;

        Debug.Log("Transfer txn hash:" + transactionHash);

        transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
        var transferReceipt = transactionReceiptPolling.Result;

        var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>();
        Debug.Log("Transferd amount from event: " + transferEvent[0].Event.Value);

        var getLogsRequest = new EthGetLogsUnityRequest(url);

        var eventTransfer = TransferEventDTO.GetEventABI();
        yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(deploymentReceipt.ContractAddress, account));

        var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

        Debug.Log("Transferd amount from get logs event: " + eventDecoded[0].Event.Value);
    }



    // Update is called once per frame
    void Update () {
		
	}

    public struct KeyPair
    {
        public string pub;
        public string priv;
    }

    public KeyPair GetRandomAccount()
    {
        int index = Random.RandomRange(1, this.accounts.Length - 1);
        KeyPair result = new KeyPair();
        result.pub = this.accounts[index];
        result.priv = this.privateKeys[index];
        return result;
    }

    public KeyPair getPlayerAccount()
    {
        KeyPair result = new KeyPair();
        result.pub = accounts[0];
        result.priv = privateKeys[0];
        return result;
    }
}

/// <summary>
/// Abstracts interaction with the Blueprint contract.
/// </summary>
//public class ERC20TokenContractClient : IDisposable {
//    // all events
//    public delegate void ValueChangedEventHandler(string key, string value);
//    public delegate void ValueRemovedEventHandler(string key);
//    
//    public delegate void TransferEventHandler(byte[] from, byte[] to, byte[] value);
//    public delegate void ApprovalEventHandler(byte[] owner, byte[] spender, byte[] value);
//
//    public event TransferEventHandler TransferEvent;
//    public event ApprovalEventHandler ApprovalEvent;
//
//    private readonly string backendHost;
//    private readonly string abi;
//    private readonly Address contractAddr;
//    private readonly byte[] privateKey;
//    private readonly byte[] publicKey;
//    private readonly ILogger logger;
//    private readonly Address address;
//    private readonly ConcurrentQueue<Action> eventActionsQueue = new ConcurrentQueue<Action>();
//
//    private DAppChainClient client;
//    private EvmContract contract;
//    private IRpcClient reader;
//    private IRpcClient writer;
//
//    public ERC20TokenContractClient(string backendHost,string contractAddr ,string abi, byte[] privateKey, byte[] publicKey, ILogger logger)
//    {
//        this.backendHost = backendHost;
//        this.abi = abi;
//        this.contractAddr = Address.FromString(contractAddr);
//        this.privateKey = privateKey;
//        this.publicKey = publicKey;
//        this.logger = logger;
//        this.address = Address.FromPublicKey(this.publicKey);
//    }
//
//    /// <summary>
//    /// Dispatches queued events. Clears the queue afterwards.
//    /// </summary>
//    public void DispatchQueuedEvents() {
//        if (this.eventActionsQueue.IsEmpty)
//            return;
//
//        Action eventAction;
//        while (this.eventActionsQueue.TryDequeue(out eventAction)) {
//            eventAction();
//        }
//    }
//
//    /// <summary>
//    /// Establishes initial connection with the contract.
//    /// </summary>
//    public async Task ConnectToContract()
//    {
//        if (this.contract == null)
//        {
//            this.contract = await GetContract(this.contractAddr);
//            this.contract.EventReceived += EventReceivedHandler;
//        }
//    }
//
//    #region Contract public functions
//
//    /* Those methods mirror the functions of the Solidity contract,
//       and are made for convenience. */
//    
//    // Transactions
//    public async Task Transfer(string to, string value) {
//        await ConnectToContract();
//        object[] tmp = new object[2];
//        tmp[0] = to;
//        tmp[1] = value;
//        await this.contract.CallAsync("transfer", tmp); // add proto
//    }
//    
//    public async Task Approve(string spender, string value) {
//        await ConnectToContract();
//        object[] tmp = new object[2];
//        tmp[0] = spender;
//        tmp[1] = value;
//        await this.contract.CallAsync("approve", tmp);
//    }
//    
//    public async Task TransferFrom(byte[] from, byte[] to, byte[] value) {
//        await ConnectToContract();
//        object[] tmp = new object[3];
//        tmp[0] = from;
//        tmp[1] = to;
//        tmp[2] = value;
//        await this.contract.CallAsync("transferFrom", tmp);
//    }
//    
//    // Simple call, storage lookup
//    public async Task<int> BalanceOf(string owner) {
//        await ConnectToContract();
//        return await this.contract.StaticCallSimpleTypeOutputAsync<int>("balanceOf", owner); // @TODO: convert to to human readable
//    }
//    
//    public async Task<byte[]> TotalSupply() {
//        await ConnectToContract();
//        return await this.contract.StaticCallSimpleTypeOutputAsync<byte[]>("totalSupply");
//    }
//
//    #endregion
//
//    public void Dispose() {
//        if (this.contract != null) {
//            this.contract.EventReceived -= EventReceivedHandler;
//        }
//
//        this.client?.Dispose();
//        this.reader?.Dispose();
//        this.writer?.Dispose();
//    }
//
//    /// <summary>
//    /// Connects to the DAppChain and returns an instance of a contract.
//    /// </summary>
//    /// <returns></returns>
//    private async Task<EvmContract> GetContract(Address contractAddr)
//    {
//        this.writer = RpcClientFactory.Configure()
//            .WithLogger(this.logger)
//            .WithWebSocket("ws://" + this.backendHost + ":46657/websocket")
//            .Create();
//
//        this.reader = RpcClientFactory.Configure()
//            .WithLogger(this.logger)
//            .WithWebSocket("ws://" + this.backendHost + ":9999/queryws")
//            .Create();
//
//        this.client = new DAppChainClient(this.writer, this.reader)
//            { Logger = this.logger };
//
//        // required middleware
//        this.client.TxMiddleware = new TxMiddleware(new ITxMiddlewareHandler[]
//        {
//            new NonceTxMiddleware(this.publicKey, this.client),
//            new SignedTxMiddleware(this.privateKey)
//        });
//
//        // If 'truffle deploy' was used to deploy the contract,
//        // you will have to use the contract address directly
//        // instead of resolving it from contract name
//        // import from abi??
//        // Address contractAddr = await this.client.ResolveContractAddressAsync("PirateToken");
//        EvmContract evmContract = new EvmContract(this.client, contractAddr, this.address, this.abi);
//
//        return evmContract;
//    }
//
//    /// <summary>
//    /// This method receives raw EVM events from the DAppChain.
//    /// Add decoding of your own events here.
//    /// </summary>
//    /// <remarks>
//    /// Events are not dispatched immediately.
//    /// Instead, they are queued to allow dispatching them when it is appropriate.
//    /// </remarks>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void EventReceivedHandler(object sender, EvmChainEventArgs e) {
//        switch (e.EventName) {
//            case "Transfer": {
//                TransferEventData eventDto = e.DecodeEventDto<TransferEventData>();
//                this.eventActionsQueue.Enqueue(() => this.TransferEvent?.Invoke(eventDto.From, eventDto.To, eventDto.Value));
//                break;
//            }
//            case "Approval": {
//                ApprovalEventData eventDto = e.DecodeEventDto<ApprovalEventData>();
//                this.eventActionsQueue.Enqueue(() => this.ApprovalEvent?.Invoke(eventDto.Owner, eventDto.Spender, eventDto.Value));
//                break;
//            }
//            default:
//                throw new ArgumentOutOfRangeException($"Unknown event {e.EventName}");
//        }
//    }
//
//    #region Event Data Transfer Objects
//
//    private class TransferEventData
//    {
//        [Parameter("byte[]")]
//        public byte[] From { get; set; }
//
//        [Parameter("byte[]")]
//        public byte[] To { get; set; }
//        
//        [Parameter("byte[]")]
//        public byte[] Value { get; set; }
//    }
//
//    private class ApprovalEventData
//    {
//        [Parameter("byte[]")]
//        public byte[] Owner { get; set; }
//
//        [Parameter("byte[]")]
//        public byte[] Spender { get; set; }
//        
//        [Parameter("byte[]")]
//        public byte[] Value { get; set; }
//    }
//    
//    #endregion
//}
