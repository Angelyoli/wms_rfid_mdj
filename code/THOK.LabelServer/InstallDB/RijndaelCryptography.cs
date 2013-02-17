﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace install
{
    /// <summary>
    /// Simple Rijndael implementation
    /// </summary>
    internal class RijndaelCryptography
    {
        RijndaelManaged myRijndael;
        ASCIIEncoding textConverter;
        byte[] fromEncrypt;
        byte[] encrypted;
        byte[] toEncrypt;
        byte[] _key;
        byte[] _IV;

        internal byte[] Key
        {
            get { return _key; }
            set { _key = value; }
        }
        internal byte[] IV
        {
            get { return _IV; }
            set { _IV = value; }
        }
        internal byte[] Encrypted
        {
            get { return encrypted; }
        }

        internal RijndaelCryptography()
        {
            myRijndael = new RijndaelManaged();
            myRijndael.Mode = CipherMode.CBC;
            textConverter = new ASCIIEncoding();
        }

        internal virtual void GenKey()
        {
            //Create a new key and initialization vector.
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();

            //Get the key and IV.
            _key = myRijndael.Key;
            _IV = myRijndael.IV;
        }

        internal void Encrypt(string TxtToEncrypt)
        {
            //Get an encryptor.
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(_key, _IV);

            //Encrypt the data.
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor,
                                                      CryptoStreamMode.Write);

            //Convert the data to a byte array.
            toEncrypt = textConverter.GetBytes(TxtToEncrypt);

            //Write all data to the crypto stream and flush it.
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            //Get encrypted array of bytes.
            encrypted = msEncrypt.ToArray();
        }

        internal string Decrypt(byte[] crypted)
        {
            //Get a decryptor.
            ICryptoTransform decryptor = myRijndael.CreateDecryptor(_key, _IV);

            //Decrypting the encrypted byte array.
            MemoryStream msDecrypt = new MemoryStream(crypted);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor,
                                                      CryptoStreamMode.Read);

            fromEncrypt = new byte[crypted.Length];

            //Read the data out of the crypto stream.
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            //Convert the byte array into a string.
            return textConverter.GetString(fromEncrypt);


        }
    }

}


