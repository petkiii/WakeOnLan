using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;

namespace WakeOnLan;

public class Target
{
    [Key]
    public string NormalizedName { get; init; }
    public string Name { get; init; }
    public string Host { get; init; }
    public int Port { get; init; }
    public PhysicalAddress MacAddress { get; init; }
    
}