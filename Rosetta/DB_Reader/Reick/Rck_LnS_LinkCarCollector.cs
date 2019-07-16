using System.Data.SqlClient;
using Rosetta.Model.Kaditz;

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

namespace Rosetta.DB_Reader.Reick
{
    internal class Rck_LnS_LinkCarCollector : Rck_LinkCollcetor_OfT<KdzCarLink>, IReickQuelle
    {
        public Rck_LnS_LinkCarCollector() : base(2020, 2021, 2022)
        {
        }
        //public override bool Prepare()
        //{
        //    bool b = true;
        //    try
        //    {
        //        using (var rea = _platform.EntityReader[_johEntries[0]].Reader(_cpool, _platform.Variant))
        //        {
        //            while (rea.Read())
        //            {
        //                var y = _MakeLink(rea);
        //                _platform.Graph.Links.Add(y);
        //                _ilist.Add(y);
        //            }
        //        }
        //        _platform.Reporter.AddMessage(new RoReMessage(){Message = "Wczytano definicje krawędzi", EvType = RoReEventType.Info});
        //        var rsysPrT = _platform.EntityReader[_johEntries[1]].Query(_cpool, _platform.Variant)
        //            .Select(n => new {obj = n.GetIntNumber(0), sys = n.GetIntNumber(1)}).ToList();
        //        _platform.Reporter.AddInfo("Odczytano środki transportu PrT");
        //        var stage2PrT = rsysPrT.Join(_platform.DemFoundation.TSysList.PrT(), rs => rs.sys, ts => ts.ID,
        //            (rs, ts) => new {rs.obj, tsys = ts});
        //        _platform.Reporter.AddInfo("Wybrano środki transportu PrT");

        //        var rsysPuT = _platform.EntityReader[_johEntries[2]].Query(_cpool, _platform.Variant)
        //            .Select(n => new { obj = n.GetIntNumber(0), sys = n.GetIntNumber(1) }).ToList();
        //        _platform.Reporter.AddInfo("Odczytano środki transportu PuT");
        //        var stage2PuT = rsysPuT.Join(_platform.DemFoundation.TSysList.PuT(), rs => rs.sys, ts => ts.ID,
        //            (rs, ts) => new { rs.obj, tsys = ts });
        //        _platform.Reporter.AddInfo("Wybrano środki transportu PuT");

        //        var stage2 = stage2PrT.Concat(stage2PuT);

        //        b = _ilist.GroupJoin(stage2, l => l.Id, sy => sy.obj, (l, sy) =>
        //        {
        //            return l.AddTSyses(sy.Select(v => v.tsys));
        //        }).All(u => u);

        //    }
        //    catch (SqlNullValueException e)
        //    {
        //        //e.StackTrace
        //        _platform.Reporter.AddException(e, this.GetType().Name);
        //        b = false;
        //    }
        //    catch (SqlException e)
        //    {
        //        e.Errors.Cast<SqlError>().ToList().ForEach(c => _platform.Reporter.AddMessage(new RoReMessage(){Caller = GetType().Name, EvType = RoReEventType.Warning, Message = c.ToString()}));
        //        _platform.Reporter.AddException(e, this.GetType().Name);
        //        b = false;
        //    }
        //    catch (Exception e)
        //    {
        //        _platform.Reporter.AddException(e, this.GetType().Name);
        //        b = false;
        //    }
        //    return b;
        //}

        protected override KdzCarLink _MakeLink(SqlDataReader rea)
        {
            return new KdzCarLink(rea, _platform.Graph.Nodes);
        }
    }
}