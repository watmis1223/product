﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ProductCalculation.Library.Entity.Setting;
using ProductCalculation.Library.Entity.Setting.PriceCalculation;

namespace ProductCalculation.Library.Storage
{
    static class ApplicationOperator
    {                
        static string priceCalculationSettingPath = string.Format("{0}\\{1}", Application.StartupPath, "application.pricecalculation.setting");

        public static PriceCalculationSetting GetPriceCalculationSetting()
        {
            PriceCalculationSetting priceCalculationSetting = null;
            GetDeserialize<PriceCalculationSetting>(priceCalculationSettingPath, ref priceCalculationSetting);

            if (priceCalculationSetting == null)
            {
                return new PriceCalculationSetting();
            }

            return priceCalculationSetting;
        }

        public static void SavePriceCalculationSetting(PriceCalculationSetting priceCalculationSettin)
        {
            SeSerialize<PriceCalculationSetting>(priceCalculationSettingPath, priceCalculationSettin);
        }

        static void SeSerialize<T>(string filePath, object serializeObject)
        {
            DataContractSerializer serializer = null;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer = new DataContractSerializer(serializeObject.GetType());
                serializer.WriteObject(fileStream, serializeObject);
                fileStream.Close();
                serializer = null;
            }

            XmlDocument xmlDocument = null;
            xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            xmlDocument.Save(filePath);
            xmlDocument = null;
        }

        static void GetDeserialize<T>(string filePath, ref T deserializeObject)
        {
            XmlDictionaryReader xmlReader = null;
            DataContractSerializer deSerializer = null;            

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    xmlReader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
                    deSerializer = new DataContractSerializer(typeof(T));
                    //deSerializer = new DataContractSerializer(typeof(DataTable), knownTypes, 2147483647, false, true, null);
                    deserializeObject = (T)deSerializer.ReadObject(xmlReader, true);
                    xmlReader.Close();
                    fileStream.Close();
                }
            }
            catch
            {
                xmlReader = null;
                deSerializer = null;                
            }            
        }
    }
}
