using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathModifier;

internal static class FirewallEdit
{
    public static void Open(int portNumber)
    {
        try
        {
            _open(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
            _open(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            Console.WriteLine($"Port {portNumber} opened successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening port {portNumber}: " + ex.Message);
        }
        void _open(NET_FW_IP_PROTOCOL_ protocol)
        {
            // 创建防火墙管理器的实例
            Type netFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr fwMgr = (INetFwMgr)Activator.CreateInstance(netFwMgrType);
            INetFwOpenPorts openPorts = fwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts;

            // 创建一个新的开放端口
            Type netFwOpenPortType = Type.GetTypeFromProgID("HNetCfg.FwOpenPort", false);
            INetFwOpenPort port = (INetFwOpenPort)Activator.CreateInstance(netFwOpenPortType)!;

            // 设置端口参数
            port.Name = $"PathModifier:{portNumber}:{protocol.ToString().Split('_').Last()}"; // 端口名称
            port.Port = portNumber; // 端口号
            port.Protocol = protocol; // 协议类型
            port.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL; // 作用域
            port.Enabled = true; // 是否启用

            // 检查端口名称是否已经存在
            foreach (INetFwOpenPort existingPort in openPorts)
            {
                if (existingPort.Name == port.Name)
                {
                    Console.WriteLine("firewall policy already exists. ignore");
                    return;
                }
            }
            // 添加开放端口到防火墙
            openPorts.Add(port);
        }

    }
}
