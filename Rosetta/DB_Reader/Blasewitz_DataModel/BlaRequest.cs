using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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

namespace Rosetta.DB_Reader.Blasewitz_DataModel
{
    /// <summary>
    ///     Całość żądania wykonania
    /// </summary>
    [XmlRoot("Request")]
    [XmlInclude(typeof(BlaReqBatch))]
    [XmlInclude(typeof(BlaReqSliceDefinition))]
    public class BlaRequest
    {
        [XmlArray("Batches")] [XmlArrayItem("Batch")]
        public List<BlaReqBatch> Batches = new List<BlaReqBatch>();

        [XmlArray("Quelles")] [XmlArrayItem("Quelle")]
        public string[] Reicks;

        [XmlElement("Slice")] public BlaReqSliceDefinition Slice { get; set; }

        [XmlIgnore]
        public IEnumerable<byte> EntityIDs
        {
            get { return Batches.SelectMany(ba => ba.PhaIDs.Select(u => (byte) (u % 100))).Distinct().OrderBy(a => a); }
        }

        public void ToXml(string fileName)
        {
            var serializer = __MakeSerializer();
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fs, this);
                fs.Close();
            }
        }

        private static XmlSerializer __MakeSerializer()
        {
            Type[] RqTypes = {typeof(BlaReqBatch), typeof(byte), typeof(string)};
            var serializer = new XmlSerializer(typeof(BlaRequest), RqTypes);
            return serializer;
        }

        public static BlaRequest LoadFromFile(string path)
        {
            var fs = new FileStream(path, FileMode.Open);
            var serializer = __MakeSerializer();
            return (BlaRequest) serializer.Deserialize(fs);
        }

        public bool ContainsPha(byte phaId)
        {
            return Batches.Any(b => b.ContainsPha(phaId));
        }
    }
}