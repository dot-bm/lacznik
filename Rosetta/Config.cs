using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Rosetta.DB_Reader;
using Rosetta.KarlsrurerStr;

//Łącznik – DB entries to Model Transfer File/Network converter
//(c) 2017 Bartłomiej Mikulski
//https://github.com/dot-bm/lacznik
//
//This file is part of Łącznik.
//
//Łącznik is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation; either version 3 of the License, or
//(at your option) any later version.
//
//Łącznik is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public License
//along with Łącznik.  If not, see <http://www.gnu.org/licenses/>.
//
//Ten plik jest częścią Łącznika.
//
//Łącznik jest wolnym oprogramowaniem: możesz go rozprowadzać dalej
//i/lub modyfikować na warunkach Mniejszej Powszechnej Licencji Publicznej GNU,
//wydanej przez Fundację Wolnego Oprogramowania - według wersji 3 tej
//Licencji lub (według twojego wyboru) którejś z późniejszych wersji.
//
//Łącznik rozpowszechniany jest z nadzieją, iż będzie on
//użyteczny - jednak BEZ JAKIEJKOLWIEK GWARANCJI, nawet domyślnej
//gwarancji PRZYDATNOŚCI HANDLOWEJ albo PRZYDATNOŚCI DO OKREŚLONYCH
//ZASTOSOWAŃ. W celu uzyskania bliższych informacji sięgnij do Mniejszej Powszechnej Licencji Publicznej GNU.
//
//Z pewnością wraz z Łącznikiem otrzymałeś też egzemplarz
//Mniejszej Powszechnej Licencji Publicznej GNU (GNU Lesser General Public License).
//Jeśli nie - zobacz <http://www.gnu.org/licenses/>. 

namespace Rosetta
{
    public class Config2
    {
        private XDocument __xd;

        public int EpSg => int.Parse(Common("epsg"));

        public string Root => Common("root");

        public string VersionFileName => Path.Combine(Root, VF_PfdHelper.PfdEntryByExt("ver"), "output.ver");

        public string TraFile => Path.Combine(Root, VF_PfdHelper.PfdEntryByExt("tra"), "output.tra");

        private XDocument __loadXml()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
                    Environment.SpecialFolderOption.None);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = Path.Combine(path, @"Lacznik\Config.xml");
                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        __xd = XDocument.Load(fs);
                    }
                }
            }
            catch
            {
            }

            return __xd;
        }

        private XDocument __content()
        {
            return __xd ?? __loadXml();
        }

        public string Epsg(string epsgId = null)
        {
            var ePsg = string.IsNullOrWhiteSpace(epsgId) ? Common("epsg") : epsgId;
            if (string.IsNullOrWhiteSpace(ePsg)) return "";
            return __content()?.Document?.Descendants("Spatial")?.SingleOrDefault()?.Elements("epsg")
                       ?.FirstOrDefault(q => q.Attribute("id")?.Value == ePsg)?.Value ?? "";
        }

        public string Common(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "";
            return __content()?.Document?.Descendants("Mock")?.SingleOrDefault()?.Elements("Variable")
                       .FirstOrDefault(q => q.Attribute("key")?.Value == key)?.Value ?? "";
        }

        public string Templater(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "";
            return __content()?.Document?.Descendants("Templates")?.SingleOrDefault()?.Elements("Template")
                       .FirstOrDefault(q => q.Attribute("key")?.Value == key)?.Value ?? "";
        }

        public DBConnectionPool ConnectionPool(byte? connId = null)
        {
            var connID = connId == null ? Common("ActiveConn") : connId.ToString();
            var dbset = __content()?.Document?.Descendants("Connections")?.SingleOrDefault()?.Elements("Connection")
                .SingleOrDefault(q => q?.Attribute("ID")?.Value == connID);
            return new DBConnectionPool
            {
                Server = dbset?.Elements("Variable")?.FirstOrDefault(q => q?.Attribute("key")?.Value == "serverName")
                             ?.Value ?? "",
                Database = dbset?.Elements("Variable")?.FirstOrDefault(q => q?.Attribute("key")?.Value == "database")
                               ?.Value ?? "",
                TimeOut = int.Parse(dbset?.Elements("Variable")
                                        ?.FirstOrDefault(q => q?.Attribute("key")?.Value == "timeout")?.Value ?? "0")
            };
        }

        //private class C_Old
        //{

        //}

        //public 
    }

    /// <summary>
    ///     Depreciated
    /// </summary>
    public class Config
    {
        public readonly Inx1 Entry;

        //XDocument __xdocument = 
        private readonly Dictionary<string, string> __cfg = new Dictionary<string, string>();
        private XDocument __xdocument;

        public Config()
        {
            Entry = new Inx1(this);
            try
            {
                __LoadXml();
            }
            catch (Exception e)
            {
            }
        }

        public string Root => __cfg["root"];

        public string VersionFileName => Root + "\\" + VF_PfdHelper.PfdEntryByExt("ver") + "\\output.ver";

        public string TraFile => __cfg["root"] + "\\" + VF_PfdHelper.PfdEntryByExt("tra") + "\\output.tra";

        public void SimpleFillCfg(Dictionary<string, string> inpDict)
        {
            foreach (var n in inpDict) __cfg.Add(n.Key, n.Value);
            //__cfg = new Dictionary<string,string>(
        }


        /// <summary>
        ///     Wczytuje plik konfiguracyjny z AppData
        /// </summary>
        public void LoadFromFile()
        {
            if (__cfg.Count > 0) return;
            var mock = __xdocument.Document.Descendants("Mock").Single();
            foreach (var e in mock.Elements("Variable"))
                __cfg.Add(e.Attribute("key").Value, e.Attribute("value").Value);
        }

        private void __LoadXml()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.None);
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(path, @"Lacznik\Config.xml");
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    //XmlTextReader xr = new XmlTextReader(fs);
                    //XmlDocument __xdocument = new XmlDocument();
                    //__xdocument.Load(fs);
                    __xdocument = XDocument.Load(fs);
                    //var t = mock.Elements("Variable").First();                    
                }
            }
        }

        public string GetEPSGDef(string epsgId)
        {
            return __xdocument.Document.Descendants("Spatial").Single().Elements("epsg")
                .First(q => q.Attribute("id").Value == epsgId).Value;
        }

        //public string GetDictEntry(string key)
        //{
        //    return __cfg[key];
        //}

        public class Inx1
        {
            private readonly Config __y;

            internal Inx1(Config y)
            {
                __y = y;
            }

            public string this[string key] => __y.__cfg[key];
        }
    }
}