using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Xml;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.Utils
{
    public class ManageDownloadedFiles
    {
        #region To_Read_Exported_or_Extracted_file

        [TxAction("GetLastDownloadedFile", "")]
        public string GetLastDownloadedFile(string DownloadDirectory)
        {
            try
            {
                Thread.Sleep(2000);
                return GetNewestFile(new DirectoryInfo(DownloadDirectory)).FullName;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(DownloadDirectory + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("GetLastDownloadedFileName", "")]
        public string GetLastDownloadedFileName(string DownloadDirectory)
        {
            try
            {
                Thread.Sleep(2000);
                return GetNewestFile(new DirectoryInfo(DownloadDirectory)).Name;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(DownloadDirectory + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }
        [TxAction("ReadFileListWithinZipContains", "")]
        public bool ReadFileListWithinZipContains(string filePath, int index, string expectedvalue)
        {
            try
            {
                List<string> fileList = new List<string>();

                using (var zip = ZipFile.OpenRead(filePath))
                {
                    foreach (var entry in zip.Entries)
                        fileList.Add(entry.Name);
                }
                if (fileList[index].Contains(expectedvalue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return false;
        }
        public static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            try
            {
                return directory.GetFiles()
                .Union(directory.GetDirectories().Select(d => GetNewestFile(d)))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(directory + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("GetFileSize", "")]
        public string GetFileSize(string filePath)
        {
            try
            {
                FileInfo fileInformation = new FileInfo(filePath);
                var size = (fileInformation.Length / 1024);
                return (fileInformation.Length / 1024).ToString();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("ReadExcelFile", "")]
        public ActionColl<string> ReadExcelFile(string filePath, string sheetName, int row, int uptoCol)
        {
            try
            {
                List<String> data = new List<string>();

                var datas = File.ReadAllText(filePath);

                if (filePath.Contains(".csv"))
                {
                    foreach (string line in datas.Split('\n'))
                        data.Add(line.Replace("\t", "").Replace("\r", "").Replace(" ", ""));
                    // data.Add(line);
                    return new ActionColl<string>(data);
                }

                else
                {
                    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet result = excelReader.AsDataSet();
                    DataTable table;
                    if (sheetName.All(Char.IsDigit))
                        table = result.Tables[Convert.ToInt32(sheetName)];
                    else
                        table = result.Tables[sheetName];
                    for (int col = 1; col <= uptoCol; col++)
                    {
                        data.Add(table.Rows[row - 1][col].ToString());
                    }
                    stream.Close();
                    return new ActionColl<string>(data);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("ReadExcelFileFromZeroIndex", "")]
        public ActionColl<string> ReadExcelFileFromZeroIndex(string filePath, string sheetName, int row, int uptoCol)
        {
            try
            {
                List<String> data = new List<string>();

                var datas = File.ReadAllText(filePath);

                if (filePath.Contains(".csv"))
                {
                    foreach (string line in datas.Split('\n'))
                        data.Add(line.Replace("\t", "").Replace("\r", "").Replace(" ", ""));
                    // data.Add(line);
                    return new ActionColl<string>(data);
                }

                else
                {
                    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet result = excelReader.AsDataSet();
                    DataTable table;
                    if (sheetName.All(Char.IsDigit))
                        table = result.Tables[Convert.ToInt32(sheetName)];
                    else
                        table = result.Tables[sheetName];
                    for (int col = 0; col <= uptoCol; col++)
                        data.Add(table.Rows[row - 1][col].ToString());
                    stream.Close();
                    return new ActionColl<string>(data);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }
        
        [TxAction("ReadExportedXMLFile", "")]
        public ActionColl<string> ReadExportedXMLFile(string filePath, string objectName, string attrId)
        {
            List<string> attrData = new List<string>();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);
                XmlNode objsNode = xmlDocument.SelectSingleNode("//Objs");
                XmlNodeList DataObjNodeList = objsNode.HasChildNodes ? objsNode.ChildNodes : null;
                if (DataObjNodeList != null)
                    foreach (XmlNode dataObjNode in DataObjNodeList)
                        foreach (XmlAttribute attr in dataObjNode.Attributes)
                            if (attr.Name == "sName")
                                if (attr.Value == objectName)
                                {
                                    XmlNodeList dataNodeList = dataObjNode.HasChildNodes ? dataObjNode.ChildNodes : null;
                                    if (dataNodeList != null)
                                        foreach (XmlNode dataNode in dataNodeList)
                                            if (dataNode.Name == "Data")
                                            {
                                                XmlNodeList attributeNodeList = dataNode.HasChildNodes ? dataNode.ChildNodes : null;
                                                foreach (XmlNode attrNode in attributeNodeList)
                                                {
                                                    foreach (XmlAttribute attrOfattrNode in attrNode.Attributes)
                                                    {
                                                        if (attrOfattrNode.Name == "ID_Att" && attrOfattrNode.Value == attrId)
                                                        {
                                                            for (int i = 1; i < attrNode.Attributes.Count; i++)
                                                                attrData.Add(attrNode.Attributes.Item(i).Value);

                                                            return new ActionColl<string>(attrData);
                                                        }
                                                    }
                                                }
                                            }
                                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        //[TxAction("ReadFileListWithinZip", "")]
        //public ActionColl<string> ReadFileListWithinZip(string filePath)
        //{
        //    try
        //    {
        //        List<string> fileList = new List<string>();

        //        using (var zip = ZipFile.Read(filePath))
        //        {
        //            foreach (var entry in zip.Entries)
        //                fileList.Add(entry.FileName);
        //        }
        //        return new ActionColl<string>(fileList);
        //    }
        //    catch (FileNotFoundException)
        //    {
        //        Console.WriteLine(filePath + " is missing.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("There is a critical error : " + ex.ToString());
        //    }
        //    return null;
        //}

        [TxAction("IsXmlTagPresent", "")]
        public bool IsXmlTagPresent(string filePath, String nodePath)
        {
            //filePath = @"C:\Users\User\Downloads\2019-08-19_16-46-02_Export.xml";
            try
            {
                XmlDocument xmlDocumment = new XmlDocument();
                xmlDocumment.Load(filePath);
                return xmlDocumment.SelectSingleNode(nodePath) != null ? true : false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return false;
        }

        [TxAction("IsXmlChildTagPresent", "")]
        public bool IsXmlChildTagPresent(string filePath, String parentNodePath, String childNodeName)
        {
            //filePath = @"C:\Users\User\Downloads\2019-08-19_16-46-02_Export.xml";
            try
            {
                XmlDocument xmlDocumment = new XmlDocument();
                xmlDocumment.Load(filePath);
                XmlNode parentNode = xmlDocumment.SelectSingleNode(parentNodePath);
                List<string> childNodes = GetChildXmlNodeName(parentNode);
                return (childNodes != null && childNodes.Contains(childNodeName)) ? true : false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return false;
        }

        [TxAction("ReadXmlAttrValueByName", "")]
        public string ReadXmlAttrValueByName(string filePath, String nodePath, string attributeName)
        {
            //filePath = @"C:\Users\User\Downloads\2019-08-19_16-46-02_Export.xml";
            filePath = filePath.Replace(".crdownload","");
            try
            {
                XmlDocument xmlDocumment = new XmlDocument();
                xmlDocumment.Load(filePath);
                XmlNode parentNode = xmlDocumment.SelectSingleNode(nodePath);
                foreach (XmlAttribute attribute in parentNode.Attributes)
                    if (attribute.Name == attributeName)
                        return attribute.Value;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("ReadXmlTagValue", "")]
        public string ReadXmlTagValue(string filePath, String nodePath)
        {
            //filePath = @"C:\Users\User\Downloads\2019-08-19_16-46-02_Export.xml";
            try
            {
                XmlDocument xmlDocumment = new XmlDocument();
                xmlDocumment.Load(filePath);
                XmlNode parentNode = xmlDocumment.SelectSingleNode(nodePath);
                return parentNode.InnerText.Replace("\r","").Replace("\n","") ;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return null;
        }

        [TxAction("ReadXmlAttributesValue", "")]
        public ActionColl<string> ReadXmlAttributesValue(string filePath, String nodePath)
        {
            //filePath = @"C:\Users\User\Downloads\2019-08-19_16-46-02_Export.xml";
            List<string> attributevalue = new List<string>();
            try
            {
                XmlDocument xmlDocumment = new XmlDocument();
                xmlDocumment.Load(filePath);
                XmlNode parentNode = xmlDocumment.SelectSingleNode(nodePath);
                foreach (XmlAttribute attribute in parentNode.Attributes)
                    attributevalue.Add(attribute.Value);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(filePath + " is missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is a critical error : " + ex.ToString());
            }
            return new ActionColl<string>(attributevalue);
        }

        public List<XmlNode> GetChildXmlNode(XmlNode xmlNode)
        {
            List<XmlNode> childNodes = new List<XmlNode>();
            foreach (XmlNode subNode in xmlNode.ChildNodes)
                if (!(Equals(subNode.Name, "DTxt") || Equals(subNode.Name, "DStr") || Equals(subNode.Name, "DURL") || Equals(subNode.Name, "TextValues")))
                    childNodes.Add(subNode);
            return childNodes;
        }

        public List<string> GetChildXmlNodeName(XmlNode xmlNode)
        {
            List<string> childNodes = new List<string>();
            foreach (XmlNode subNode in xmlNode.ChildNodes)
                if (!(Equals(subNode.Name, "DTxt") || Equals(subNode.Name, "DStr") || Equals(subNode.Name, "DURL") || Equals(subNode.Name, "TextValues")))
                    childNodes.Add(subNode.Name);
            return childNodes;
        }

        #endregion
    }
}
