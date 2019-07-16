using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    ///     Poszczególne przebiegi
    /// </summary>
    [XmlType("Batch")]
    public class BlaReqBatch
    {
        private List<BlaResultRow> __results;

        private byte __rindexer;

        private MemoryStream __streamedResult;

        [XmlAttribute("Mode")] public Blasewitz.Bla_WorkMode Mode;

        [XmlArray("PhaS")] [XmlArrayItem("Pha")]
        public byte[] PhaIDs;


        public BlaReqBatch()
        {
        }

        public BlaReqBatch(byte streamId, byte[] phas, Blasewitz.Bla_WorkMode mode)
        {
            PhaIDs = phas;
            BatchId = streamId;
            Mode = mode;
        }

        [XmlIgnore] public Stream Result => __streamedResult ?? __ResultsToStream();

        [XmlIgnore] public IEnumerable<BlaResultRow> Results => __results;

        [XmlIgnore] public bool HasResults => __results != null && __results.Count > 0;

        [XmlAttribute("File")] public string FileName { get; set; }

        [XmlAttribute("Stream")] public byte BatchId { get; set; }

        private Stream __ResultsToStream()
        {
            var b = HasResults;
            if (!b) return null;
            __streamedResult = new MemoryStream();
            using (var sw = new StreamWriter(__streamedResult, bufferSize: 1024, leaveOpen: true,
                encoding: Encoding.Unicode))
            {
                b = Results.OrderBy(a => a.EntityID).ThenBy(a => a.OrdNum).Select(a => a.Content + "\n").All(n =>
                {
                    sw.Write(n);
                    return true;
                });
                sw.Flush();
            }

            if (!b) return null;
            __streamedResult.Seek(0, SeekOrigin.Begin);
            return __streamedResult;
        }

        public void AddResults(IEnumerable<BlaResultRow> nRows)
        {
            if (__results == null) __results = new List<BlaResultRow>();
            var i = __rindexer++;
            __results.AddRange(nRows.Select(a => a.RemapId(i)));
        }

        public bool ContainsPha(byte phaId)
        {
            return PhaIDs.Any(p => p == phaId);
        }
    }
}