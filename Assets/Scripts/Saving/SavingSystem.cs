using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace RPG.Saving
{
  public class SavingSystem : MonoBehaviour
  {
    public void Save(string saveName)
    {
      string path = GetPathFromSaveName(saveName);
      print("Saving to: " + path);
      using (FileStream stream = File.Open(path, FileMode.Create))
      {
        Transform playerTransform = GetPlayerTransform();
        byte[] buffer = SerializeVector(playerTransform.position);
        stream.Write(buffer, 0, buffer.Length);
      }
    }
    public void Load(string saveName)
    {
      string path = GetPathFromSaveName(saveName);
      print("Loading from: " + path);
      using (FileStream stream = File.Open(path, FileMode.Open))
      {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        Vector3 positionFromFile = DeserializeVector(buffer);
        GetPlayerTransform().position = positionFromFile;
      }
    }

    private Transform GetPlayerTransform()
    {
      return GameObject.FindWithTag("Player").transform;
    }

    private byte[] SerializeVector(Vector3 vector)
    {
      byte[] vectorBytes = new byte[3 * 4]; //vector3 has 3 floats, each float is 4 bytes
      BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
      BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
      BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

      return vectorBytes;
    }

    private Vector3 DeserializeVector(byte[] buffer)
    {
      Vector3 deserializedVector = new Vector3();
      deserializedVector.x = BitConverter.ToSingle(buffer, 0);
      deserializedVector.y = BitConverter.ToSingle(buffer, 4);
      deserializedVector.z = BitConverter.ToSingle(buffer, 8);

      return deserializedVector;
    }

    private string GetPathFromSaveName(string saveName)
    {
      return Path.Combine(Application.persistentDataPath, saveName + ".sav");
    }
  }
}
