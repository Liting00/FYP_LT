using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;

public class RelayManager : Singleton<RelayManager>
{
    [SerializeField]
    private string environment = "production";

    [SerializeField]
    private int maxConnection = 2;

    public bool isRelayEnabled => Transport != null &&
        Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    public async Task<RelayHostData> SetupRelay()
    {
        Debug.Log($"Relay Server starting with max connection {maxConnection}");
        InitializationOptions options = new InitializationOptions()
           .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnection);

        RelayHostData relayHostData = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData
        };
        relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

        Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
            relayHostData.Key, relayHostData.ConnectionData);

        Debug.Log($"Relay Server generated with a join code {relayHostData.JoinCode}");

        return relayHostData;
    }
    public async Task<RelayJoinData> JoinRelay(string joinCode)
    {
        InitializationOptions options = new InitializationOptions()
           .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData relayJoinData = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4,
            JoinCode = joinCode
        };

        Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
            relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

        Debug.Log($"Client Join Game with join code {joinCode}");

        return relayJoinData;
    }
}
