using System;
using System.Collections.Generic;
using System.Linq;

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

namespace Rosetta.KarlsrurerStr
{
    public static class VF_PfdHelper
    {
        private static readonly List<Tuple<string, string, string>> __visPfdDef;

        static VF_PfdHelper()
        {
            __visPfdDef = new List<Tuple<string, string, string>>();
            __visPfdDef.Add(new Tuple<string, string, string>("Project directories", "pfd", "Projektverzeichnisse"));
            __visPfdDef.Add(new Tuple<string, string, string>("Version", "ver", "Versionen"));
            __visPfdDef.Add(new Tuple<string, string, string>("Network", "net", "Netze"));
            __visPfdDef.Add(new Tuple<string, string, string>("OD demand data", "dmd", "Nachfragedaten"));
            __visPfdDef.Add(new Tuple<string, string, string>("Scenario management project", "vpdb;vpdbx",
                "Szenariomanagement-Projekt"));
            __visPfdDef.Add(new Tuple<string, string, string>("Matrix", "mtx;mx;fma;*", "Matrizen"));
            __visPfdDef.Add(new Tuple<string, string, string>("Access database", "mdb", "Access-Datenbank"));
            __visPfdDef.Add(
                new Tuple<string, string, string>("Access 2007 database", "accdb", "Access-2007-Datenbank"));
            __visPfdDef.Add(new Tuple<string, string, string>("Model transfer file", "tra", "Modelltransfer-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("ESRI shapefile", "shp", "Shapefile"));
            __visPfdDef.Add(new Tuple<string, string, string>("Attributes", "att", "Attribute"));
            __visPfdDef.Add(new Tuple<string, string, string>("Active network objects", "ane", "Aktive Netzelemente"));
            __visPfdDef.Add(new Tuple<string, string, string>("Filter", "fil", "Filter"));
            __visPfdDef.Add(
                new Tuple<string, string, string>("Procedure parameters", "par;xml", "Verfahrensparameter"));
            __visPfdDef.Add(new Tuple<string, string, string>("AddIn", "vai", "Visum AddIns"));
            __visPfdDef.Add(new Tuple<string, string, string>("Script", "vbs;js;pys;py;rb;pl;tcl", "Skript-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("Other input data", "*", "Sonstige Eingabedaten"));
            __visPfdDef.Add(new Tuple<string, string, string>("Other output data", "*", "Sonstige Ausgabedaten"));
            __visPfdDef.Add(new Tuple<string, string, string>("Graphic parameters", "gpa;gpax", "Grafikparameter"));
            __visPfdDef.Add(new Tuple<string, string, string>("Background",
                "emf;wmf;bmp;dwg;dxf;ecw;jp2;jpg;png;shp;sid;svg;tga;tif;gif", "Hintergruende"));
            __visPfdDef.Add(new Tuple<string, string, string>("Texts", "txt", "Texte"));
            __visPfdDef.Add(new Tuple<string, string, string>("Image", "bmp;jpg;wmf;emf;gif;tiff;png", "Bilddateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("SVG file", "svg", "SVG-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("DXF file", "dxf", "DXF-Dateien"));
            __visPfdDef.Add(
                new Tuple<string, string, string>("Screenshot", "jpg;wmf;emf;bmp;gif;tiff;png", "Screenshots"));
            __visPfdDef.Add(new Tuple<string, string, string>("Exported turn volumes",
                "jpg;png;gif;wmf;emf;bmp;tiff;svg", "Exportierte Knotenströme"));
            __visPfdDef.Add(new Tuple<string, string, string>("Legend parameters", "lgd", "Legenden-Parameter"));
            __visPfdDef.Add(new Tuple<string, string, string>("Timetable graphic parameters", "gpt;gpbe;gpgt;gptt",
                "Fahrplan-Grafikparameter"));
            __visPfdDef.Add(new Tuple<string, string, string>("Matrix editor graphic parameters", "gpm",
                "Grafikparameter Matrixeditor"));
            __visPfdDef.Add(new Tuple<string, string, string>("Schematic line diagram graphic parameters", "gpsld",
                "Grafikparameter Schematischer Liniennetzplan"));
            __visPfdDef.Add(new Tuple<string, string, string>(
                "Transfers display of regular services - graphic parameters", "gpta",
                "Grafikparameter Umsteiger-Taktdarstellung"));
            __visPfdDef.Add(new Tuple<string, string, string>("Timetable layout", "tly;tlbe;tltt;tlls;tlbs",
                "Fahrplan-Layout"));
            __visPfdDef.Add(new Tuple<string, string, string>("List layout", "lla", "Listen-Layout"));
            __visPfdDef.Add(new Tuple<string, string, string>("Quickview layout", "qla", "Schnellansichts-Layout"));
            __visPfdDef.Add(new Tuple<string, string, string>("Matrix editor layout", "mly", "Matrixeditor-Layout"));
            __visPfdDef.Add(new Tuple<string, string, string>("Survey data", "*", "Befragungsdaten"));
            __visPfdDef.Add(new Tuple<string, string, string>("PuT connections", "con", "Verbindungsdatei"));
            __visPfdDef.Add(new Tuple<string, string, string>("PrT routes", "rim", "Routen-Import"));
            __visPfdDef.Add(new Tuple<string, string, string>("EMME project", "emme", "Emme"));
            __visPfdDef.Add(new Tuple<string, string, string>("PuT interfaces project", "putp;puti;haf",
                "ÖV-Schnittstellen-Parameterdateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("TModel project", "tla", "TModel-Dateien"));
            __visPfdDef.Add(
                new Tuple<string, string, string>("Network merge parameters", "nmp", "Netzvereinigungs-Para"));
            __visPfdDef.Add(new Tuple<string, string, string>("Parameters for Read network additively", "anrp",
                "Additives-Netzlesen-Para"));
            __visPfdDef.Add(new Tuple<string, string, string>("Parameters for Read demand additively", "adrp",
                "Additives-Nachfragelesen-Para"));
            __visPfdDef.Add(new Tuple<string, string, string>("Parameters for matrix operations", "xml;cod",
                "Parameter für Matrixoperationen"));
            __visPfdDef.Add(new Tuple<string, string, string>("RASW file", "rwf", "RASW-Fall"));
            __visPfdDef.Add(new Tuple<string, string, string>("Log file", "*", "Log-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("Projection", "prj", "Projections"));
            __visPfdDef.Add(new Tuple<string, string, string>("Script menu file", "xml", "Skriptmenue-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("User VDF DLLs", "dll", "UserVDF-DLLs"));
            __visPfdDef.Add(new Tuple<string, string, string>("Intervals", "att;cod", "Intervalle"));
            __visPfdDef.Add(new Tuple<string, string, string>("MUULI Log file", "mlg", "MUULI-Log-Dateien"));
            __visPfdDef.Add(new Tuple<string, string, string>("User preferences", "xml", "Benutzereinstellungen"));
            __visPfdDef.Add(new Tuple<string, string, string>("Combination", "*", "Verknuepfungen"));
        }

        public static string PfdEntryByExt(string ext)
        {
            return __visPfdDef.First(q => q.Item2.Split(';').Any(q1 => q1 == ext)).Item1;
        }

        public static IEnumerable<Tuple<string, string, string>> EnumValues()
        {
            return __visPfdDef.Select(a => a);
        }
    }
}