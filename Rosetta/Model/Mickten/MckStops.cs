using System;
using System.Collections.Generic;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.DB_Reader;
using Rosetta.DB_Reader.Blasewitz_DataModel;
using Rosetta.Model.Trachau;

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

namespace Rosetta.Model.Mickten
{
    public class MckStops
    {
        //private List<MckStop> __iList;
        private PuTModel __parent;
        private readonly BlaCommonStorage __root;

        public MckStops(PuTModel puTModel, BlaCommonStorage root)
        {
            __parent = puTModel;
            __root = root;
        }

        public IEnumerable<MckStop> Items =>
            __root.Graph.Nodes.Items.Where(n => n.HasHalteStelle).Select(n => n.HalteStelle);

        public bool Load(DBConnectionPool cpool)
        {
            var b = true;
            try
            {
                __root.Reporter.AddInfo("Odczyt definicji przystanków");
                var hsdef = __root.EntityReader[401].Query(cpool, __root.Variant)
                    .Select(r => new
                    {
                        lx4010 = r.GetIntNumber(0), lx4060 = r.GetIntNumber(1), Nazwa = r.GetString(2),
                        LX4002 = r.GetIntNumber(3)
                    }).Join(__root.Graph.Nodes.Items, h => h.lx4010, no => no.ObjectId,
                        (h, no) => new {node = no, h.lx4010, h.Nazwa, h.lx4060, h.LX4002})
                    .ToUniqueList(kT => kT.lx4010);
                __root.Reporter.AddInfo("Odczyt TSys dla przystanków");
                var hstsys = __root.EntityReader[402].Query(cpool, __root.Variant)
                    .Select(r => new {lx4010 = r.GetIntNumber(0), lx3121 = r.GetIntNumber(1)}).Join(
                        __root.DemFoundation.TSysList.PuT(), h => h.lx3121, pu => pu.ID,
                        (h, pu) => new {h.lx4010, tsys = pu});
                var hs = hsdef.GroupJoin(hstsys, d => d.lx4010, h => h.lx4010, (d, hmany) => new {d, hmany}).ToList();
                foreach (var h in hs)
                {
                    var st = new MckStop(h.d.node);
                    st.Name = h.d.Nazwa;
                    st.TypeNo = h.d.lx4060;
                    st.HaltOfTSys = new TSysSet(h.hmany.Select(t => t.tsys));
                }
            }
            catch (Exception e)
            {
                __root.Reporter.AddException(e);
                b = false;
            }

            return b;
        }
    }
}