using MessagePack;
using MessagePack.Resolvers;
using PokeCommon.Models;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
namespace Poke.UPDL
{


        public class UPDLParser
    {
        public static byte[] GamePokeToBytes(SimpleGamePokemonTeam gamePokemon)
        {
            var serializedBytes = MessagePackSerializer.Serialize(gamePokemon);

            // Compress the byte array using GZip
            using (var compressedStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gzipStream.Write(serializedBytes, 0, serializedBytes.Length);
                }
                return compressedStream.ToArray();
            }

        }

        public static SimpleGamePokemonTeam DeserializeAndDecompress(byte[] compressedBytes)
        {
            using (var compressedStream = new MemoryStream(compressedBytes))
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                gzipStream.CopyTo(decompressedStream);
                var serializedBytes = decompressedStream.ToArray();
                return MessagePackSerializer.Deserialize<SimpleGamePokemonTeam>(serializedBytes);
            }
        }


        public static SimpleGamePokemonTeam DeserializeAndDecompress(string pokeStr)
        {
            return DeserializeAndDecompress(Convert.FromBase64String(pokeStr));
            using (var compressedStream = new MemoryStream(Encoding.UTF8.GetBytes(pokeStr)))
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                gzipStream.CopyTo(decompressedStream);
                var serializedBytes = decompressedStream.ToArray();
                return MessagePackSerializer.Deserialize<SimpleGamePokemonTeam>(serializedBytes);
            }
        }
    }
}
