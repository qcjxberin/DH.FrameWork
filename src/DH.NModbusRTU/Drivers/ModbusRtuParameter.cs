using System.ComponentModel;

namespace NewLife.IoT.Drivers;

/// <summary>ModbusRtu参数</summary>
public class ModbusRtuParameter : ModbusParameter
{
    /// <summary>串口</summary>
    [Description("串口")]
    public String PortName { get; set; }

    /// <summary>波特率</summary>
    [Description("波特率")]
    public Int32 Baudrate { get; set; }

    /// <summary>字节超时。数据包间隔，默认10ms</summary>
    [Description("字节超时。数据包间隔，默认10ms")]
    public Int32 ByteTimeout { get; set; } = 10;
}