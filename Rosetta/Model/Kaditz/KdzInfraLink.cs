using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Rosetta.CoreTools;
using Rosetta.CoreTools.Geometry;
using Rosetta.Model.Mickten;
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

namespace Rosetta.Model.Kaditz
{
    public abstract class KdzInfraLink : KdzLink
    {
        public KdzInfraLink(int id, int p1Id, int p2Id, MckNodes nodes, byte type, decimal length) : base(id, p1Id,
            p2Id, nodes, type, length)
        {
        }

        public KdzInfraLink(SqlDataReader inprec, MckNodes nodes) : base(inprec.GetIntNumber(0),
            inprec.GetIntNumber(1), inprec.GetIntNumber(2), nodes, (byte) (inprec.GetIntNumberOrDefault(3) ?? 0),
            inprec.GetFixNumber(6) / 1000m)
        {
            if (inprec.GetIntNumber(4) > 0)
            {
                var gp = new GeoPolyLine(inprec.GetString(5), Length);
                MidPoints = gp.MidPoints().ToList();
            }
        }

        protected abstract Trachau.Trachau.Colligation _MyColligation { get; }

        public bool AddTSyses(IEnumerable<TSys> sy)
        {
            var c = new TSysSet(sy);
            if (c.ColligationOnly(_MyColligation))
            {
                TSysSet = c;
                return true;
            }

            return false;
        }
    }
}