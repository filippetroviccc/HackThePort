using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;
using Loom.Nethereum.ABI.FunctionEncoding.Attributes;
using Loom.Client;
using Loom.Client.Samples;
using UnityEngine;

/// <summary>
/// Abstracts interaction with the Blueprint contract.
/// </summary>
public class ERC20TokenContractClient : IDisposable {
    // all events
    public delegate void ValueChangedEventHandler(string key, string value);
    public delegate void ValueRemovedEventHandler(string key);
    
    public delegate void TransferEventHandler(byte[] from, byte[] to, byte[] value);
    public delegate void ApprovalEventHandler(byte[] owner, byte[] spender, byte[] value);

    public event TransferEventHandler TransferEvent;
    public event ApprovalEventHandler ApprovalEvent;

    private readonly string backendHost;
    private readonly string abi;
    private readonly Address contractAddr;
    private readonly byte[] privateKey;
    private readonly byte[] publicKey;
    private readonly ILogger logger;
    private readonly Address address;
    private readonly ConcurrentQueue<Action> eventActionsQueue = new ConcurrentQueue<Action>();

    private DAppChainClient client;
    private EvmContract contract;
    private IRpcClient reader;
    private IRpcClient writer;

    public ERC20TokenContractClient(string backendHost,string contractAddr ,string abi, byte[] privateKey, byte[] publicKey, ILogger logger)
    {
        this.backendHost = backendHost;
        this.abi = abi;
        this.contractAddr = Address.FromString(contractAddr);
        this.privateKey = privateKey;
        this.publicKey = publicKey;
        this.logger = logger;
        this.address = Address.FromPublicKey(this.publicKey);
    }

    /// <summary>
    /// Dispatches queued events. Clears the queue afterwards.
    /// </summary>
    public void DispatchQueuedEvents() {
        if (this.eventActionsQueue.IsEmpty)
            return;

        Action eventAction;
        while (this.eventActionsQueue.TryDequeue(out eventAction)) {
            eventAction();
        }
    }

    /// <summary>
    /// Establishes initial connection with the contract.
    /// </summary>
    public async Task ConnectToContract()
    {
        if (this.contract == null)
        {
            this.contract = await GetContract(this.contractAddr);
            this.contract.EventReceived += EventReceivedHandler;
        }
    }

    #region Contract public functions

    /* Those methods mirror the functions of the Solidity contract,
       and are made for convenience. */
    
    // Transactions
    public async Task Transfer(string to, string value) {
        await ConnectToContract();
        object[] tmp = new object[2];
        tmp[0] = to;
        tmp[1] = value;
        await this.contract.CallAsync("transfer", tmp); // add proto
    }
    
    public async Task Approve(string spender, string value) {
        await ConnectToContract();
        object[] tmp = new object[2];
        tmp[0] = spender;
        tmp[1] = value;
        await this.contract.CallAsync("approve", tmp);
    }
    
    public async Task TransferFrom(byte[] from, byte[] to, byte[] value) {
        await ConnectToContract();
        object[] tmp = new object[3];
        tmp[0] = from;
        tmp[1] = to;
        tmp[2] = value;
        await this.contract.CallAsync("transferFrom", tmp);
    }
    
    // Simple call, storage lookup
    public async Task<int> BalanceOf(string owner) {
        await ConnectToContract();
        return await this.contract.StaticCallSimpleTypeOutputAsync<int>("balanceOf", owner); // @TODO: convert to to human readable
    }
    
    public async Task<byte[]> TotalSupply() {
        await ConnectToContract();
        return await this.contract.StaticCallSimpleTypeOutputAsync<byte[]>("totalSupply");
    }

    #endregion

    public void Dispose() {
        if (this.contract != null) {
            this.contract.EventReceived -= EventReceivedHandler;
        }

        this.client?.Dispose();
        this.reader?.Dispose();
        this.writer?.Dispose();
    }

    /// <summary>
    /// Connects to the DAppChain and returns an instance of a contract.
    /// </summary>
    /// <returns></returns>
    private async Task<EvmContract> GetContract(Address contractAddr)
    {
        this.writer = RpcClientFactory.Configure()
            .WithLogger(this.logger)
            .WithWebSocket("ws://" + this.backendHost + ":46657/websocket")
            .Create();

        this.reader = RpcClientFactory.Configure()
            .WithLogger(this.logger)
            .WithWebSocket("ws://" + this.backendHost + ":9999/queryws")
            .Create();

        this.client = new DAppChainClient(this.writer, this.reader)
            { Logger = this.logger };

        // required middleware
        this.client.TxMiddleware = new TxMiddleware(new ITxMiddlewareHandler[]
        {
            new NonceTxMiddleware(this.publicKey, this.client),
            new SignedTxMiddleware(this.privateKey)
        });

        // If 'truffle deploy' was used to deploy the contract,
        // you will have to use the contract address directly
        // instead of resolving it from contract name
        // import from abi??
        // Address contractAddr = await this.client.ResolveContractAddressAsync("PirateToken");
        EvmContract evmContract = new EvmContract(this.client, contractAddr, this.address, this.abi);

        return evmContract;
    }

    /// <summary>
    /// This method receives raw EVM events from the DAppChain.
    /// Add decoding of your own events here.
    /// </summary>
    /// <remarks>
    /// Events are not dispatched immediately.
    /// Instead, they are queued to allow dispatching them when it is appropriate.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventReceivedHandler(object sender, EvmChainEventArgs e) {
        switch (e.EventName) {
            case "Transfer": {
                TransferEventData eventDto = e.DecodeEventDto<TransferEventData>();
                this.eventActionsQueue.Enqueue(() => this.TransferEvent?.Invoke(eventDto.From, eventDto.To, eventDto.Value));
                break;
            }
            case "Approval": {
                ApprovalEventData eventDto = e.DecodeEventDto<ApprovalEventData>();
                this.eventActionsQueue.Enqueue(() => this.ApprovalEvent?.Invoke(eventDto.Owner, eventDto.Spender, eventDto.Value));
                break;
            }
            default:
                throw new ArgumentOutOfRangeException($"Unknown event {e.EventName}");
        }
    }

    #region Event Data Transfer Objects

    private class TransferEventData
    {
        [Parameter("byte[]")]
        public byte[] From { get; set; }

        [Parameter("byte[]")]
        public byte[] To { get; set; }
        
        [Parameter("byte[]")]
        public byte[] Value { get; set; }
    }

    private class ApprovalEventData
    {
        [Parameter("byte[]")]
        public byte[] Owner { get; set; }

        [Parameter("byte[]")]
        public byte[] Spender { get; set; }
        
        [Parameter("byte[]")]
        public byte[] Value { get; set; }
    }
    
    #endregion
}
