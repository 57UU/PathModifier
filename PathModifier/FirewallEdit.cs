using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PathModifier;

internal static class FirewallEdit
{
    public static void Open(int portNumber)
    {
        try
        {
            _open_com(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
            _open_com(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            //_open_clsid(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
            //_open_clsid(NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            Console.WriteLine($"Port {portNumber} opened successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening port {portNumber}: " + ex.Message);
        }
        void _open_com(NET_FW_IP_PROTOCOL_ protocol)
        {
            // 获取 INetFwMgr 的实例
            Type netFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr fwMgr = (INetFwMgr)Activator.CreateInstance(netFwMgrType);

            // 获取 INetFwOpenPorts 的实例
            INetFwOpenPorts openPorts = fwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts;

            // 创建 INetFwOpenPort 的实例
            Type netFwOpenPortType = Type.GetTypeFromProgID("HNetCfg.FwOpenPort", false);
            INetFwOpenPort port = (INetFwOpenPort)Activator.CreateInstance(netFwOpenPortType);

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
        void _open_reflect(NET_FW_IP_PROTOCOL_ protocol)
        {
            // 获取 INetFwMgr 的类型
            Type netFwMgrType = Type.GetType("HNetCfg.FwMgr")!;

            // 使用反射创建 INetFwMgr 的实例
            object fwMgrInstance = Activator.CreateInstance(netFwMgrType);

            // 获取 INetFwOpenPorts 的实例
            PropertyInfo property = netFwMgrType.GetProperty("LocalPolicy");
            object localPolicy = property.GetValue(fwMgrInstance);
            property = localPolicy.GetType().GetProperty("CurrentProfile");
            object profile = property.GetValue(localPolicy);
            PropertyInfo openPortsProperty = profile.GetType().GetProperty("GloballyOpenPorts");
            INetFwOpenPorts openPorts = (INetFwOpenPorts)openPortsProperty.GetValue(profile);

            // 创建 INetFwOpenPort 的类型
            Type netFwOpenPortType = Type.GetType("HNetCfg.FwOpenPort");

            // 使用反射创建 INetFwOpenPort 的实例
            object portInstance = Activator.CreateInstance(netFwOpenPortType);

            // 设置端口参数
            PropertyInfo portNameProperty = netFwOpenPortType.GetProperty("Name");
            portNameProperty.SetValue(portInstance, $"PathModifier:{portNumber}:{protocol.ToString().Split('_').Last()}");
            PropertyInfo portProperty = netFwOpenPortType.GetProperty("Port");
            portProperty.SetValue(portInstance, portNumber);
            PropertyInfo protocolProperty = netFwOpenPortType.GetProperty("Protocol");
            protocolProperty.SetValue(portInstance, (int)protocol);
            PropertyInfo scopeProperty = netFwOpenPortType.GetProperty("Scope");
            scopeProperty.SetValue(portInstance, NET_FW_SCOPE_.NET_FW_SCOPE_ALL);
            PropertyInfo enabledProperty = netFwOpenPortType.GetProperty("Enabled");
            enabledProperty.SetValue(portInstance, true);

            // 添加端口到防火墙规则
            MethodInfo addMethod = openPortsProperty.PropertyType.GetMethod("Add");
            addMethod.Invoke(openPorts, new[] { portInstance });
        }
        void _open_clsid(NET_FW_IP_PROTOCOL_ protocol)
        {
            // 定义 COM 类型的 CLSID
            Guid fwMgrClsid = new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}");
            Guid fwOpenPortClsid = new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}");

            // 使用 Type.GetTypeFromCLSID 获取类型
            Type netFwMgrType = Type.GetTypeFromCLSID(fwMgrClsid);
            Type netFwOpenPortType = Type.GetTypeFromCLSID(fwOpenPortClsid);

            // 创建 INetFwMgr 的实例
            INetFwMgr fwMgr = (INetFwMgr)Activator.CreateInstance(netFwMgrType);
            INetFwOpenPorts openPorts = fwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts;

            // 创建 INetFwOpenPort 的实例
            INetFwOpenPort port = (INetFwOpenPort)Activator.CreateInstance(netFwOpenPortType);

            // 设置端口参数
            port.Name = $"PathModifier:{portNumber}:{protocol.ToString().Split('_').Last()}";
            port.Port = portNumber;
            port.Protocol =protocol;
            port.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            port.Enabled = true;

            // 添加端口到防火墙规则
            openPorts.Add(port);
        }

    }
}
