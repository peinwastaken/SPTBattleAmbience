using Fika.Core.Networking.LiteNetLib.Utils;
using UnityEngine;

namespace SPTBattleAmbienceFika.Packets;

public class AmbienceEventPacket : INetSerializable
{
    public string SoundType = string.Empty;
    public string SoundCategory = string.Empty;
    public string ClipName = string.Empty;
    public Vector3 Position = Vector3.zero;
    public float Volume = 0f;
    public int Rolloff = 0;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(SoundType);
        writer.Put(SoundCategory);
        writer.Put(ClipName);
        writer.PutUnmanaged<Vector3>(Position);
        writer.Put(Volume);
        writer.Put(Rolloff);
    }

    public void Deserialize(NetDataReader reader)
    {
        SoundType = reader.GetString();
        SoundCategory = reader.GetString();
        ClipName = reader.GetString();
        Position = reader.GetUnmanaged<Vector3>();
        Volume = reader.GetFloat();
        Rolloff = reader.GetInt();
    }

    public override string ToString()
    {
        return $"AmbienceEventPacket({Position}, {SoundType}, {SoundCategory}, {ClipName})";
    }
}
