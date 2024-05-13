using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HotelRoomManagementApp
{
    public class EncryptionService
    {


        public string Encrypt(string plainText)
        {
            try
            {

                byte[] encryptedData = Encoding.UTF8.GetBytes(plainText);
                string encryptedString = Convert.ToBase64String(encryptedData);

                return encryptedString;
            }
            catch (FormatException)
            {
                // If the input is not a valid Base64 string, return it as is
                return plainText;
            }
        }

        public string Decrypt(string cipherText)
        {

            try
            {
                byte[] decryptedData = Convert.FromBase64String(cipherText);
                string decryptedString = Encoding.UTF8.GetString(decryptedData);

                return decryptedString;
            }
            catch (FormatException)
            {
                // If the input is not a valid Base64 string, return it as is
                return cipherText;
            }

        }
    }
}
